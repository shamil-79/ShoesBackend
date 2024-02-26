using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.DTOs.ProductDTOs
{
    public class ProductDTO
    {
      
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public decimal Price { get; set; }
       
        public string ProductType { get; set; }
        [Required]
        public int CategoryId { get; set; }

        
    }
}
