using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models

{
    [Table("CartDeatil")]
    public class CartDeatil
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public int GameId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        public Game Game { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
