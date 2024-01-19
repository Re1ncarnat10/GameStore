using Microsoft.EntityFrameworkCore;
using System.Linq;

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

            var games = from game in _db.Games.Include(g => g.Genre)
                        where string.IsNullOrWhiteSpace(sTerm) || game.GameName.ToLower().StartsWith(sTerm)
                        select game;

            if (genreId > 0)
            {
                games = games.Where(a => a.GenreId == genreId);
            }

            return await games.ToListAsync();
        }
    }
}
