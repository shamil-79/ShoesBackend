using E_Commerce_Backend.Dbcontext;
using E_Commerce_Backend.Jwt;
using E_Commerce_Backend.Models.DTOs.CartDTOs;
using E_Commerce_Backend.Models.DTOs.OrderDtos;
using E_Commerce_Backend.Models.DTOs.RazorpayDtos;
using E_Commerce_Backend.Models.ENTITYS;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace E_Commerce_Backend.Services.OrderSevices
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly ShoesDbcontext _dbContext;
        private readonly string HostUrl;
        private readonly IJwtServices _jwtServices;
        public OrderService(IConfiguration configuration, ShoesDbcontext dbContext, IJwtServices jwtServices)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            HostUrl = _configuration["HostUrl:url"];
            _jwtServices = jwtServices;
        }

        

        public async Task<bool> CreateOrder(int userid, OrderRequestDto orderRequests)
        {
            try
            {

                var cart = await _dbContext.cart.Include(ci => ci.cartItems).ThenInclude(p => p.product).FirstOrDefaultAsync(c => c.UserId == userid);
                var order = new OrderMain
                {
                    userId = userid,
                    OrderDate = DateTime.Now,
                    CustomerCity = orderRequests.CustomerCity,
                    CustomerEmail = orderRequests.CustomerEmail,
                    CustomerName = orderRequests.CustomerName,
                    CustomerPhone = orderRequests.CustomerPhone,
                    HomeAddress = orderRequests.HomeAddress,
                    OrderString = orderRequests.OrderString,
                    TransactionId = orderRequests.TransactionId,
                    OrderItems = cart.cartItems.Select(Oi => new OrderItem
                    {
                        ProductId = Oi.ProductId,
                        Quantity = Oi.Quantity,
                        TotalPrice = Oi.Quantity * Oi.product.Price
                    }).ToList()
                };
                _dbContext.orders.Add(order);
                _dbContext.cart.Remove(cart);
                await _dbContext.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<OrderViewDto>> GetOrderDtails(int userid)
        {
            var orders=await _dbContext.orders.Include(oi=>oi.OrderItems).ThenInclude(p=>p.Product).Where(u=>u.userId==userid).ToListAsync();
            var orderdetails=new List<OrderViewDto>();
            foreach(var order in orders)
            {
                foreach(var Orderitem in order.OrderItems)
                {
                    var orderD = new OrderViewDto
                    {
                        Id = Orderitem.Id,
                        OrderDate = order.OrderDate,
                        ProductName = Orderitem.Product.ProductName,
                        ProductImage = HostUrl + Orderitem.Product.ProductImage,
                        Quantity = Orderitem.Quantity,
                        TotalPrice = Orderitem.TotalPrice,
                        OrderId = order.OrderString,
                        OrderStatus = order.OrderStatus,

                    };
                    orderdetails.Add(orderD);
                }
            }
            return orderdetails;
        }
        public async Task<List<OrderViewDto>> AdminviewUser(int userid)
        {
            var orders = await _dbContext.orders.Include(oi => oi.OrderItems).ThenInclude(p => p.Product).Where(u => u.userId == userid).ToListAsync();
            var orderdetails = new List<OrderViewDto>();
            foreach (var order in orders)
            {
                foreach (var Orderitem in order.OrderItems)
                {
                    var orderD = new OrderViewDto
                    {
                        Id = Orderitem.Id,
                        OrderDate = order.OrderDate,
                        ProductName = Orderitem.Product.ProductName,
                        ProductImage = HostUrl + Orderitem.Product.ProductImage,
                        Quantity = Orderitem.Quantity,
                        TotalPrice = Orderitem.TotalPrice,
                        OrderId = order.OrderString,
                        OrderStatus = order.OrderStatus,

                    };
                    orderdetails.Add(orderD);
                }
            }
            return orderdetails;
        }
        public async Task<List<OrderAdminViewDto>> GetOrderDetailAdmin()
        {
            var orders=await _dbContext.orders.Include(o=>o.OrderItems).ToListAsync();
            if (orders != null)
            {
                var orderdetail=orders.Select(d=>new OrderAdminViewDto
                {
                    Id = d.Id,
                    CustomerEmail = d.CustomerEmail,
                    CustomerName = d.CustomerName,
                    OrderDate= d.OrderDate,
                    OrderId=d.OrderString,
                    OrderStatus=d.OrderStatus,
                    TransactionId=d.TransactionId,
                    

                }).ToList();
                return orderdetail;
            }
            return new List<OrderAdminViewDto>();
        }
        public async Task<OrderAdminDetailViewDto> GetOrderDetailsById(int orderId)
        {
            var order=await _dbContext.orders.Include(i=>i.OrderItems).ThenInclude(p=>p.Product).FirstOrDefaultAsync(o=>o.Id==orderId);
            if (order != null)
            {
                var orderdetail = new OrderAdminDetailViewDto
                {
                    Id = order.Id,
                    CustomerEmail = order.CustomerEmail,
                    CustomerName = order.CustomerName,
                    OrderDate = order.OrderDate,
                    CustomerCity = order.CustomerCity,
                    OrderStatus = order.OrderStatus,
                    OrderString = order.OrderString,
                    CustomerPhone = order.CustomerPhone,
                    HomeAddress = order.HomeAddress,
                    TransactionId = order.TransactionId,
                    orderProducts = order.OrderItems.Select(oi => new CartViewDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.ProductName,
                        ProductImage = HostUrl + oi.Product.ProductImage,
                        Price = oi.Product.Price,
                        Quantity = oi.Quantity,
                        TotalAmount = oi.Product.Price

                    }).ToList()
                };
                return orderdetail;
            }
            return new OrderAdminDetailViewDto();
        }
        public async Task<decimal> GetTotalRevenue()
        {
            var order = await _dbContext.orders.Include(o => o.OrderItems).ToListAsync();

            if (order != null)
            {
                var orderdetails = order.SelectMany(o => o.OrderItems);
                var totalIncome = orderdetails.Sum(od => od.TotalPrice);
                return totalIncome;
            }

            return 0;
        }

        public async Task<int> GetTotalOrders()
        {
            var order = await _dbContext.orders.Include(o => o.OrderItems).ToListAsync();

            if (order != null)
            {
                var orderdet = order.SelectMany(o => o.OrderItems);
                var totalIncome = orderdet.Sum(od => od.Quantity);
                return totalIncome;
            }

            return 0;
        }

        public async Task<int> TodayOrders()
        {
            DateTime daystart= DateTime.Today;
            DateTime dayend=daystart.AddDays(1).AddTicks(-1);
            var order=await _dbContext.orders.Include(o=>o.OrderItems).Where(o => o.OrderDate >= daystart && o.OrderDate <= dayend).ToListAsync();
            if(order != null)
            {
                var orderd=order.SelectMany(order => order.OrderItems);
                var todayorder = orderd.Sum(o => o.Quantity);
                return todayorder;
            }
            return 0;
        }
        public async Task<decimal> TodaysRevenu()
        {
            DateTime daystart = DateTime.Today;
            DateTime dayend = daystart.AddDays(1).AddTicks(-1);
            var order = await _dbContext.orders.Include(o => o.OrderItems).Where(o => o.OrderDate >= daystart && o.OrderDate <= dayend).ToListAsync();
            if (order != null)
            {
                var orderd = order.SelectMany(order => order.OrderItems);
                decimal revenue = orderd.Sum(o => o.TotalPrice);
                return revenue;
            }
            return 0;

        }
        public async Task<bool>UpdateOrder(int orderid,OrderUpdateDto orderUpdateDto)
        {
            var order = await _dbContext.orders.FindAsync(orderid);
            if(order != null)
            {
                order.OrderStatus= orderUpdateDto.OrderStatus;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public string Razerpay(long price)
        {
            Dictionary<string, object> input = new Dictionary<string, object>();
            Random random = new Random();
            string TrasactionId = random.Next(0, 1000).ToString();
            input.Add("amount", Convert.ToDecimal(price) * 100);
            input.Add("currency", "INR");
            input.Add("receipt", TrasactionId);

            string key = _configuration["Razorpay:KeyId"];
            string secret = _configuration["Razorpay:KeySecret"];

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            var OrderId = order["id"].ToString();

            return OrderId;
        }

        public bool Payment(RazorpayDto razorpay)
        {
            if (razorpay == null ||
        razorpay.razorpay_payment_id == null ||
        razorpay.razorpay_order_id == null ||
        razorpay.razorpay_signature == null)
            {
                return false;
            }
            RazorpayClient client = new RazorpayClient(_configuration["Razorpay:KeyId"], _configuration["Razorpay:KeySecret"]);
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("razorpay_payment_id", razorpay.razorpay_payment_id);
            attributes.Add("razorpay_order_id", razorpay.razorpay_order_id);
            attributes.Add("razorpay_signature", razorpay.razorpay_signature);

            Utils.verifyPaymentSignature(attributes);


            return true;
        }
    }
}
