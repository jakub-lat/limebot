using LimeBot.Bot.Attributes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LimeBot.Bot.Extensions;
using DSharpPlus.Interactivity;
using LimeBot.DAL.Models;
using LimeBot.Bot.Utils;
using LimeBot.DAL;

namespace LimeBot.Bot.Commands
{
    [Category("Moderation"), RequireGuild]
    public class ModerationCommands : MyCommandModule
    {
        public ModerationCommands(GuildContext db) : base(db) { }

        private async Task CheckMemberHierarchy(CommandContext ctx, DiscordMember member)
        {
            if(ctx.Guild.CurrentMember.Hierarchy <= member.Hierarchy || member.Hierarchy >= ctx.Member.Hierarchy || member.IsOwner || member.PermissionsIn(ctx.Channel).HasPermission(Permissions.Administrator))
            {
                await ctx.RespondAsync("Oh no, this user is too important!");
                throw new CommandCanceledException();
            }
        }

        [Command("ban"), Description("Ban an user"), RequirePermissions(Permissions.BanMembers)]
        public async Task Ban(CommandContext ctx, DiscordMember member, [RemainingText] string reason = "No reason")
        {
            await member.SendMessageAsync($"You were banned from **{ctx.Guild.Name}**. Reason: `{reason}`");

            await member.BanAsync(0, reason);
            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Ban,
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });

            await ctx.RespondAsync($"Banned **{member.Username}#{member.Discriminator}** with reason: `{reason}`");
        }

        [Command("ban"), Description("Ban an user"), RequirePermissions(Permissions.BanMembers)]
        public async Task Ban(CommandContext ctx, DiscordMember member, int deleteMessageDays, [RemainingText] string reason = "No reason")
        {
            await CheckMemberHierarchy(ctx, member);
            if(deleteMessageDays < 0 || deleteMessageDays > 7)
            {
                await ctx.RespondAsync("Argument `<deleteMessageDays>` must be between 0 and 7!");
                return;
            }

            await member.SendMessageAsync($"You were banned from **{ctx.Guild.Name}**. Reason: `{reason}`");

            await member.BanAsync(deleteMessageDays, reason);
            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Ban,
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });

            await ctx.RespondAsync($"Banned **{member.Username}#{member.Discriminator}** with reason: `{reason}`");
        }

        [Command("bans"), Description("Shows a list of bans"), RequirePermissions(Permissions.BanMembers)]
        public async Task Bans(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var bans = await ctx.Guild.GetBansAsync();

            var embedBase = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Config.settings.embedColor)
            };

            var pageCount = (bans.Count - 1) / 10 + 1;
            var pages = bans.Select((x, i) => new { str = $"{i+1}. **{x.User.Username}#{x.User.Discriminator}** (`{x.User.Id}`) - {x.Reason}", index = i })
                .GroupBy(x => x.index / 10)
                .Select(i => new Page
                {
                    Embed = embedBase.WithTitle($"Bans for **{ctx.Guild.Name}** ({i.Key + 1}/{pageCount})")
                        .WithDescription(string.Join("\n", i.Select(x => x.str).ToList()))
                });

            var emojis = new PaginationEmojis
            {
                SkipLeft = DiscordEmoji.FromUnicode("🏠"),
                SkipRight = null,
                Stop = DiscordEmoji.FromUnicode("⏹"),
                Left = DiscordEmoji.FromUnicode("⬆️"),
                Right = DiscordEmoji.FromUnicode("⬇️")
            };
            await interactivity.SendPaginatedMessageAsync(ctx.Channel, ctx.Member, pages, emojis);
        }

        [Command("unban"), Description("Removes user ban"), RequirePermissions(Permissions.BanMembers)]
        public async Task Unban(CommandContext ctx, DSharpPlus.Entities.DiscordUser user, [RemainingText] string reason = "No reason")
        {
            await ctx.Guild.UnbanMemberAsync(user);

            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Unban,
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = user.Username + "#" + user.Discriminator,
                Date = DateTime.UtcNow
            });

            await ctx.RespondAsync($"Removed ban **{user.Username}#{user.Discriminator}** with reason: `{reason}`");
        }

        [Command("kick"), Description("Kicks an user"), RequirePermissions(Permissions.KickMembers)]
        public async Task Kick(CommandContext ctx, DiscordMember member, [RemainingText] string reason = "No reason")
        {
            await CheckMemberHierarchy(ctx, member);

            await member.SendMessageAsync($"You were kicked from **{ctx.Guild.Name}**. Reason: `{reason}`");

            await member.RemoveAsync(reason);

            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Kick,
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });

            await ctx.RespondAsync($"Kicked **{member.Username}#{member.Discriminator}** with reason: `{reason}`");
        }

        [Command("mute"), Description("Mutes an user (so they cannot type/speak) for specified time"), RequirePermissions(Permissions.ManageRoles)]
        public async Task TempMute(CommandContext ctx, DiscordMember member, TimeSpan? time, [RemainingText] string reason = "No reason")
        {
            await CheckMemberHierarchy(ctx, member);

            if (time?.TotalSeconds < 60)
            {
                await ctx.RespondAsync("Minimal mute time is 60s");
                return;
            }

            var role = ctx.Guild.Roles.GetValueOrDefault(guild.MutedRoleId);

            if (role == null)
            {
                await ctx.RespondAsync("Creating muted role...");
                role = await ctx.Guild.CreateRoleAsync("Muted", Permissions.AccessChannels | Permissions.ReadMessageHistory, null, null, false);
                guild.MutedRoleId = role.Id;
                foreach (var channel in ctx.Guild.Channels.Values)
                {
                    _ = channel.AddOverwriteAsync(role, Permissions.None, Permissions.SendMessages | Permissions.Speak);
                }
            }

            await member.GrantRoleAsync(role);

            if (time != null)
            {
                var unmuteTime = DateTime.UtcNow + (TimeSpan) time;
                var check = await db.MutedUsers.Where(i => i.UserId == member.Id && i.GuildId == ctx.Guild.Id).FirstOrDefaultAsync();
                if (check == null)
                {
                    db.MutedUsers.Add(new MutedUser
                    {
                        UserId = member.Id,
                        GuildId = ctx.Guild.Id,
                        Time = unmuteTime
                    });
                } else
                {
                    check.Time = unmuteTime;
                    db.Entry(check).State = EntityState.Modified;
                }
            }
            
            await db.SaveChangesAsync();

            await ctx.RespondAsync($"Muted **{member.Mention}** with reason: `{reason}` {(time != null ? "for" : "")} {time?.ToHumanReadableString() ?? ""}");

            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Mute,
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });
        }

        [Command("mute"), RequirePermissions(Permissions.ManageRoles)]
        public async Task Mute(CommandContext ctx, DiscordMember member, [RemainingText] string reason = "No reason")
        {
            await TempMute(ctx, member, null, reason);
        }

        [Command("unmute"), Description("Unmute an user"), RequirePermissions(Permissions.ManageRoles)]
        public async Task Unmute(CommandContext ctx, DiscordMember member, [RemainingText] string reason = "No reason")
        {
            var role = ctx.Guild.Roles.GetValueOrDefault(guild.MutedRoleId);
            
            if (role == null)
            {
                await ctx.RespondAsync("Muted role not found");
                return;
            }

            await member.RevokeRoleAsync(role);

            await ctx.RespondAsync($"Unmuted **{member.Mention}** with reason: `{reason}`");

            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Unmute,
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });
        }

        [Command("warn"), Aliases("warning"), Description("Warns an user"), RequireUserPermissions(Permissions.ManageRoles)]
        public async Task Warn(CommandContext ctx, DiscordMember member, [RemainingText] string reason = "No reason")
        {
            var warnCount = await db.Entry(guild).Collection(i=>i.Warns).Query().Where(i => i.UserId == member.Id).CountAsync();

            await CheckMemberHierarchy(ctx, member);
            
            guild.Warns.Add(new Warn
            {
                UserId = member.Id,
                AuthorId = ctx.Member.Id,
                Reason = reason
            });

            db.Entry(guild).State = EntityState.Modified;
            await db.SaveChangesAsync();


            await ctx.RespondAsync($"Warned **{member.Mention}** with reason: `{reason}`. (total: {warnCount + 1} warns)");

            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Warn,
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });
        }

        [Command("warns"), Aliases("warnings"), Description("Display user warnings"), RequireUserPermissions(Permissions.ManageRoles)]
        public async Task WarnList(CommandContext ctx, DiscordMember member)
        {
            var warns = await db.Entry(guild).Collection(i => i.Warns).Query().Where(i => i.UserId == member.Id).ToListAsync();

            var embed = new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = $"{member.Username}#{member.Discriminator}",
                    IconUrl = member.GetAvatarUrl(ImageFormat.Png, 64)
                },
                Title = $"Found {warns.Count} warnings",
                Description = string.Join("\n", warns.Select((i, n) => $"{n + 1}. {i.Reason} (from {ctx.Guild.Members[i.AuthorId].Mention})").ToList()) + $"\n\n**Type `{ctx.Prefix}delwarn <member> <#>` to remove selected warning**",
                Color = new DiscordColor(Config.settings.embedColor)
            };
            await ctx.RespondAsync(embed: embed.Build());
        }

        [Command("delwarn"), Aliases("deletewarn", "unwarn"), Description("Removes specific warning from user"), RequireUserPermissions(Permissions.ManageRoles)]
        public async Task DelwarnInfo(CommandContext ctx, DiscordMember member = null)
        {
            if(member == null)
            {
                await ctx.RespondAsync($"Type `{ctx.Prefix}delwarn <member> <#>` to delete a warning. To see a list of warns for an user, type `{ctx.Prefix}warns <member>`");
            } else
            {
                await WarnList(ctx, member);
            }
        }

        [Command("delwarn")]
        public async Task RemoveWarn(CommandContext ctx, DiscordMember member, uint id)
        {
            var warns = await db.Entry(guild).Collection(i => i.Warns).Query().Where(i => i.UserId == member.Id).ToListAsync();
            var warn = warns.ElementAtOrDefault((int)id - 1);
            if(warn == null)
            {
                await ctx.RespondAsync($"Warning with id {id} for this user not found.");
            } else
            {
                guild.Warns.RemoveAll(i => i.Id == warn.Id);
                await db.SaveChangesAsync();

                await ctx.RespondAsync($"Removed warning with reason `{warn.Reason}` for **{member.Username}#{member.Discriminator}**.");
            }
            
        }

        [Command("purge"), Description("Deletes last specified amount of messages"), Aliases("clear"), RequirePermissions(Permissions.ManageMessages)]
        public async Task PurgeChat(CommandContext ctx, uint amount)
        {
            var messages = await ctx.Channel.GetMessagesAsync((int)amount + 1);

            await ctx.Channel.DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await ctx.RespondAsync($"Purge completed.");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }

        [Command("lock"), Description("Lock current channel, so people can't type there"), RequirePermissions(Permissions.ManageChannels)]
        public async Task LockChannel(CommandContext ctx)
        {
            await ctx.Channel.AddOverwriteAsync(ctx.Guild.EveryoneRole, deny: Permissions.SendMessages);
            await ctx.RespondAsync($"Channel is now locked. Type {ctx.Prefix}unlock to unlock it.");
        }

        [Command("unlock"), Description("Unlock current channel"), RequirePermissions(Permissions.ManageChannels)]
        public async Task UnlockChannel(CommandContext ctx)
        {
            var overwrite = ctx.Channel.PermissionOverwrites.Where(x => x.Type == OverwriteType.Role && x.Id == ctx.Guild.EveryoneRole.Id).FirstOrDefault();
            if(overwrite != null)
            {
                await overwrite.UpdateAsync(allow: overwrite.Allowed, deny: overwrite.Denied.Revoke(Permissions.SendMessages));
            }
            await ctx.RespondAsync("Unlocked the channel.");
        }
    }
}
