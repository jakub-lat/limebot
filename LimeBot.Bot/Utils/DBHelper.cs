using System.Threading.Tasks;
using DSharpPlus.Entities;
using LimeBot.DAL;
using LimeBot.DAL.Models;

namespace LimeBot.Bot.Utils
{
    public static class DbHelper
    {

        public static async Task<GuildData> GetGuild(this GuildContext ctx, ulong id)
        {
            var guild = await ctx.Guilds.FindAsync(id);
            if(guild == null)
            {
                var newGuild = await ctx.InsertGuild(id);
                return newGuild;
            } else
            {
                return guild;
            }
        }

        public static async Task<GuildData> InsertGuild(this GuildContext ctx, ulong id)
        {
            var newGuild = new GuildData()
            {
                Id = id
            };
            ctx.Guilds.Add(newGuild);
            await ctx.SaveChangesAsync();
            return newGuild;
        }

        public static async Task AddLog(this GuildContext db, DSharpPlus.Entities.DiscordGuild dGuild, DSharpPlus.Entities.DiscordUser author, GuildData guild, GuildLog log)
        {
            /*guild.Logs.Add(log);
            db.Entry(guild).State = EntityState.Modified;
            await db.SaveChangesAsync();*/

            if(guild.EnableModLogs)
            {
                var chn = dGuild.Channels[guild.ModLogsChannel];

                var color = new DiscordColor(log.Action switch
                {
                    LogAction.Mute => "#cad628",
                    LogAction.Ban => "#7c0b01",
                    LogAction.Kick => "#c61735",
                    LogAction.Warn => "#0d86dd",
                    _ => Config.settings.embedColor
                });
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{log.Action}: {log.TargetUser}",
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        Name = author.Username + "#" + author.Discriminator,
                        IconUrl = author.GetAvatarUrl(DSharpPlus.ImageFormat.Png, 64)
                    },
                    Description = $"Reason: `{log.Reason}`",
                    Timestamp = log.Date,
                    Color = color
                };
                await chn.SendMessageAsync(embed: embed);
            }
        }
    }
}
