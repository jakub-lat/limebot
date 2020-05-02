using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
    public class TestCommands : BaseCommandModule
    {
        [Command("test")]
        public async Task Test(CommandContext ctx, TimeSpan? time = null, string reason = null)
        {
            await ctx.RespondAsync($"Reason: {reason}, time: {time}");
        }
    }
}
