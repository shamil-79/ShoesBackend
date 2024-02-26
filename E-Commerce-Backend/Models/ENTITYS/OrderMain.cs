using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.ENTITYS
{
    public class OrderMain
    {
        public int Id { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string CustomerPhone { get; set; }
        [Required]
        public string CustomerCity { get; set; }
        [Required]
        public string HomeAddress { get; set; }
        [Required]
        public string OrderString { get; set; }
        [Required]
        public string OrderStatus { get; set; }
        [Required]
        public string TransactionId { get; set; }

        public Users users { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
