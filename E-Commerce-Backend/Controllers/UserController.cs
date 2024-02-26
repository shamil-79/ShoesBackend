using AutoMapper;
using E_Commerce_Backend.Models.DTOs.userDTOS;
using E_Commerce_Backend.Models.ENTITYS;
using E_Commerce_Backend.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersService _usersService;
        public UserController(IConfiguration configuration, IUsersService usersService)
        {
            _configuration = configuration;
            _usersService = usersService;
        }
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(UserRegisterDTO UserRegisterDTO)
        {
            try 
            {
                var ifexist = await _usersService.RegistrationUser(UserRegisterDTO);
                if (!ifexist)
                {
                    return BadRequest("user already exist");
                }
                return Ok(ifexist);
            }catch (Exception ex)
            {
                return StatusCode(500, $"an error occured, {ex.Message}");
            }
            
        }
        [HttpGet("allusers")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Getuser()
        {
            try
            {
                var data = await _usersService.GetUser();
                if (data == null)
                {
                    return BadRequest("theres no data");
                }
                return Ok(data);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }
        [HttpGet("userby{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetuserbyId(int id)
        {
            try
            {
                var byId = await _usersService.GetUserById(id);
                if (byId == null)
                {
                    return NotFound("invalid user");
                }
                return Ok(byId);

            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }
        [HttpPost("LogingUser")]
        public async Task<IActionResult> Logging([FromBody] UserLoginDTO UserLoginDTO)
        {
            try
            {

                var user = await _usersService.Loging(UserLoginDTO);
                if (user == null)
                {
                    return NotFound("Invalid Email or Password");
                }
                bool PWvalidate = BCrypt.Net.BCrypt.Verify(UserLoginDTO.Password, user.Password);
                if (!PWvalidate)
                {
                    return BadRequest("Wrong Password");

                }
                if(user.isBlocked)
                {
                    return BadRequest("Access denied");
                }
                string token = GenerateToken(user);
                return Ok(new { Token = token, Name = user.Name, });
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost("Adminlogin")]
        
        public async Task<IActionResult> AdminLogging([FromBody] UserLoginDTO UserLoginDTO)
        {
            try
            {

                var admin = await _usersService.Loging(UserLoginDTO);
                if (admin == null)
                {
                    return NotFound("Invalid Email or Password");
                }
                bool PWvalidate = BCrypt.Net.BCrypt.Verify(UserLoginDTO.Password, admin.Password);
                if (!PWvalidate)
                {
                    return BadRequest("Wrong Password");

                }
                if (admin.isBlocked)
                {
                    return BadRequest("Access denied");
                }
                string token = GenerateToken(admin);
                return Ok(new { Token = token, Name = admin.Name, });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("BlockUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Block(int userid)
        {
            try
            {
                if (userid <= 0)
                {
                    return NotFound();
                }
                var status = await _usersService.BlockUser(userid);
                if (!status)
                {
                    return NotFound();
                }
                return Ok(status);

            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPut("UnBlockUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> unBlock(int userid)
        {
            try
            {
                if (userid <= 0)
                {
                    return BadRequest();
                }
                var status = await _usersService.UnBlockUser(userid);
                if (!status)
                {
                    return BadRequest("user not found");
                }
                return Ok(status);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        private string GenerateToken(Users users)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, users.Id.ToString()),
            new Claim(ClaimTypes.Name, users.Name),
            new Claim(ClaimTypes.Role, users.Role),
            new Claim(ClaimTypes.Email, users.Email),
        };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(1)

            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}
