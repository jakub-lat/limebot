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

namespace PotatoBot.Bot.Commands
{
    [Category("Moderation")]
    public class ModerationCommands : MyCommandModule
    {
        public ModerationCommands(GuildContext db) : base(db) { }

        [Command("ban"), Description("Ban an user"), RequirePermissions(Permissions.BanMembers)]
        public async Task Ban(CommandContext ctx, DiscordMember member, string reason = "No reason")
        {
            var guild = await db.GetGuild(ctx.Guild.Id.ToString());
            await db.LoadLogs(guild);

            await member.SendMessageAsync($"You were banned from **{ctx.Guild.Name}**. Reason: `{reason}`");

            await member.BanAsync(30, reason);
            guild.Logs.Add(new GuildLog
            {
                Action = LogAction.Ban.ToString(),
                Reason = reason,
                AuthorId = ctx.Member.Id.ToString(),
                Details = member.Id.ToString(),
                Date = DateTime.UtcNow
            });
            await ctx.RespondAsync($"Banned **{member.Username}#{member.Discriminator}** with reason: `{reason}`");
            await db.SaveChangesAsync();
        }

        [Command("kick"), Description("Kicks an user"), RequirePermissions(Permissions.KickMembers)]
        public async Task Kick(CommandContext ctx, DiscordMember member, string reason = "No reason")
        {
            var guild = await db.GetGuild(ctx.Guild.Id.ToString());
            await db.LoadLogs(guild);

            await member.SendMessageAsync($"You were kicked from **{ctx.Guild.Name}**. Reason: `{reason}`");

            await member.RemoveAsync(reason);
            guild.Logs.Add(new GuildLog
            {
                Action = LogAction.Kick.ToString(),
                Reason = reason,
                AuthorId = ctx.Member.Id.ToString(),
                Details = member.Id.ToString(),
                Date = DateTime.UtcNow
            });
            await ctx.RespondAsync($"Kicked **{member.Username}#{member.Discriminator}** with reason: `{reason}`");
            await db.SaveChangesAsync();
        }

        [Command("mute"), Description("Mutes an user"), RequirePermissions(Permissions.ManageRoles)]
        public async Task Mute(CommandContext ctx, DiscordMember member, string reason = "No reason")
        {
            var guild = await db.GetGuild(ctx.Guild.Id.ToString());
            await db.LoadLogs(guild);

            var role = ctx.Guild.Roles.Values.FirstOrDefault(i => i.Id.ToString() == guild.MutedRoleId);

            if(role == null)
            {
                await ctx.RespondAsync("Creating muted role...");
                role = await ctx.Guild.CreateRoleAsync("Muted", Permissions.AccessChannels | Permissions.ReadMessageHistory, null, null, false);
                guild.MutedRoleId = role.Id.ToString();
                foreach(var channel in ctx.Guild.Channels.Values)
                {
                    _ = channel.AddOverwriteAsync(role, Permissions.None, Permissions.SendMessages | Permissions.Speak);
                }
            }

            await member.GrantRoleAsync(role);
            await ctx.RespondAsync($"Muted **{member.Username}#{member.Discriminator}** with reason: `{reason}`");

            guild.Logs.Add(new GuildLog
            {
                Action = LogAction.Mute.ToString(),
                Reason = reason,
                AuthorId = ctx.Member.Id.ToString(),
                Details = member.Id.ToString(),
                Date = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
    }
}
