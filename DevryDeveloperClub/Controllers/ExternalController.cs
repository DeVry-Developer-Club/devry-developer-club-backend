using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.GitHub;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Extensions;
using DevryDeveloperClub.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DevryDeveloperClub.Controllers
{
    
    [Route("api/[controller]")]
    public class ExternalController : Controller
    {
        private readonly SignInManager<ClubMember> _signInManager;
        private readonly ILogger<ExternalController> _logger;
        private readonly UserManager<ClubMember> _userManager;
        private readonly IJwtService _jwt;
        
        public ExternalController(SignInManager<ClubMember> signInManager, ILogger<ExternalController> logger, UserManager<ClubMember> userManager, IJwtService jwt)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _jwt = jwt;
        }

        [HttpGet]
        [HttpPost]
        [Route("signin-github")]
        [AllowAnonymous]
        public IActionResult SignInGithub(string returnUrl = null)
        {
            return new ChallengeResult(
                GitHubAuthenticationDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(GithubCallback)),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", "GitHub" }
                    },
                    Parameters =
                    {
                        { "scope" , "read:org read:user" }
                    }
                });
        }

        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GithubCallback()
        {
            foreach (var item in HttpContext.Items)
                Console.WriteLine(item.Key + " " + item.Value);
            
            var result = await HttpContext.AuthenticateAsync(GitHubAuthenticationDefaults.AuthenticationScheme);

            if (result?.Succeeded != true)
                throw new Exception("External Authentication Error");

            if (!result.Principal.Claims.Any(x => x.Type == ClaimTypes.NameIdentifier))
                return BadRequest("Invalid unique ID");
            
            string githubId = result.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            // Does the user exist already?
            ClubMember user = await _userManager.FindByIdAsync(githubId);

            /*
              If the user doesn't exist we need to provision a new user
              with the information provided by our external provider               
             */
            
            if (user == null)
                user = await ProvisionUser(result.Principal.Claims);
            
            // Generate JWT token based on user information
            var token = await _jwt.GenerateUserToken(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                id = user.Id
            });
        }
        
        private async Task<ClubMember> ProvisionUser(IEnumerable<Claim> claims)
        {
            ClubMember user = new();
            
            if (claims == null)
                throw new ArgumentNullException(nameof(claims));
            
            foreach(var claim in claims)
                Console.WriteLine($"\t{claim.Type} \t|\t {claim.Value}");
            
            if (claims.ClaimExists(ClaimTypes.NameIdentifier, out string id))
                user.Id = id;

            if (claims.ClaimExists(ClaimTypes.Name, out string name))
                user.UserName = name;
            
            if (claims.ClaimExists(ClaimTypes.Email, out string email))
                user.Email = email;

            if (claims.ClaimExists("urn:github:url", out string profile))
                user.GithubProfile = profile;

            if (claims.ClaimExists("urn:github:name", out string displayName))
                user.DisplayName = displayName;
            
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
                return user;

            throw new Exception(string.Join("\n", result.Errors.Select(x => x.Description)));
        }
    }
}