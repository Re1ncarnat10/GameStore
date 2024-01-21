using GameStore.Models;
using GameStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;

namespace GameStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sterm="", int genreId=0)
        {
            IEnumerable<Game> games= await _homeRepository.GetGames(sterm,genreId);
            IEnumerable<Genre> genres = await _homeRepository.Genres();
            GameDisplayModel gameModel = new GameDisplayModel
            {
                Games = games,
                Genres = genres,
                STerm=sterm,
                GenreId=genreId
            };
            
        return View(gameModel);
        }

        [Route("Home/About_us")]
        public IActionResult AboutUs()
        {
            return View("About_us");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("Home/GameDetails/{id}")]
        public async Task<IActionResult> GameDetails(int id)
        {
            var game = await _homeRepository.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

    }
}
