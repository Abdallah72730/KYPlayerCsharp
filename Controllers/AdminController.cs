using KYPlayer.Areas.Identity.Data;
using KYPlayer.Data;
using KYPlayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KYPlayer.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager)
        { _context = context; _userManager = userManager; }

        //Moderate Ratings
        public async Task<IActionResult> Ratings()
        {
            var ratings = await _context.Ratings
                .Include(r => r.Player)
                .Include(r => r.Fan)
                .OrderByDescending(r => r.Timestamp)
                .ToListAsync();
            return View(ratings);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                // Recalculate PSR for affected player
                var player = await _context.Players
                    .Include(p => p.Ratings)
                    .FirstOrDefaultAsync(p => p.PlayerId == rating.PlayerId);
                player.TotalRatingsCount = player.Ratings.Count - 1;
                player.UpdatePSR();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Ratings));
        }

        // Flag or Delete User
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.GetUsersInRoleAsync("Fan");
            return View(users);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null) await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Users));
        }

        //Player Reports
        public async Task<IActionResult> Reports()
        {
            var players = await _context.Players
                .Include(p => p.Ratings)
                .OrderByDescending(p => p.CurrentPSR)
                .ToListAsync();
            return View(players);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(int playerId)
        {
            var player = await _context.Players
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);
            var report = new Report
            {
                PlayerId = playerId,
                PlayerName = player.PlayerName,
                GeneratedAt = DateTime.UtcNow,
                AverageSeasonPSR = player.CurrentPSR
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Reports));
        }

    }
}
