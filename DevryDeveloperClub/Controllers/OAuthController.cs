using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using AspNet.Security.OAuth.GitHub;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Extensions;
using DevryDeveloperClub.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevryDeveloperClub.Controllers
{
    
    [Route("api/oauth")]
    public class OAuthController : Controller
    {
        private readonly ILogger<OAuthController> _logger;
        private readonly UserManager<ClubMember> _userManager;
        private readonly IJwtService _jwt;

        public OAuthController(ILogger<OAuthController> logger, UserManager<ClubMember> userManager, IJwtService jwt)
        {
            _logger = logger;
            _userManager = userManager;
            _jwt = jwt;
        }

        /// <summary>
        /// Retrieve a list of supported OAuth providers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("providers")]
        public Task<string[]> Providers() => Task.FromResult(
            new[]
            {
                GitHubAuthenticationDefaults.AuthenticationScheme,
                DiscordAuthenticationDefaults.AuthenticationScheme
            });

        /// <summary>
        /// Generate a populated instance of <see cref="AuthenticationProperties"/>
        /// </summary>
        /// <param name="provider">External provider to use</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        /// <exception cref="Exception">When provider is null or empty</exception>
        AuthenticationProperties CreateChallengeProps(string provider, string returnUrl = null)
        {
            AuthenticationProperties props = new()
            {
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", provider }
                }
            };
            
            switch (provider)
            {
                case GitHubAuthenticationDefaults.AuthenticationScheme:
                    props.RedirectUri = Url.Action(nameof(GithubCallback));
                    props.Parameters.Add(new KeyValuePair<string, object?>("scope", "read:org read:user"));
                    break;
                
                case DiscordAuthenticationDefaults.AuthenticationScheme:
                    props.RedirectUri = Url.Action(nameof(DiscordCallback));
                    break;
                
                default:
                    throw new Exception($"{provider} -- invalid scheme");
            }

            return props;
        }
        
        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public IActionResult SignIn(string provider, string returnUrl = null)
            => new ChallengeResult(provider, CreateChallengeProps(provider,returnUrl));

        
        [HttpGet, HttpPost]
        private async Task<IActionResult> ProcessCallbackData(HttpContext context, string provider)
        {
            var result = await context.AuthenticateAsync(provider);

            if (result?.Succeeded != true)
                return BadRequest("External Authentication Error");

            string id = result.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (id == null)
            {
                _logger.LogError($"Invalid Unique Id for provider: {provider}\n\t" +
                                 $"{string.Join("\n\t", result.Principal.Claims.Select(x=>x.Type + " --> " + x.Value))}");
                throw new Exception("Invalid unique Id");
            }

            // Does the user already exist?
            ClubMember user = await _userManager.FindByIdAsync(id);

            /*
              If the user doesn't exist we need to provision a new user
              with the information provided by our external provider               
             */

            if (user == null)
                user = await ProvisionUser(result.Principal.Claims);

            var token = await _jwt.GenerateUserToken(user);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpGet, HttpPost]
        [Route("discord-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> DiscordCallback()
            => await ProcessCallbackData(HttpContext, DiscordAuthenticationDefaults.AuthenticationScheme);
        
        [HttpGet, HttpPost]
        [Route("github-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> GithubCallback()
            => await ProcessCallbackData(HttpContext, GitHubAuthenticationDefaults.AuthenticationScheme);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        private async Task<ClubMember> ProvisionUser(IEnumerable<Claim> claims, string provider = GitHubAuthenticationDefaults.AuthenticationScheme)
        {
            ClubMember user = new();
            
            if (claims == null)
                throw new ArgumentNullException(nameof(claims));

            if (claims.ClaimExists(ClaimTypes.NameIdentifier, out string id))
                user.Id = id;

            if (claims.ClaimExists(ClaimTypes.Name, out string name))
                user.UserName = name;
            
            LinkedAccount linked = new()
            {
                Id = user.Id,
                Username = user.UserName,
            };
            
            switch (provider)
            {
                case GitHubAuthenticationDefaults.AuthenticationScheme:
                    linked.Provider = GitHubAuthenticationDefaults.AuthenticationScheme;
            
                    if (claims.ClaimExists(ClaimTypes.Email, out string email))
                        user.Email = email;

                    if (claims.ClaimExists("urn:github:url", out string profile))
                        user.GithubProfile = profile;

                    if (claims.ClaimExists("urn:github:name", out string displayName))
                        user.DisplayName = displayName;
                    
                    break;
                
                case DiscordAuthenticationDefaults.AuthenticationScheme:
                    linked.Provider = DiscordAuthenticationDefaults.AuthenticationScheme;
                    break;
            }
            
            user.LinkedAccounts.Add(linked);
            
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
                return user;

            throw new Exception(string.Join("\n", result.Errors.Select(x => x.Description)));
        }
    }
}