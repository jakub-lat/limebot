using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using PotatoBot;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PotatoBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        [HttpGet("invite")]
        public IActionResult Invite([FromQuery] string id = null)
        {
            var idquery = id !=null ? $"&guild_id={id}" : null;
            return Redirect($"https://discordapp.com/oauth2/authorize?client_id={Config.settings.ClientID}&permissions=8&scope=bot{idquery}");
        }
    }
}