using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
