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
        public int GameId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; }
        public Game Game { get; set; }
    }
}
