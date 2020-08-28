using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using LimeBot.DAL.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;
using LimeBot.Bot.Utils;
using LimeBot.DAL;

namespace LimeBot.Bot
{
    public class BotEvents
    {
        private DiscordClient client;

        private Timer statusTimer;
        private int statusIndex;
        private readonly DiscordActivity[] statuses = new DiscordActivity[] {
            new DiscordActivity($"{Config.settings.DefaultPrefix}help", ActivityType.ListeningTo),
            new DiscordActivity("You!", ActivityType.Watching),
            new DiscordActivity("Lime sounds", ActivityType.ListeningTo),
            new DiscordActivity("I hate Lemon", ActivityType.Playing),
            new DiscordActivity("{guilds} guilds", ActivityType.Watching)
        };

        private Timer unmuteTimer;

        private Random rnd = new Random();

        public BotEvents(DiscordClient client)
        {
            this.client = client;

            statusTimer = new Timer();
            statusTimer.Elapsed += ChangeStatus;
            statusTimer.Interval = 20000;
            statusTimer.Enabled = true;

            client.UpdateStatusAsync(statuses[0], UserStatus.Idle);

            unmuteTimer = new Timer();
            unmuteTimer.Elapsed += UnmuteTask;
            unmuteTimer.Interval = 60000;
            unmuteTimer.Enabled = true;
            UnmuteTask();
        }
        
        private void ChangeStatus(object source, ElapsedEventArgs e)
        {
            var status = statuses[statusIndex];
            status.Name = status.Name.Replace("{guilds}", client.Guilds.Count.ToString());
            client.UpdateStatusAsync(status, UserStatus.Idle);
            if (statusIndex >= statuses.Length - 1)
            {
                statusIndex = 0;
            }
            else
            {
                statusIndex++;
            }
        }

        private async void UnmuteTask(object source = null, ElapsedEventArgs e = null)
        {
            using var ctx = new GuildContext();
            var expired = ctx.MutedUsers.Where(i => i.Time <= DateTime.UtcNow);
            foreach (var muted in await expired.ToListAsync())
            {
                try
                {
                    var guild = await ctx.GetGuild(muted.GuildId);
                    var dGuild = client.Guilds[muted.GuildId];

                    var role = dGuild.Roles[guild.MutedRoleId];
                    _ = dGuild.Members[muted.UserId].RevokeRoleAsync(role);

                    var member = dGuild.Members[muted.UserId];
                    await ctx.AddLog(dGuild, client.CurrentUser, guild, new GuildLog
                    {
                        Action = LogAction.Unmute,
                        AuthorId = client.CurrentUser.Id,
                        Date = DateTime.Now,
                        Reason = "Auto unmute",
                        TargetUser = member.Username + "#" + member.Discriminator
                    });
                }
                catch
                {
                    Console.WriteLine($"Error while unmuting user:");
                    Console.WriteLine(muted.ToString());
                }
            }
            ctx.MutedUsers.RemoveRange(expired);
            await ctx.SaveChangesAsync();
        }

        public async Task CommandErrored(CommandErrorEventArgs e)
        {
            if (e.Exception is CommandCanceledException ex)
            {
                //await e.Context.RespondAsync($":warning: {ex.Message}");
            } else if (e.Exception is ChecksFailedException ce) 
            {
                var perm = "";
                foreach (var check in ce.FailedChecks)
                {
                    perm = check switch
                    {
                        RequirePermissionsAttribute c 
                            => ce.Context.Channel.PermissionsFor(ce.Context.Member).HasPermission(c.Permissions) 
                                ? c.Permissions.ToPermissionString() : "",
                        RequireBotPermissionsAttribute c => c.Permissions.ToPermissionString(),
                        _ => perm
                    };
                }
                if (!string.IsNullOrEmpty(perm))
                    await e.Context.Channel.SendMessageAsync($"I don't have following permissions: `{perm}`.");
            } else if (e.Exception is ArgumentException)
            {
                await CommandHelp.SendCommandHelp(e.Context, e.Command, true);
            }
            else
            {
                Console.WriteLine(e.Exception);
            }
        }
        
        public async Task MessageCreated(MessageCreateEventArgs e)
        {
            if (e.Author?.IsBot ?? e.Guild == null || e.Author == null) return;
            using var ctx = new GuildContext();

            var guild = await ctx.GetGuild(e.Guild.Id);
            if (guild.EnableLeveling)
            {
                var member = await ctx.Entry(guild).Collection(i => i.Members).Query().Where(i => i.UserId == e.Author.Id).FirstOrDefaultAsync();
                if (member == null)
                {
                    member = new GuildMember
                    {
                        UserId = e.Author.Id,
                        XP = guild.MinMessageXP,
                        LastMessaged = DateTime.Now
                    };
                    guild.Members.Add(member);
                }
                else if (DateTime.Now >= member.LastMessaged.AddMinutes(1))
                {
                    if (DateTime.Now <= member.LastMessaged.AddMinutes(2))
                    {
                        member.XP += rnd.Next(guild.MinMessageXP, guild.MaxMessageXP);
                        member.LastMessaged = DateTime.Now;
                        var lvl = member.XP / guild.RequiredXPToLevelUp;

                        if (guild.EnableLevelUpMessage)
                        {
                            var chn = e.Guild.GetChannel(guild.LevelUpMessageChannel ?? e.Channel.Id) ?? e.Channel;
                            if (lvl > (member.XP - 10) / guild.RequiredXPToLevelUp)
                            {
                                await chn.SendMessageAsync(guild.LevelUpMessage.Replace("{user}", e.Author.Mention).Replace("{level}", lvl.ToString()));
                            }
                        }
                        var roleData = guild.RolesForLevel.FirstOrDefault(x => x.Level == lvl);
                        if(roleData != null)
                        {
                            var role = e.Guild.GetRole(roleData.Role);
                            if(role != null) await ((DiscordMember)e.Author).GrantRoleAsync(role, "Role for level");
                        }
                    }
                    else
                    {
                        member.LastMessaged = DateTime.Now;
                    }
                }

                await ctx.SaveChangesAsync();
            }
            
            if (guild.EnableReputation && (e.Message.Content.StartsWith("thanks") || e.Message.Content.StartsWith("thx")))
            {
                var mentions = e.MentionedUsers.Where(x => x.Id != e.Author.Id && x.IsBot == false).ToArray();
                if (!mentions.Any()) return;
                foreach (var user in mentions)
                {
                    var userDb = await ctx.Entry(guild).Collection(i => i.Members).Query().Where(i => i.UserId == user.Id).FirstOrDefaultAsync();
                    if (userDb == null)
                    {
                        userDb = new GuildMember
                        {
                            UserId = user.Id,
                            XP = guild.ReputationXP
                        };
                        guild.Members.Add(userDb);
                    }
                    else
                    {
                        userDb.XP += guild.ReputationXP;
                    }
                    await ctx.SaveChangesAsync();
                }
                await e.Channel.SendMessageAsync($"You gave karma (+{guild.ReputationXP} XP) to {string.Join(", ", mentions.Select(x => x.Mention))}.");
            } 
        }
        public async Task MemberJoined(GuildMemberAddEventArgs e)
        {
            using var ctx = new GuildContext();
            var guild = await ctx.GetGuild(e.Guild.Id);

            if (guild.EnableWelcomeMessages)
            {
                var channel = e.Guild.Channels[guild.WelcomeMessagesChannel];
                if (channel != null)
                {
                    await channel.SendMessageAsync(
                        guild.WelcomeMessage
                            .Replace("{user}", e.Member.Mention)
                            .Replace("{members}", e.Guild.MemberCount.ToString())
                    );
                }
            }

            var roles = guild?.AutoRolesOnJoin;
            if (roles != null && roles.Length > 0)
            {
                foreach (var i in roles)
                {
                    var role = e.Guild.Roles[i];
                    if (role != null) await e.Member.GrantRoleAsync(role);
                }
            }
        }

        public async Task MemberLeft(GuildMemberRemoveEventArgs e)
        {
            using var ctx = new GuildContext();
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

        public async Task MessageEdited(MessageUpdateEventArgs e)
        {
            if (e.Author?.IsBot ?? false) return;
            using var ctx = new GuildContext();
            var guild = await ctx.GetGuild(e.Guild.Id);

            if (guild.EnableMessageLogs && e.MessageBefore.Content != e.Message.Content)
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
                    Description = $@"{e.MessageBefore.Content}
**To:** {e.Message.Content}
**In channel** {e.Channel.Mention} ([Jump]({e.Message.JumpLink}))",
                    Timestamp = DateTime.Now,
                    Color = new DiscordColor("#cad628"),
                    Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"User ID: {e.Author.Id}" }
                };
                await chn.SendMessageAsync(embed: embed);
            }
        }

        public async Task MessageDeleted(MessageDeleteEventArgs e)
        {
            if (e.Message.Author?.IsBot == true) return;
            using var ctx = new GuildContext();
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
                        IconUrl = author?.GetAvatarUrl(ImageFormat.Png, 64)
                    },
                    Description = @$"{e.Message.Content}
**In channel** {e.Channel.Mention}",
                    Timestamp = DateTime.Now,
                    Color = new DiscordColor("#9b2212"),
                    Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"User ID: {e.Message.Author.Id}" }
                };
                if (e.Message.Attachments.Any())
                {
                    embed.ThumbnailUrl = e.Message.Attachments.First().ProxyUrl;
                }
                await chn.SendMessageAsync(embed: embed);
            }
        }

        public async Task MessageReactionAdd(MessageReactionAddEventArgs e)
        {
            if (e.Guild == null) return;
            using var ctx = new GuildContext();
            var rr = await ctx.ReactionRoles.Where(i => i.GuildId == e.Guild.Id && i.MessageId == e.Message.Id && i.Emoji == e.Emoji.ToString()).FirstOrDefaultAsync();

            if (rr == null) return;

            var member = e.User as DiscordMember;
            var role = e.Guild.Roles[rr.RoleId];
            await member.GrantRoleAsync(role);

            var guild = await ctx.GetGuild(e.Guild.Id);
            if (guild.ReactionRolesNotifyDM)
                await member.SendMessageAsync($"Added role **@{role.Name}** on **{e.Guild.Name}**");
        }

        public async Task MessageReactionRemove(MessageReactionRemoveEventArgs e)
        {
            if (e.User.IsBot) return;
            using var ctx = new GuildContext();
            var rr = await ctx.ReactionRoles.Where(i => i.GuildId == e.Guild.Id && i.MessageId == e.Message.Id && i.Emoji == e.Emoji.ToString()).FirstOrDefaultAsync();

            if (rr == null) return;

            var member = e.User as DiscordMember;
            var role = e.Guild.Roles[rr.RoleId];
            await member.RevokeRoleAsync(role);

            var guild = await ctx.GetGuild(e.Guild.Id);
            if (guild.ReactionRolesNotifyDM)
                await member.SendMessageAsync($"Removed role **@{role.Name}** on **{e.Guild.Name}**");
        }
    }
}