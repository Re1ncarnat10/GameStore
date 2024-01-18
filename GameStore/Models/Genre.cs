using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string GenreName { get; set; }

        public List <Game> Games { get; set; }
    }
}
