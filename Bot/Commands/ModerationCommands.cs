using Bot.Attributes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Net.Models;
using PotatoBot.Bot.Utils;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PotatoBot.Utils;
using Microsoft.EntityFrameworkCore;
using PotatoBot.Models;
using Bot.Extensions;

namespace PotatoBot.Bot.Commands
{
    [Category("Moderation")]
    public class ModerationCommands : MyCommandModule
    {
        public ModerationCommands(GuildContext db) : base(db) { }

        GuildData guild;

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            guild = await db.GetGuild(ctx.Guild.Id);
            await base.BeforeExecutionAsync(ctx);
        }

        [Command("ban"), Description("Ban an user"), RequirePermissions(Permissions.BanMembers)]
        public async Task Ban(CommandContext ctx, DiscordMember member, string reason = "No reason")
        {
            await member.SendMessageAsync($"You were banned from **{ctx.Guild.Name}**. Reason: `{reason}`");

            await member.BanAsync(30, reason);
            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Ban.ToString(),
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });
        }

        [Command("kick"), Description("Kicks an user"), RequirePermissions(Permissions.KickMembers)]
        public async Task Kick(CommandContext ctx, DiscordMember member, string reason = "No reason")
        {
            await member.SendMessageAsync($"You were kicked from **{ctx.Guild.Name}**. Reason: `{reason}`");

            await member.RemoveAsync(reason);

            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Kick.ToString(),
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });
        }

        [Command("mute"), Description("Mutes an user for specified time"), RequirePermissions(Permissions.ManageRoles)]
        public async Task TempMute(CommandContext ctx, DiscordMember member, TimeSpan? time, string reason = "No reason")
        {
            var role = ctx.Guild.Roles.GetValueOrDefault(guild.MutedRoleId);

            if(role == null)
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

            if(time != null)
            {
                var unmuteTime = DateTime.UtcNow + (time ?? new TimeSpan());
                var check = await db.MutedUsers.Where(i => i.UserId == member.Id && i.GuildId == ctx.Guild.Id).FirstOrDefaultAsync();
                if(check == null)
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
                
                await db.SaveChangesAsync();
            }

            await ctx.RespondAsync($"Muted **{member.Mention}** with reason: `{reason}` {(time != null ? "for" : "")} {time?.ToHumanReadableString() ?? ""}");

            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Mute.ToString(),
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });
        }

        [Command("mute")]
        public async Task Mute(CommandContext ctx, DiscordMember member, string reason = "No reason")
        {
            await TempMute(ctx, member, null, reason);
        }

        [Command("unmute"), Description("Unmutes an user"), RequirePermissions(Permissions.ManageRoles)]
        public async Task Unmute(CommandContext ctx, DiscordMember member, string reason = "No reason")
        {
            var role = ctx.Guild.Roles.GetValueOrDefault(guild.MutedRoleId);

            if (role == null) return;

            await member.RevokeRoleAsync(role);

            await ctx.RespondAsync($"Unmuted **{member.Mention}** with reason: `{reason}`");

            await db.AddLog(ctx.Guild, ctx.Member, guild, new GuildLog
            {
                Action = LogAction.Unmute.ToString(),
                Reason = reason,
                AuthorId = ctx.Member.Id,
                TargetUser = member.Username + "#" + member.Discriminator,
                Date = DateTime.UtcNow
            });
        }

        [Command("warn"), Aliases("warning"), Description("Warns an user"), RequireUserPermissions(Permissions.ManageRoles)]
        public async Task Warn(CommandContext ctx, DiscordMember member, string reason = "No reason")
        {
            var warnCount = await db.Entry(guild).Collection(i => i.Warns).Query().Where(i => i.UserId == member.Id).CountAsync();

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
                Action = LogAction.Warn.ToString(),
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
                Title = $"Warns for {member.Username}#{member.Discriminator}",
                Description = string.Join("\n", warns.Select((i, n) => $"**{n+1}**. {i.Reason} (from {ctx.Guild.Members[i.AuthorId].Mention})").ToList()),
                Color = new DiscordColor(Config.settings.embedColor)
            };
            await ctx.RespondAsync(embed: embed.Build());
        }

        [Command("purge"), Description("Deletes last specified amount of messages"), Aliases("clear"), RequireBotPermissions(Permissions.ManageMessages)]
        public async Task PurgeChat(CommandContext ctx, uint amount)
        {
            var messages = await ctx.Channel.GetMessagesAsync((int)amount + 1);

            await ctx.Channel.DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await ctx.RespondAsync($"Purge completed.");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
    }
}
