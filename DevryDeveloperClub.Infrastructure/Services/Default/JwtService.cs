using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevryDeveloperClub.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DevryDeveloperClub.Infrastructure.Services.Default
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<ClubMember> _userManager;
        private readonly IConfiguration _configuration;
        
        public JwtService(UserManager<ClubMember> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> GenerateUserToken(ClubMember user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            /*
                This is important
                - In order for us to restrict certain parts of our application
                    we need to add whatever roles they have from the database
             */
            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}