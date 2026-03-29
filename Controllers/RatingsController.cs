using KYPlayer.Areas.Identity.Data;
using KYPlayer.Data;
using KYPlayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KYPlayer.Controllers
{
    [Authorize(Roles="Fan")]
    public class RatingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public RatingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int playerId, int value)
        {
            var userId = _userManager.GetUserId(User);

            //  Fan can only rate each player once
            var existing = await _context.Ratings
                .FirstOrDefaultAsync(r => r.PlayerId == playerId && r.FanId == userId);
            if (existing != null)
            {
                TempData["Error"] = "You have already rated this player.";
                return RedirectToAction("Details", "Players", new { id = playerId });
            }

            var rating = new Rating
            {
                PlayerId = playerId,
                FanId = userId,
                Value = value,
                Timestamp = DateTime.UtcNow
            };
            _context.Ratings.Add(rating);

            // Recalculate PSR
            var player = await _context.Players
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);
            player.TotalRatingsCount++;
            player.UpdatePSR();

            await _context.SaveChangesAsync();
                
            return RedirectToAction("Details", "Players", new { id = playerId });
        }


    }
}
