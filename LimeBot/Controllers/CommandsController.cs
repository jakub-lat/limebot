using Microsoft.AspNetCore.Mvc;
using LimeBot.Bot;

namespace LimeBot.Controllers
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