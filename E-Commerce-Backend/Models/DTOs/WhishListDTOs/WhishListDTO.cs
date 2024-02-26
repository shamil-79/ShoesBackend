using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.DTOs.WhishListDTOs
{
    public class WhishListDTO
    {
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
