﻿namespace GameStore.ViewModels
{
    public class EditGameViewModel
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int GenreId { get; set; }
    }
}