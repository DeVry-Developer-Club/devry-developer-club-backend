using System.Linq;
using System.Threading.Tasks;
using DevryDeveloperClub.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DevryDeveloperClub.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet("~/signin")]
        public async Task<string[]> SignIn() =>
            (await HttpContext.GetExternalProvidersAsync())
            .Select(x=>x.DisplayName)
            .ToArray();   
        

        [HttpPost("~/signin")]
        public async Task<IActionResult> SignIn(string provider)
        {
            // external provider chosen by user
            if (string.IsNullOrWhiteSpace(provider))
                return BadRequest("Invalid provider");

            if (!await HttpContext.IsProviderSupportedAsync(provider))
                return BadRequest($"{provider} is not supported");

            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, provider);
        }

        [HttpGet("~/signout")]
        [HttpPost("~/signout")]
        public IActionResult SignOutCurrentUser()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}