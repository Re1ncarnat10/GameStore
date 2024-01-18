using System.ComponentModel.DataAnnotations;
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
        public double Price { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
