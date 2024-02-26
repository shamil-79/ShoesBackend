using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.ENTITYS
{
    public class CartItem
    {
        public int Id { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public virtual Cart cart { get; set; }
        public virtual Products product { get; set; }
    }
}
