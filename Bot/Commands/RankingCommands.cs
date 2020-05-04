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
            var members = (await db.Entry(guild).Collection(i => i.Members).Query().OrderBy(x => x.XP).Take(100).ToListAsync()).Select((x, i) => new { index = i, member = x });
            var raw = members.Where(x => x.member.UserId == ctx.Member.Id).FirstOrDefault();
            var position = raw == null ? "100+" : (raw.index + 1).ToString();
            var member = raw == null ? await db.Entry(guild).Collection(i => i.Members).Query().Where(x => x.UserId == ctx.Member.Id).FirstOrDefaultAsync() : raw.member;

            var embed = new DiscordEmbedBuilder
            {
                Title = ctx.Member.Username,
                Description = $@"XP: {member?.XP}
Level: {member?.XP / 100}
Rank: #{position}",
                Color = new DiscordColor(Config.settings.embedColor)
            };
            await ctx.RespondAsync(embed: embed);

            /*var member = await db.Members.FromSqlRaw("WITH ranking AS (SELECT *, row_number() over(ORDER BY \"XP\" DESC) AS \"Position\" FROM \"Members\") SELECT * FROM ranking WHERE \"UserId\"={0} AND \"GuildId\"={1}", ctx.Member.Id, ctx.Guild.Id).ToListAsync();

            Console.WriteLine(member[0].Position);*/
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
