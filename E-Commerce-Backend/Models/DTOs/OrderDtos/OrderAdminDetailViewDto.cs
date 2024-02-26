using E_Commerce_Backend.Models.DTOs.CartDTOs;

namespace E_Commerce_Backend.Models.DTOs.OrderDtos
{
    public class OrderAdminDetailViewDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerCity { get; set; }
        public string HomeAddress { get; set; }
        public string OrderString { get; set; }
        public string OrderStatus { get; set; }
        public string TransactionId { get; set; }

        public DateTime OrderDate { get; set; }

        public List<CartViewDto> orderProducts { get; set; }
    }
}
