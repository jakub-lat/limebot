using Bot;
using Bot.Attributes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using PotatoBot.Bot.Utils;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotatoBot.Bot.Commands
{
    [Category("Music")]
    public class MusicCommands : MyCommandModule
    {
        
        public MusicCommands(GuildContext db) : base(db) { }

        /*
        [Command("join"), RequireBotPermissions(Permissions.UseVoice)]
        public async Task GuildInfo(CommandContext ctx)
        {
            await ctx.RespondAsync("Connecting");
            var chn = ctx.Member?.VoiceState?.Channel;
            if (chn == null)
                throw new InvalidOperationException("You need to be in a voice channel.");

            var lava = Lavalink.node;
            Console.WriteLine(lava == null);
            await lava.ConnectAsync(chn);
            await ctx.RespondAsync("👌");
        }
        */
    }
}
