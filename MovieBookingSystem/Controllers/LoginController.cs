using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieBookingSystem.AppDBContexts;
using MovieBookingSystem.Services;
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
        private readonly LoginService loginService;
        
        public LoginController(LoginService loginService) 
        {
            this.loginService = loginService;
        }

        [HttpPost]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            string token = loginService.Login(loginRequest.Email, loginRequest.Password);

            if (!string.IsNullOrEmpty(token)) 
            {
                return Ok(token);
            }

            return Unauthorized();
        }

        

    }
}
