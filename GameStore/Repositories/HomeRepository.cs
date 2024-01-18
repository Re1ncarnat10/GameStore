
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }
        public async Task<IEnumerable<Game>> GetGames(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Game> games = await(from game in _db.Games
                         join genre in _db.Genres
                         on game.GenreId equals genre.Id
                         where string.IsNullOrWhiteSpace(sTerm) || (game!=null && game.GameName.ToLower().StartsWith(sTerm))
                         select new Game
                         {
                             Id = game.Id,
                             ImageUrl = game.ImageUrl,
                             Developer = game.Developer,
                             GameName = game.GameName,
                             GenreId = game.GenreId,
                             Price = game.Price,
                             GenreName = game.GenreName
                         }
                         ).ToListAsync();
            if(genreId>0)
            {
                games = games.Where(a => a.GenreId == genreId).ToList();
            }
            return games;
        }
    }
}
