using Bot.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PotatoBot.Bot.Utils;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotatoBot.Bot.Commands
{
    [Category("System")]
    public class SystemCommands : MyCommandModule
    {
        public SystemCommands(GuildContext db) : base(db) { }

        [Command("guild")]
        public async Task GuildInfo(CommandContext ctx)
        {
            var guild = await db.GetGuild(ctx.Guild.Id.ToString());
            await db.LoadLogs(guild);
            await ctx.RespondAsync($"ID: `{guild.Id}`, Prefix: {guild.Prefix}, Muted role: {guild.MutedRoleId}, Logs count: {guild.Logs.Count}");
        }

        [Command("prefix"), Description("Get or set the prefix")]
        public async Task GetPrefix(CommandContext ctx)
        {
            var guild = await db.GetGuild(ctx.Guild.Id.ToString());
            await ctx.RespondAsync($"The prefix is `{guild.Prefix}`");
        }

        [Command("prefix"), RequireUserPermissions(DSharpPlus.Permissions.ManageMessages)]
        public async Task SetPrefix(CommandContext ctx, string newPrefix)
        {
            var guild = await db.GetGuild(ctx.Guild.Id.ToString());
            guild.Prefix = newPrefix;
            await db.SaveChangesAsync();
            await ctx.RespondAsync($"Set the prefix to `{newPrefix}`");
        }

        [Command("help"), Description("If you are stuck")]
        public async Task Help(CommandContext ctx)
        {
            var url = "https://potatodiscordbot.azurewebsites.net";
            var embed = new DiscordEmbedBuilder {
                Title = "LimeBOT help",
                Description = $"Hey! I am **LIME**.\n If you are stuck, **[here is a list of my commands]({url}/commands)**.\nIf you want to configure me, **[login to dashboard]({url}/manage/{ctx.Guild.Id})**",
                Color = new DiscordColor("#0de25f")
            };
            await ctx.RespondAsync(null, false, embed.Build());
        }
    }
}
