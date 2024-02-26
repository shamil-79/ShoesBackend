using E_Commerce_Backend.Models.DTOs.OrderDtos;
using E_Commerce_Backend.Models.DTOs.RazorpayDtos;

namespace E_Commerce_Backend.Services.OrderSevices
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(int userid, OrderRequestDto orderRequests);
        public  Task<List<OrderViewDto>> GetOrderDtails(int userid);
        public Task<List<OrderViewDto>> AdminviewUser(int userid);
        public Task<decimal> GetTotalRevenue();
        public Task<int> GetTotalOrders();
        public  Task<List<OrderAdminViewDto>> GetOrderDetailAdmin();
        public Task<OrderAdminDetailViewDto> GetOrderDetailsById(int orderId);
        public Task<int> TodayOrders();
        public Task<decimal> TodaysRevenu();
        public Task<bool> UpdateOrder(int orderid,OrderUpdateDto orderUpdateDto);
        string Razerpay(long price);
        public bool Payment(RazorpayDto razorpay);
    }
}
