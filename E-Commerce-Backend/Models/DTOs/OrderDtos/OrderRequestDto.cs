using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models.DTOs.OrderDtos
{
    public class OrderRequestDto
    {
        public string CustomerName { get; set; }
        [EmailAddress]
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerCity { get; set; }
        public string HomeAddress { get; set; }
        public string OrderString { get; set; }
        public string TransactionId { get; set; }
    }
}
