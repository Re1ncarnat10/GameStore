using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameStore.Models;
using GameStore.Data;
using GameStore.ViewModels;
using GameStore.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Admin/Manage
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Manage(string sTerm, int genreId, int? id)
        {
            if (id != null)
            {
                return RedirectToAction("Edit", new { id = id });
            }

            var model = new GameDisplayModel
            {
                Genres = await _context.Genres.ToListAsync(),
                Games = await _context.Games
                    .Where(g => (string.IsNullOrEmpty(sTerm) || g.GameName.Contains(sTerm)) &&
                                (genreId == 0 || g.GenreId == genreId))
                    .ToListAsync()
            };

            return View(model);
        }
        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.Include(g => g.Genre).FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "GenreName");
            return View();
        }

        // POST: Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var game = new Game
                {
                    GameName = model.GameName,
                    Publisher = model.Publisher,
                    Developer = model.Developer,
                    ReleaseDate = model.ReleaseDate,
                    Price = model.Price,
                    Description = model.Description,
                    GenreId = model.GenreId
                };

                if (model.ImageFile != null)
                {
                    var fileName = Path.GetFileName(model.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    game.ImageUrl = "/images/" + fileName; // Save the path to the image
                }

                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(model);
        }




        // GET: Games/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var model = new EditGameViewModel
            {
                Id = game.Id,
                GameName = game.GameName,
                Publisher = game.Publisher,
                Developer = game.Developer,
                ReleaseDate = game.ReleaseDate,
                Price = game.Price,
                Description = game.Description,
                GenreId = game.GenreId
            };

            return View(model);
        }

        // POST: Games/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditGameViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var game = new Game
                {
                    Id = model.Id,
                    GameName = model.GameName,
                    Publisher = model.Publisher,
                    Developer = model.Developer,
                    ReleaseDate = model.ReleaseDate,
                    Price = model.Price,
                    Description = model.Description,
                    GenreId = model.GenreId
                };

                if (model.ImageFile != null)
                {
                    var fileName = Path.GetFileName(model.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    game.ImageUrl = "/images/" + fileName;
                }

                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage));
            }
            return View(model);
        }

        public IActionResult Delete()
        {
            return View();
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            var model = new DeleteGameViewModel
            {
                Id = game.Id,
                GameName = game.GameName,
                Developer = game.Developer,
                Price = game.Price,
                GenreName = game.Genre.GenreName,
                ImageUrl = game.ImageUrl
            };

            return View(model);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return Json(new { success = false });
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
        // GET: Admin/Genre
        public IActionResult Genre()
        {
            return View();
        }

        // POST: Admin/Genre
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Genre(CreateGenreViewModel model)
        {
            if (ModelState.IsValid)
            {
                var genre = new Genre
                {
                    GenreName = model.GenreName
                };

                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(model);
        }

    }
}