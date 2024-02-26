using E_Commerce_Backend.Models.ENTITYS;
using E_Commerce_Backend.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult>GetallCart()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                return Ok(await _cartService.GetallCartitem(jwtToken));
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult>Addtocart(int productid)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                await _cartService.AddtoCart(jwtToken, productid);
                return Ok("Product successfully added to cart");
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DelectFromCart(int productid)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                await _cartService.DeleteCart(jwtToken, productid);
                return Ok("successfully deleted");
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("add-Qty")]
        [Authorize]
        public async Task<IActionResult> ItemQtyPlus(int productid)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                await _cartService.Quanityplus(jwtToken, productid);
                return Ok("Quantity increased");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("MIN-Qty")]
        [Authorize]
        public async Task<IActionResult> ItemQtyMinus(int productid)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                await _cartService.Quanityminus(jwtToken, productid);
                return Ok("Quantity Decresed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
