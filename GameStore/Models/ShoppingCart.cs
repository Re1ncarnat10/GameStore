using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }
        public int UserId { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
