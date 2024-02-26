namespace E_Commerce_Backend.Models.DTOs.OrderDtos
{
    public class OrderAdminViewDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string TransactionId { get; set; }
    }
}
