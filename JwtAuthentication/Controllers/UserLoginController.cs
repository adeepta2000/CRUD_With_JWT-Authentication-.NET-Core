using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private IConfiguration _config;
        public UserLoginController(IConfiguration config)
        {

            _config = config;

        }

        private Users AuthenticateUser(Users u)
        {
            Users _u = null!;

            if (u.Email == "user@gmail.com" && u.Password == "00000")
            {
                _u = new Users { Email = "USER@GMAIL.COM" };
            }

            return u;
        }

        private string GenerateToken(Users u)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var crendentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(2), signingCredentials: crendentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users u)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(u);
            if (user.Email == "user@gmail.com" && user.Password == "00000")
            {
                var token = GenerateToken(user);
                response = Ok(new { token = token });
            }

            return response;
        }
    }
}
