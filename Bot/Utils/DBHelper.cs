using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PotatoBot.Bot;
using DSharpPlus.Entities;
using static DSharpPlus.Entities.DiscordEmbedBuilder;

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

        public static async Task AddLog(this GuildContext ctx, GuildData guild, GuildLog log)
        {
            guild.Logs.Add(log);
            ctx.Entry(guild).State = EntityState.Modified;
            await ctx.SaveChangesAsync();

            if(guild.EnableModLogs)
            {
                var dc = BotService.instance.discord;
                var chn = dc.Guilds[guild.Id].Channels[guild.ModLogsChannel];

                var author = dc.Guilds[guild.Id].Members[log.AuthorId];
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{log.Action}: {log.TargetUser}",
                    Author = new EmbedAuthor
                    {
                        Name = author.Username + "#" + author.Discriminator,
                        IconUrl = dc.Guilds[guild.Id].Members[log.AuthorId].GetAvatarUrl(DSharpPlus.ImageFormat.Png, 64)
                    },
                    Description = $"Reason: `{log.Reason}`",
                    Timestamp = log.Date
                };
                await chn.SendMessageAsync(embed: embed);
            }
        }
    }
}
