using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PotatoBot.Bot;
using DSharpPlus.Entities;
using static DSharpPlus.Entities.DiscordEmbedBuilder;
using DSharpPlus.CommandsNext;
using DAL;

namespace PotatoBot.Utils
{
    public static class DBHelper
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

        public static async Task AddLog(this GuildContext db, CommandContext ctx, GuildData guild, GuildLog log)
        {
            guild.Logs.Add(log);
            db.Entry(guild).State = EntityState.Modified;
            await db.SaveChangesAsync();

            if(guild.EnableModLogs)
            {
                var chn = ctx.Guild.Channels[guild.ModLogsChannel];

                var author = ctx.Member;

                var color = new DiscordColor(Enum.Parse<LogAction>(log.Action) switch
                {
                    LogAction.Mute => "#cad628",
                    LogAction.Ban => "#7c0b01",
                    LogAction.Kick => "#c61735",
                    LogAction.Warn => "#0d86dd",
                    _ => "#0d86dd"
                });
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{log.Action}: {log.TargetUser}",
                    Author = new EmbedAuthor
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
