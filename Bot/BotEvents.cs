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
                var guild = await ctx.GetGuild(args.Guild.Id);

                if(guild.EnableWelcomeMessages)
                {
                    var channel = args.Guild.Channels[guild.WelcomeMessagesChannel];
                    if(channel != null)
                    {
                        await channel.SendMessageAsync(guild.WelcomeMessage.Replace("{user}", args.Member.Mention));
                    }
                }

                var roles = guild?.AutoRolesOnJoin;
                if(roles != null && roles.Count > 0)
                {
                    foreach (var i in roles)
                    {
                        var role = args.Guild.Roles[i];
                        if(role != null) await args.Member.GrantRoleAsync(role);
                    }
                }
            }
        }

        public async Task MemberLeft(GuildMemberRemoveEventArgs args)
        {
            using (var ctx = new GuildContext(connectionString))
            {
                var guild = await ctx.GetGuild(args.Guild.Id);

                if (guild.EnableWelcomeMessages)
                {
                    var channel = args.Guild.Channels[guild.WelcomeMessagesChannel];
                    if (channel != null)
                    {
                        await channel.SendMessageAsync(guild.LeaveMessage.Replace("{user}", args.Member.Username + "#" + args.Member.Discriminator));
                    }
                }
            }
        }
    }
}
