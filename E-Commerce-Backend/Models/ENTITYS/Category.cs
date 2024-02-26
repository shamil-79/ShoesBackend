using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.ENTITYS
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual List<Products> Products { get; set; }
    }
}
