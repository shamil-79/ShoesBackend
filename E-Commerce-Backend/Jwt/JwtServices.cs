using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce_Backend.Jwt
{
    public class JwtServices:IJwtServices
    {
        private readonly IConfiguration _configuration;
        private readonly string secretKey;

        public JwtServices(IConfiguration configuration)
        {
            _configuration = configuration;
            secretKey = _configuration["Jwt:Key"];
        }
        public int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Encoding.UTF8.GetBytes(secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                ValidateIssuer = false,
                ValidateAudience = false
            };


            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);


            if (validatedToken is not JwtSecurityToken jwtToken)
            {
                throw new SecurityTokenException("Invalid JWT token.");
            }


            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);


            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new SecurityTokenException("Invalid or missing user ID claim.");
            }

            return userId;
        }


    }
}
