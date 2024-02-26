using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.ENTITYS
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ProductImage { get; set; }
        public string ProductType { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual List<CartItem> CartItems { get; set; }
    }
}
