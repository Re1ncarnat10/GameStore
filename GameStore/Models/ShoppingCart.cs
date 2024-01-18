using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;
namespace GameStore.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ICollection<CartDeatil> CartDeatils { get; set; }

    }
}
