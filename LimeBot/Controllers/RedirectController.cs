using LimeBot.DAL;
using Microsoft.AspNetCore.Mvc;

namespace LimeBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        [HttpGet("invite")]
        public IActionResult Invite([FromQuery] string id = null)
        {
            var idquery = id != null ? $"&guild_id={id}" : null;
            return Redirect($"https://discordapp.com/oauth2/authorize?client_id={Config.settings.ClientId}&permissions={Config.settings.RequiredPermissions}&scope=bot{idquery}");
        }
    }
}