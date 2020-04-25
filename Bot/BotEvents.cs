using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PotatoBot.Bot.Utils;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    public class BotEvents
    {
        private string connectionString;

        public BotEvents(string conn)
        {
            connectionString = conn;
        }


        public async Task MessageCreated(MessageCreateEventArgs e)
        {
            
        }

        public async Task MemberJoined(GuildMemberAddEventArgs args)
        {
            using(var ctx = new GuildContext(connectionString))
            {
                var guild = await ctx.GetGuild(args.Guild.Id.ToString());
                var roles = guild?.AutoRolesOnJoin;
                if(roles != null && roles.Count > 0)
                {
                    foreach (var i in roles)
                    {
                        var role = args.Guild.Roles.Values.FirstOrDefault(x => x.Id.ToString() == i);
                        await args.Member.GrantRoleAsync(role);
                    }
                }
                
            }
            
        }
    }
}
