using Bot.Attributes;
using Bot.Utils;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using PotatoBot;
using PotatoBot.Bot.Commands;
using PotatoBot.Bot.Utils;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Commands
{
    [Category("Ranking")]
    public class RankingCommands : MyCommandModule
    {
        public RankingCommands(GuildContext db) : base(db) { }

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            await base.BeforeExecutionAsync(ctx);
            if (!guild.EnableLeveling) throw new CommandCanceledException();
        }

        [Command("rank"), Description("Shows your XP, level and ranking position")]
        public async Task Rank(CommandContext ctx)
        {
            var members = (await db.Entry(guild).Collection(i => i.Members).Query().OrderBy(x => x).Take(250).ToListAsync()).Select((x, i) => new { index = i, member = x });
            var member = members.Where(x => x.member.UserId == ctx.Member.Id).FirstOrDefault();

            var embed = new DiscordEmbedBuilder
            {
                Title = ctx.Member.Username,
                Description = $@"XP: {member.member.XP}
Level: {member.member.XP / 100}
Rank: #{member.index + 1}",
                Color = new DiscordColor(Config.settings.embedColor)
            };
            await ctx.RespondAsync(embed: embed);
        }

        [Command("leaderboard"), Aliases("top", "lb"), Description("Shows top 20 active users")]
        public async Task Leaderboard(CommandContext ctx)
        {

            var tasks = (await db.Entry(guild).Collection(i => i.Members).Query().OrderBy(x => x).Take(20).ToListAsync())
                .Select(async x => new {
                    x.XP,
                    Level = x.XP / 100,
                    User = await ctx.Guild.GetMemberAsync(x.UserId)
                }).ToList();
            var members = await Task.WhenAll(tasks);


            var embed = new DiscordEmbedBuilder
            {
                Title = $"Top users for {ctx.Guild.Name}",
                Description = "```" + string.Join("\n",
                members.Select((x, i) => $"{i + 1}. {x.User.Username} - Level {x.XP / 100} ({x.XP} XP)").ToList())
                + "```",
                Color = new DiscordColor(Config.settings.embedColor)
            };

            await ctx.RespondAsync(embed: embed);
        }
    }
}
