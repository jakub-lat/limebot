using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PotatoBot.Bot.Utils;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using PotatoBot.Utils;

namespace Bot
{
    public class BotEvents
    {
        private DiscordClient client;
        private string connectionString;
        private Timer statusTimer;
        private int statusIndex = 0;
        private readonly DiscordActivity[] statuses = new DiscordActivity[] {
            new DiscordActivity("You!", ActivityType.Watching),
            new DiscordActivity("Lime sounds", ActivityType.ListeningTo),
            new DiscordActivity("I hate Lemon", ActivityType.Playing),
            new DiscordActivity("{guilds} guilds", ActivityType.Watching)
        };

        public BotEvents(DiscordClient client, string conn)
        {
            this.client = client;
            connectionString = conn;

            statusTimer = new Timer();
            statusTimer.Elapsed += new ElapsedEventHandler(ChangeStatus);
            statusTimer.Interval = 20000;
            statusTimer.Enabled = true;

            client.UpdateStatusAsync(statuses[0], UserStatus.Idle);
        }

        private void ChangeStatus(object source, ElapsedEventArgs e)
        {
            var status = statuses[statusIndex];
            status.Name = status.Name.Replace("{guilds}", client.Guilds.Count.ToString());
            client.UpdateStatusAsync(status);
            if(statusIndex >= statuses.Length-1)
            {
                statusIndex = 0;
            } else
            {
                statusIndex++;
            }
        }

        public async Task MessageCreated(MessageCreateEventArgs e)
        {
        }

        public async Task MemberJoined(GuildMemberAddEventArgs e)
        {
            using(var ctx = new GuildContext(connectionString))
            {
                var guild = await ctx.GetGuild(e.Guild.Id);

                if(guild.EnableWelcomeMessages)
                {
                    var channel = e.Guild.Channels[guild.WelcomeMessagesChannel];
                    if(channel != null)
                    {
                        await channel.SendMessageAsync(
                            guild.WelcomeMessage
                                .Replace("{user}", e.Member.Mention)
                                .Replace("{members}", e.Guild.MemberCount.ToString())
                        );
                    }
                }

                var roles = guild?.AutoRolesOnJoin;
                if(roles != null && roles.Length > 0)
                {
                    foreach (var i in roles)
                    {
                        var role = e.Guild.Roles[i];
                        if(role != null) await e.Member.GrantRoleAsync(role);
                    }
                }
            }
        }

        public async Task MemberLeft(GuildMemberRemoveEventArgs e)
        {
            using (var ctx = new GuildContext(connectionString))
            {
                var guild = await ctx.GetGuild(e.Guild.Id);

                if (guild.EnableWelcomeMessages)
                {
                    var channel = e.Guild.Channels[guild.WelcomeMessagesChannel];
                    await channel.SendMessageAsync(
                        guild.LeaveMessage
                            .Replace("{user}", e.Member.Username + "#" + e.Member.Discriminator)
                            .Replace("{members}", e.Guild.MemberCount.ToString())
                    );
                }
            }
        }

        public async Task MessageEdited(MessageUpdateEventArgs e)
        {
            if (e.Author.IsBot) return;
            using (var ctx = new GuildContext(connectionString))
            {
                var guild = await ctx.GetGuild(e.Guild.Id);

                if (guild.EnableMessageLogs)
                {
                    var chn = e.Guild.Channels[guild.MessageLogsChannel];

                    var author = e.Author;
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = $"Message edited",
                        Author = new DiscordEmbedBuilder.EmbedAuthor
                        {
                            Name = author.Username + "#" + author.Discriminator,
                            IconUrl = author.GetAvatarUrl(ImageFormat.Png, 64)
                        },
                        Description = $"**Before:**\n {e.MessageBefore.Content}\n**After:**\n {e.Message.Content}",
                        Timestamp = DateTime.Now
                    };
                    await chn.SendMessageAsync(embed: embed);
                }
            }
        }

        public async Task MessageDeleted(MessageDeleteEventArgs e)
        {
            if (e.Message.Author.IsBot) return;
            using (var ctx = new GuildContext(connectionString))
            {
                var guild = await ctx.GetGuild(e.Guild.Id);

                if (guild.EnableMessageLogs)
                {
                    var chn = e.Guild.Channels[guild.MessageLogsChannel];

                    var author = e.Message.Author;
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = $"Message deleted",
                        Author = new DiscordEmbedBuilder.EmbedAuthor
                        {
                            Name = author.Username + "#" + author.Discriminator,
                            IconUrl = author.GetAvatarUrl(ImageFormat.Png, 64)
                        },
                        Description = $"**Content:**\n {e.Message.Content}",
                        Timestamp = DateTime.Now
                    };
                    await chn.SendMessageAsync(embed: embed);

                }
            }
        }
    }
}
