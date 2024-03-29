﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GameStore.Models
{
    [Table("Game")]
    public class Game
    {
        
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1)]

        public string GameName { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public string Developer { get; set; }

        public string ReleaseDate { get; set; }
        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetails> CartDetails { get; set; }

        [NotMapped]
        public string GenreName {  get; set; }

    }
}
