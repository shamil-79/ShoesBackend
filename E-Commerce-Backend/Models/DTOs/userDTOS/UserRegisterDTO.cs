using System.ComponentModel.DataAnnotations;



namespace E_Commerce_Backend.Models.DTOs.userDTOS
{
    public class UserRegisterDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }



    }
}
