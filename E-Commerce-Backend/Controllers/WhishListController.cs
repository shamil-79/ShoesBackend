using E_Commerce_Backend.Services.WhislistServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhishListController : ControllerBase
    {
        private readonly IWhishListServices _Wlist;
        public WhishListController(IWhishListServices wlist)
        {
            _Wlist = wlist;
        }

        [HttpGet("get-whishlist")]
        [Authorize]
        public async Task<IActionResult>GetWhislList()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                return Ok(await _Wlist.GetWhishList(jwtToken));
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost("add-whishlist")]
        [Authorize]
        public async Task<IActionResult>AddToWhishlist(int productid)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                var W_isexist = await _Wlist.AddToWhishlist(jwtToken, productid);
                if (!W_isexist)
                {
                    return BadRequest("Already In WhishList");
                }
                else
                {
                    return Ok("Added To Whislist");
                }
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpDelete("remove-from-w_list")]
        [Authorize]
        public async Task<IActionResult> Deletfromwhishlist(int productid)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                await _Wlist.RemoveFromWhishlist(jwtToken, productid);
                return Ok("Removed successfully");
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
