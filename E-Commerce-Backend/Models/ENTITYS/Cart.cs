using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.ENTITYS
{
    public class Cart
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual Users users { get; set; }
        public virtual List<CartItem> cartItems { get; set; }
    }
}
