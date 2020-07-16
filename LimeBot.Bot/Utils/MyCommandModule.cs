using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using LimeBot.DAL;
using LimeBot.DAL.Models;

namespace LimeBot.Bot.Utils
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
