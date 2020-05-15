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
            if (!guild.EnableLeveling) throw new CommandCanceledException("Leveling is disabled in this server!");
        }

        const float scale = 1.5f;

        [Command("rank"), Aliases("level"), Description("Shows XP, level and ranking position for you or specified member")]
        public async Task Rank(CommandContext ctx, DiscordMember member = null)
        {
            member ??= ctx.Member;

            var members = (await db.Entry(guild).Collection(i => i.Members).Query().OrderByDescending(x => x.XP).Take(100).ToListAsync()).Select((x, i) => new { index = i, member = x });
            var raw = members.Where(x => x.member.UserId == member.Id).FirstOrDefault();
            var position = raw == null ? "100+" : (raw.index + 1).ToString();
            var m = raw == null ? await db.Entry(guild).Collection(i => i.Members).Query().Where(x => x.UserId == member.Id).FirstOrDefaultAsync() : raw.member;

            using var bmp = new Bitmap((int)(500*scale), (int)(150*scale));
            using var g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.PixelOffsetMode = PixelOffsetMode.Half;

            var darkGray = ColorTranslator.FromHtml("#212121");
            var mainFont = new Font("Segoe UI", 21 * scale, FontStyle.Bold);
            var secondary = new Font("Segoe UI", 17 * scale, FontStyle.Regular);
            var secondary2 = new Font("Segoe UI", 14 * scale, FontStyle.Bold);

            // background
            g.DrawRoundedRectangle(new Rectangle(0, 0, bmp.Width, bmp.Height), 40, darkGray);

            Bitmap avatar;
            using (var client = new HttpClient())
            {
                var stream = await client.GetStreamAsync(member.GetAvatarUrl(DSharpPlus.ImageFormat.Jpeg, 128));
                avatar = new Bitmap(stream);
                avatar = new Bitmap(avatar, new Size((int)(128 * scale), (int)(128 * scale)));
            }

            // avatar
            g.DrawImage(GraphicsHelper.OvalImage(avatar), new PointF(11 * scale, 11 * scale));

            // username
            var stringStart = new PointF(145 * scale, 12 * scale);
            g.DrawString(member.Username.Truncate(12), mainFont, Brushes.White, stringStart);
            var size = g.MeasureString(member.Username.Truncate(12), mainFont, stringStart, StringFormat.GenericDefault);
            // discriminator
            g.DrawString("#" + member.Discriminator, secondary, Brushes.Gray, new PointF(stringStart.X + size.Width - (7 * scale), stringStart.Y + (5 * scale)));

            // position
            g.DrawString("#", secondary, Brushes.Gray, new PointF(160 * scale, 70 * scale));
            g.DrawString(position.ToString(), mainFont, Brushes.White, new PointF(175 * scale, 63 * scale));

            var level = (m.XP / guild.RequiredXPToLevelUp);
            var percent = m.XP / (float)((level + 1) * guild.RequiredXPToLevelUp);

            // level
            g.DrawString("LVL", secondary, Brushes.Gray, new PointF(255 * scale, 70 * scale));
            g.DrawString(level.ToString(), mainFont, Brushes.White, new PointF(295 * scale, 63 * scale));

            // xp bar
            g.DrawRoundedRectangle(new RectangleF(145 * scale, 115 * scale, 345 * scale, 20 * scale), 25, Color.Gray);
            g.DrawRoundedRectangle(new RectangleF(145 * scale, 115 * scale, Math.Max((int)(345 * scale * percent), 20 * scale), 20 * scale), 25, Color.LimeGreen);

            g.DrawString($"{m.XP} / {(level + 1) * guild.RequiredXPToLevelUp} XP", secondary2, Brushes.Black, new PointF(154 * scale, 112 * scale));

            var path = Path.Combine(Environment.CurrentDirectory, $"tmp_{ctx.Member.Id}.png");
            bmp.Save(path);
            await ctx.RespondWithFileAsync(path);
            File.Delete(path);
        }

        [Command("leaderboard"), Aliases("top", "lb"), Description("Get a link to server's leaderboard")]
        public async Task Leaderboard(CommandContext ctx)
        {
            var url = Config.settings.DashboardURL + "/leaderboard/" + ctx.Guild.Id;
            var embed = new DiscordEmbedBuilder
            {
                Title = $"Leaderboard for {ctx.Guild.Name}",
                Color = new DiscordColor(Config.settings.embedColor),
                Url = url,
                Description = $"[Click here]({url}) to view most active users in {ctx.Guild.Name}"
            };

            await ctx.RespondAsync(embed: embed);
        }
    }
}
