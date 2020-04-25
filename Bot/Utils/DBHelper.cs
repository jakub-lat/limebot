using DSharpPlus.CommandsNext;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PotatoBot.Bot.Utils
{
    public static class DBHelper
    {
        public static async Task<GuildData> GetGuild(this GuildContext ctx, ulong id)
        {
            var guild = await ctx.Guilds.FindAsync(id);
            if(guild == null)
            {
                var newGuild = new GuildData()
                {
                    Id = id
                };
                ctx.Guilds.Add(newGuild);
                await ctx.SaveChangesAsync();
                return newGuild;
            } else
            {
                return guild;
            }
        }

        public static async Task LoadLogs(this GuildContext ctx, GuildData guild)
        {
            await ctx.Entry(guild).Collection(g => g.Logs).LoadAsync();
        }
    }
}
