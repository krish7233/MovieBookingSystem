using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieBookingSystem.AppDBContexts;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;

namespace MovieBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MovieBookingDBContext _dbContext;
        private readonly IConfiguration _configuration;
        public LoginController(MovieBookingDBContext movieBookingDBContext, IConfiguration configuration) 
        {
            _dbContext = movieBookingDBContext;
            _configuration = configuration;
        }

        [HttpPost]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (_dbContext.users.Any(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password)) {
                return Ok(GenerateJWTToken(loginRequest.Email, (loginRequest.Email == "krishsharma7233@gmail.com" ? "admin" : "user")));
            }

            return Unauthorized();
        }

        private string GenerateJWTToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration.GetSection("jwt:key").Value;
            var audience = _configuration.GetSection("jwt:audience").Value;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim( "email", email), new Claim(ClaimTypes.Role, role) }),
                Expires = DateTime.Now.AddDays(7),
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
