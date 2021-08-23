using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Domain.ViewModels.Authentication;
using DevryDeveloperClub.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevryDeveloperClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ClubMember> _userManager;
        private readonly IJwtService _jwt; 
        
        public AuthController(UserManager<ClubMember> userManager,  IJwtService jwt)
        {
            _userManager = userManager;
            _jwt = jwt;
        }
        

        [HttpPost]
        [Route("login")]
        // api/auth
        public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid username or password");

            var token = await _jwt.GenerateUserToken(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                id = user.Id
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
                return BadRequest("User already exists");

            ClubMember user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest("Error registering user");

            return Ok("User created successfully");
        }
    }
}