using E_Commerce_Backend.Models.DTOs.OrderDtos;
using E_Commerce_Backend.Models.DTOs.RazorpayDtos;
using E_Commerce_Backend.Services.OrderSevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orders;
        public OrderController(IOrderService orders)
        {
            _orders = orders;
        }
        [HttpPost("Place-order")]
        [Authorize]
        public async Task<IActionResult>PlaceOrder(int userid, OrderRequestDto orderRequests)
        {
            try
            {
                if (orderRequests == null)
                {
                    return BadRequest("invalid");
                }
                var response = await _orders.CreateOrder(userid, orderRequests);
                return Ok(response);
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("get_order_details-user")]
        [Authorize]
        public async Task<ActionResult> GetOrderDetails(int userid)
        {
            try
            {
                var details = await _orders.GetOrderDtails(userid);
                if(details == null)
                {
                    return BadRequest("invalid or no details exixt");
                }
                return Ok(details);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("total_revenue")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetTotalRevenue()
        {
            try
            {
                return Ok(await _orders.GetTotalRevenue());
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("get-order-details-admin")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetOrderDetailsAdmin()
        {
            try
            {
                return Ok(await _orders.GetOrderDetailAdmin());
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("get-detailed-order-admin")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetDetailedOrder(int orderId)
        {
            try
            {
                return Ok(await _orders.GetOrderDetailsById(orderId));
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPut("update-order-status")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateOrder(int orderId, [FromBody] OrderUpdateDto updateOrder)
        {
            try
            {
                var staus=await _orders.UpdateOrder(orderId,updateOrder);
                return Ok("status Changed");
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("adminUserOrder")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult> SingleUserOrder(int userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest("invalid user");
                }
                return Ok(await _orders.AdminviewUser(userId));
            }catch(Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

        [HttpGet("total-orders")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult> GetTotalOrders()
        {
            try
            {
                return Ok(await _orders.GetTotalOrders());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("today-orders")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult> TodayOrders()
        {
            try
            {
                return Ok(await _orders.TodayOrders());
            }catch (Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

        [HttpGet("today-revenue")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> TodayRevenue()
        {
            try
            {
                return Ok(await _orders.TodaysRevenu());
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("order-create-razorpay")]
        [Authorize]
        public async Task<ActionResult> RazorpayOrder(long price)
        {
            try
            {
                if (price <= 0)
                {
                    return BadRequest("enter The actual Amount ");
                }
                var OrderId= _orders.Razerpay(price);
                return Ok(OrderId);
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost("payment")]
        [Authorize]
        public IActionResult Payment(RazorpayDto razorpay)
        {
            try
            {
                if(razorpay == null)
                {
                    return BadRequest("razorpay details must not null here");
                }
                var RZpay =  _orders.Payment(razorpay);
                return Ok(RZpay);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
