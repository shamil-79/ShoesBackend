using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.ENTITYS
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public int Quantity { get; set; }



        public virtual OrderMain Order { get; set; }
        public virtual Products Product { get; set; }
    }
}
