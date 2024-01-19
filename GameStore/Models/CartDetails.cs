using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models

{
    [Table("CartDetail")]
    public class CartDetails
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public int GameId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public Game Game { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
