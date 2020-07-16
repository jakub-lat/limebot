using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace TestBot
{
    public class TestCommands : BaseCommandModule
    {
        [Command("test"), RequireGuild]
        public async Task Test(CommandContext ctx)
        {
            
        }
        
    }
}
