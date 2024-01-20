namespace GameStore.ViewModels
{
    public class DeleteGameViewModel
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public string Developer { get; set; }
        public decimal Price { get; set; }
        public string GenreName { get; set; }
        public string ImageUrl { get; set; }
    }
}
