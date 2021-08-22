using System;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.GitHub;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevryDeveloperClub.Controllers
{
    
    [Route("api/[controller]")]
    public class ExternalController : Controller
    {
        private readonly SignInManager<MongoUser> _signInManager;
        private readonly ILogger<ExternalController> _logger;
        private readonly UserManager<MongoUser> _userManager;

        public ExternalController(SignInManager<MongoUser> signInManager, ILogger<ExternalController> logger, UserManager<MongoUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
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
                    RedirectUri = Url.Action(nameof(Callback)),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", "GitHub" }
                    }
                });
        }

        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Callback(string returnUrl = null, string remoteError = null)
        {
            var result = await HttpContext.AuthenticateAsync("External");

            if (result?.Succeeded != true)
                throw new Exception("External Authentication Error");
            
            return NoContent();
        }
            
    }
}