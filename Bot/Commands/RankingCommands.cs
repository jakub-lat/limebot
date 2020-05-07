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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Commands
{
    [Category("Ranking"), RequireGuild]
    public class RankingCommands : MyCommandModule
    {
        public RankingCommands(GuildContext db) : base(db) { }

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            await base.BeforeExecutionAsync(ctx);
            if (!guild.EnableLeveling) throw new CommandCanceledException();
        }

        [Command("rank"), Aliases("level"), Description("Shows your XP, level and ranking position")]
        public async Task Rank(CommandContext ctx, DiscordMember member = null)
        {
            member ??= ctx.Member;

            var members = (await db.Entry(guild).Collection(i => i.Members).Query().OrderByDescending(x => x.XP).Take(100).ToListAsync()).Select((x, i) => new { index = i, member = x });
            var raw = members.Where(x => x.member.UserId == member.Id).FirstOrDefault();
            var position = raw == null ? "100+" : (raw.index + 1).ToString();
            var m = raw == null ? await db.Entry(guild).Collection(i => i.Members).Query().Where(x => x.UserId == member.Id).FirstOrDefaultAsync() : raw.member;

            using var bmp = new Bitmap(500, 150);
            using var g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.PixelOffsetMode = PixelOffsetMode.Half;

            var darkGray = ColorTranslator.FromHtml("#212121");
            var mainFont = new Font("Segoe UI", 21, FontStyle.Bold);
            var secondary = new Font("Segoe UI", 17, FontStyle.Regular);
            var secondary2 = new Font("Segoe UI", 14, FontStyle.Regular);

            // background
            g.DrawRoundedRectangle(new Rectangle(0, 0, bmp.Width, bmp.Height), 20, darkGray);

            Bitmap avatar;
            using (var client = new HttpClient())
            {
                var stream = await client.GetStreamAsync(member.GetAvatarUrl(DSharpPlus.ImageFormat.Jpeg, 128));
                avatar = new Bitmap(stream);
            }

            // avatar
            g.DrawImage(GraphicsHelper.OvalImage(avatar), new Point(11, 11));

            // username
            var stringStart = new PointF(145, 12);
            g.DrawString(member.Username.Truncate(12), mainFont, Brushes.White, stringStart);
            var size = g.MeasureString(member.Username.Truncate(12), mainFont, stringStart, StringFormat.GenericDefault);
            // discriminator
            g.DrawString("#" + member.Discriminator, secondary, Brushes.Gray, new PointF(stringStart.X + size.Width - 7, stringStart.Y + 5));

            // position
            g.DrawString("#", secondary, Brushes.Gray, new PointF(160, 70));
            g.DrawString(position.ToString(), mainFont, Brushes.White, new PointF(175, 63));

            var level = (m.XP / guild.RequiredXPToLevelUp);
            var percent = m.XP / (float)((level + 1) * guild.RequiredXPToLevelUp);

            // level
            g.DrawString("LVL", secondary, Brushes.Gray, new PointF(255, 70));
            g.DrawString(level.ToString(), mainFont, Brushes.White, new PointF(295, 63));

            // xp bar
            g.DrawRoundedRectangle(new Rectangle(145, 115, 345, 20), 20, Color.Gray);
            g.DrawRoundedRectangle(new Rectangle(145, 115, Math.Max((int)(345 * percent), 20), 20), 20, Color.LimeGreen);

            g.DrawString($"{m.XP} / {(level + 1) * guild.RequiredXPToLevelUp} XP", secondary2, Brushes.Black, new PointF(154, 112));

            var path = Path.Combine(Environment.CurrentDirectory, $"tmp_{ctx.Member.Id}.png");
            bmp.Save(path);
            await ctx.RespondWithFileAsync(path);
            File.Delete(path);
        }

        [Command("leaderboard"), Aliases("top", "lb"), Description("Shows top 20 active users")]
        public async Task Leaderboard(CommandContext ctx)
        {

            var tasks = (await db.Entry(guild).Collection(i => i.Members).Query().OrderByDescending(x => x.XP).Take(20).ToListAsync())
                .Select(async x => new {
                    x.XP,
                    Level = x.XP / guild.RequiredXPToLevelUp,
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
