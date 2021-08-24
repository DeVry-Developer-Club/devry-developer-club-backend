using Microsoft.AspNetCore.Mvc;

namespace DevryDeveloperClub.Controllers
{
    [ApiController]
    public class DiscordController : ControllerBase
    {
        [Route("invite"), HttpGet] public string Invite() => "https://discord.gg/ud4rkGAe9z";
    }
}