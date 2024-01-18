using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GameStore.Models
{
    [Table("Game")]
    public class Game
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
