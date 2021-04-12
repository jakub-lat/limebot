using LimeBot.DAL.Models;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LimeBot.Bot.Attributes;
using LimeBot.Bot.Utils;
using LimeBot.DAL;

namespace LimeBot.Bot.Commands
{
    [Category("Ranking"), RequireGuild]
    public class RankingCommands : MyCommandModule
    {
        public RankingCommands(GuildContext db) : base(db) { }

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            await base.BeforeExecutionAsync(ctx);
            if (!guild.EnableLeveling) {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Leveling is disabled for this server",
                    Color = new DiscordColor(Config.settings.embedColor)
                };
                if (ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.ManageGuild)) 
                    embed.Description = $"[Click here]({Config.settings.DashboardUrl}/manage/{ctx.Guild.Id}/ranking) to enable it";
                await ctx.RespondAsync(embed: embed);
                throw new CommandCanceledException(); 
            }
        }

        public static float Scale { get; } = 1.5f;

        [Command("rank"), Aliases("level"), Description("Shows XP, level and ranking position for you or specified member")]
        public async Task Rank(CommandContext ctx, DiscordMember member = null)
        {
            member ??= ctx.Member;

            var members = (await db.Entry(guild).Collection(i => i.Members).Query().OrderByDescending(x => x.XP).Take(100).ToListAsync()).Select((x, i) => new { index = i, member = x });
            var raw = members.FirstOrDefault(x => x.member.UserId == member.Id);
            var position = raw == null ? "100+" : (raw.index + 1).ToString();
            var m = raw == null ? await db.Entry(guild).Collection(i => i.Members).Query().Where(x => x.UserId == member.Id).FirstOrDefaultAsync() ?? new GuildMember() : raw.member;

            using var bmp = new Bitmap((int)(500*Scale), (int)(150*Scale));
            using var g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.PixelOffsetMode = PixelOffsetMode.Half;

            var darkGray = ColorTranslator.FromHtml("#212121");
            var mainFont = new Font("Segoe UI", 21 * Scale, FontStyle.Bold);
            var secondary = new Font("Segoe UI", 17 * Scale, FontStyle.Regular);
            var secondary2 = new Font("Segoe UI", 14 * Scale, FontStyle.Bold);

            // background
            g.DrawRoundedRectangle(new Rectangle(0, 0, bmp.Width, bmp.Height), 40, darkGray);

            Bitmap avatar;
            using (var client = new HttpClient())
            {
                var stream = await client.GetStreamAsync(member.AvatarHash == null ? member.DefaultAvatarUrl : member.GetAvatarUrl(ImageFormat.Jpeg, 128));
                avatar = new Bitmap(stream);
                avatar = new Bitmap(avatar, new Size((int)(128 * Scale), (int)(128 * Scale)));
            }

            // avatar
            g.DrawImage(GraphicsHelper.OvalImage(avatar), new PointF(11 * Scale, 11 * Scale));

            // username
            var stringStart = new PointF(145 * Scale, 12 * Scale);
            g.DrawString(member.Username.Truncate(12), mainFont, Brushes.White, stringStart);
            var size = g.MeasureString(member.Username.Truncate(12), mainFont, stringStart, StringFormat.GenericDefault);
            // discriminator
            g.DrawString("#" + member.Discriminator, secondary, Brushes.Gray, new PointF(stringStart.X + size.Width - (7 * Scale), stringStart.Y + (5 * Scale)));

            // position
            g.DrawString("#", secondary, Brushes.Gray, new PointF(160 * Scale, 70 * Scale));
            g.DrawString(position, mainFont, Brushes.White, new PointF(175 * Scale, 63 * Scale));

            var level = (m.XP / guild.RequiredXPToLevelUp);
            var percent = m.XP / (float)((level + 1) * guild.RequiredXPToLevelUp);

            // level
            g.DrawString("LVL", secondary, Brushes.Gray, new PointF(255 * Scale, 70 * Scale));
            g.DrawString(level.ToString(), mainFont, Brushes.White, new PointF(295 * Scale, 63 * Scale));

            // xp bar
            g.DrawRoundedRectangle(new RectangleF(145 * Scale, 115 * Scale, 345 * Scale, 20 * Scale), 25, Color.Gray);
            g.DrawRoundedRectangle(new RectangleF(145 * Scale, 115 * Scale, Math.Max((int)(345 * Scale * percent), 20 * Scale), 20 * Scale), 25, Color.LimeGreen);

            g.DrawString($"{m.XP} / {(level + 1) * guild.RequiredXPToLevelUp} XP", secondary2, Brushes.Black, new PointF(154 * Scale, 112 * Scale));

            var path = Path.Combine(Environment.CurrentDirectory, $"tmp_{ctx.Member.Id}.png");
            bmp.Save(path);
            await ctx.RespondAsync(new DiscordMessageBuilder().WithFile(path));
            File.Delete(path);
        }

        [Command("leaderboard"), Aliases("top", "lb"), Description("Get a link to server's leaderboard")]
        public async Task Leaderboard(CommandContext ctx)
        {
            var url = Config.settings.DashboardUrl + "/leaderboard/" + ctx.Guild.Id;
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
