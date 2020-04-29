using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}
