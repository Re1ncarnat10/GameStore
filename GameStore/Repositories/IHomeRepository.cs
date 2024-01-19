namespace GameStore
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Game>> GetGames(string sTerm = "", int genreID = 0);
        Task<IEnumerable<Genre>> Genres();
    }
}