using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int GameId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public Order Order { get; set; }
        public Game Game { get; set; }
    }
}
