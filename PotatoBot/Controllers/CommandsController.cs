using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PotatoBot.Bot;

namespace PotatoBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            var commands = BotService.instance.GetCommands();
            return Ok(commands);
        }
    }
}