using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VideoClub.Data.DataModels;
using VideoClub.Data.Models;

namespace VideoClub.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;
        private readonly IConfiguration _config;

        public LoginController(SignInManager<Employee> signInManager, UserManager<Employee> userManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginData loginData)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginData.Email, loginData.Password, true, true);
                var user = await _userManager.FindByEmailAsync(loginData.Email);

                if (result.Succeeded && user.Active == true)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var userRole = await _userManager.GetRolesAsync(user);

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Role, userRole[0]),
                    };

                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(180),
                        signingCredentials: credentials
                        );

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), });
                }

                return NotFound("Email or password are incorrect");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
