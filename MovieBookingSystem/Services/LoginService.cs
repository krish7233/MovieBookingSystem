using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieBookingSystem.AppDBContexts;
using MovieBookingSystem.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieBookingSystem.Services
{
    public class LoginService
    {
        private readonly IConfiguration _configuration;
        private readonly MovieBookingDBContext _context;

        public LoginService(IConfiguration configuration, MovieBookingDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string Login(string username, string password)
        {
            if (_context.users.Any(u => u.Email == username && u.Password == password))
            {
                return GenerateJWTToken(username, (username == "krishsharma7233@gmail.com" ? "admin" : "user"));
            }

            return null;
        }

        private string GenerateJWTToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration.GetSection("jwt:key").Value;
            var audience = _configuration.GetSection("jwt:audience").Value;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("email", email), new Claim(ClaimTypes.Role, role) }),
                Expires = DateTime.Now.AddDays(1),
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}
