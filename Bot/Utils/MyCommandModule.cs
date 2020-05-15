using DSharpPlus.CommandsNext;
using PotatoBot.Models;
using PotatoBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotatoBot.Bot.Utils
{
    public class MyCommandModule : BaseCommandModule
    {
        protected GuildContext db;
        protected GuildData guild;

        public MyCommandModule(GuildContext db) => this.db = db;

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            db = new GuildContext();
            if (ctx.Guild != null) guild = await db.GetGuild(ctx.Guild.Id);
            else guild = null;
            await base.BeforeExecutionAsync(ctx);
        }
    }
}
