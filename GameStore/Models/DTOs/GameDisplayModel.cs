namespace GameStore.Models.DTOs
{
    public class GameDisplayModel
    {
        public IEnumerable<Game> Games { get; set; } 
        public IEnumerable<Genre> Genres { get; set; }
        public string STerm { get; set; } = "";
        public int GenreId { get; set; } = 0;
    }
}
