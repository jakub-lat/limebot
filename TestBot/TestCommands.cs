using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Drawing2D;
using System.Net.Http;

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
