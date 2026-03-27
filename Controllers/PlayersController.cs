using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KYPlayer.Data;
using KYPlayer.Models;
using Microsoft.AspNetCore.Authorization;
using KYPlayer.Models.ViewModels;

namespace KYPlayer.Controllers
{
    [Authorize] //All actions require login
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index(string searchQuery, string positionFilter, string sortBy)
        {
            var players = _context.Players.Include(p => p.Skills).AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
                players = players.Where(p =>
                    p.PlayerName.Contains(searchQuery) ||
                    p.JerseyNumber.ToString().Contains(searchQuery));

            if (!string.IsNullOrEmpty(positionFilter))
                players = players.Where(p => p.Position == positionFilter);

            players = sortBy switch
            {
                "psr_desc" => players.OrderByDescending(p => p.CurrentPSR),
                "psr_asc" => players.OrderBy(p => p.CurrentPSR),
                "name" => players.OrderBy(p => p.PlayerName),
                "jersey" => players.OrderBy(p => p.JerseyNumber),
                _ => players.OrderByDescending(p => p.CurrentPSR), // default
            };

            ViewData["SearchQuery"] = searchQuery;
            ViewData["PositionFilter"] = positionFilter;
            ViewData["SortBy"] = sortBy;
            return View(await players.ToListAsync());
        }


        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() { return View(new PlayerCreateViewModel()); }


        // POST: Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PlayerCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var player = new Player
            {
                PlayerName = vm.PlayerName,
                Age = vm.Age,
                JerseyNumber = vm.JerseyNumber,
                Position = vm.Position,
                PhotoURL = vm.PhotoURL ?? "/images/default-player.png",
                CurrentPSR = 0,    
                TotalRatingsCount = 0,
                Skills = new PlayerSkills
                {
                    Shooting = vm.Shooting,
                    Passing = vm.Passing,
                    Dribbling = vm.Dribbling,
                    Vision = vm.Vision,
                    Defense = vm.Defense,
                    Finishing = vm.Finishing,
                }

            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"{player.PlayerName} has been added to the squad!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Players/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.Include(p => p.Skills).FirstOrDefaultAsync(p => p.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }


            var vm = new PlayerEditViewModel 
            {
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName,
                Age = player.Age,
                JerseyNumber = player.JerseyNumber,
                Position = player.Position,
                PhotoURL = player.PhotoURL,
                CurrentPSR = player.CurrentPSR,
                TotalRatingsCount = player.TotalRatingsCount,
                Shooting = player.Skills?.Shooting ?? 50,
                Passing = player.Skills?.Passing ?? 50,
                Dribbling = player.Skills?.Dribbling ?? 50,
                Vision = player.Skills?.Vision ?? 50,
                Defense = player.Skills?.Defense ?? 50,
                Finishing = player.Skills?.Finishing ?? 50,

            };

            return View(vm);
        }

        // POST: Players/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int id, PlayerEditViewModel vm)
        {
            if (id != vm.PlayerId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
               return View(vm);
            }


            var player = await _context.Players.Include(p => p.Skills).FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null) return NotFound();

            player.PlayerName = vm.PlayerName;
            player.Age = vm.Age;
            player.JerseyNumber = vm.JerseyNumber;
            player.Position = vm.Position;
            player.PhotoURL = vm.PhotoURL ?? player.PhotoURL;


            if (player.Skills == null) 
            {
                player.Skills = new PlayerSkills { PlayerId = player.PlayerId };
            }


            player.Skills.Shooting = vm.Shooting;
            player.Skills.Passing = vm.Passing;
            player.Skills.Dribbling = vm.Dribbling;
            player.Skills.Vision = vm.Vision;
            player.Skills.Defense = vm.Defense;
            player.Skills.Finishing = vm.Finishing;

            _context.Update(player);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"{player.PlayerName} updated successfully.";
            return RedirectToAction(nameof(Index));


        }

        // GET: Players/Delete/5
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}
