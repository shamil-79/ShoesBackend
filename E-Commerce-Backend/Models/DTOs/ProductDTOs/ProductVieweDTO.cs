using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.DTOs.ProductDTOs
{
    public class ProductVieweDTO
    {
        public int Id { get; set; }
        
        public string ProductName { get; set; }
        
        public string ProductDescription { get; set; }
        
        public decimal Price { get; set; }
       
        public string ProductImage { get; set; }
        public string ProductType { get; set; }
        public string Category { get; set; } 
        

        
    }
}
