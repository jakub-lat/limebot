using Bot.Attributes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PotatoBot.Bot.Utils;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;

namespace PotatoBot.Bot.Commands
{
    [Category("System")]
    public class SystemCommands : MyCommandModule
    {
        public SystemCommands(GuildContext db) : base(db) { }

        [Command("guild")]
        public async Task GuildInfo(CommandContext ctx)
        {
            var guild = await db.GetGuild(ctx.Guild.Id);
            await db.LoadLogs(guild);
            await ctx.RespondAsync($"ID: `{guild.Id}`, Prefix: {guild.Prefix}, Muted role: {guild.MutedRoleId}, Logs count: {guild.Logs.Count}");
        }

        [Command("prefix"), Description("Get or set the prefix")]
        public async Task GetPrefix(CommandContext ctx)
        {
            var guild = await db.GetGuild(ctx.Guild.Id);
            await ctx.RespondAsync($"The prefix is `{guild.Prefix}`");
        }

        [Command("prefix"), RequireUserPermissions(DSharpPlus.Permissions.ManageMessages)]
        public async Task SetPrefix(CommandContext ctx, string newPrefix)
        {
            var guild = await db.GetGuild(ctx.Guild.Id);
            guild.Prefix = newPrefix;
            await db.SaveChangesAsync();
            await ctx.RespondAsync($"Set the prefix to `{newPrefix}`");
        }

        [Command("help"), Description("If you are stuck")]
        public async Task Help(CommandContext ctx)
        {
            var url = "https://potatodiscordbot.azurewebsites.net";
            var embed = new DiscordEmbedBuilder {
                Title = "LimeBOT help",
                Description = $"Hey! I am **LIME**.\n If you are stuck, **[here is a list of my commands]({url}/commands)**.\nIf you want to configure me, **[login to dashboard]({url}/manage/{ctx.Guild.Id})**",
                Color = new DiscordColor("#0de25f")
            };
            await ctx.RespondAsync(null, false, embed.Build());
        }

        [Command("ping"), Description("Bot ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        }

        [Command("userinfo"), Description("Information for commands author")]
        public async Task UserInfo(CommandContext ctx) {
            var member = ctx.Member;
            string role = "";
            foreach (var r in member.Roles)
            {
                role = r.Mention;
            }
            var embed = new DiscordEmbedBuilder
            {
                Title = $"Information as {member.Username}#{member.Discriminator}",
                ThumbnailUrl = member.AvatarUrl,
                Color = new DiscordColor("#0de25f"),
            };
            var nickname = member.Nickname;
            if (nickname == null)
            {
                nickname = "none";
            }
            embed.AddField("**Username:**", member.Username, true);
            embed.AddField("**Nickname:**", nickname, true);
            embed.AddField("**Id:**", ""+member.Id, true);
            embed.AddField("**Role:**", role, true);
            if (Config.developer.Contains(member.Id))
            {
                embed.AddField("**Developer:**", "true", true);
            }
            await ctx.RespondAsync(null, false, embed.Build());
        }

        [Command("userinfo"), Description("Information for mention member")]
        public async Task UserInfo(CommandContext ctx, DiscordMember member)
        {
            string role = null;
            foreach (var r in member.Roles)
            {
                role = r.Mention;
            }
            var embed = new DiscordEmbedBuilder
            {
                Title = $"Information as {member.Username}#{member.Discriminator}",
                ThumbnailUrl = member.AvatarUrl,
                Color = new DiscordColor("#0de25f"),
            };
            var nickname = member.Nickname;
            if (nickname == null)
            {
                nickname = "none";
            }
            embed.AddField("**Username:**", member.Username, true);
            embed.AddField("**Nickname:**", nickname, true);
            embed.AddField("**Id:**", "" + member.Id, true);
            embed.AddField("**Role:**", role, true);
            if (Config.developer.Contains(member.Id))
            {
                embed.AddField("**Developer:**", "true", true);
            }
            await ctx.RespondAsync(null, false, embed.Build());
        }

        [Command("botinfo"), Description("Bot information")]
        public async Task BotInfo(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "LimeBOT info",
                Color = new DiscordColor("#0de25f")
            };
            var users = 0;
            var textchannels = 0;
            var voicechannels = 0;
            var category = 0;
            foreach (var g in ctx.Client.Guilds)
            {
                users = users+g.Value.MemberCount;
                foreach(var channel in g.Value.Channels)
                {
                    if (channel.Value.Type==ChannelType.Category)
                    {
                        category = category + 1;
                    }
                    if (channel.Value.Type==DSharpPlus.ChannelType.Text)
                    {
                        textchannels = textchannels + 1;
                    }
                    if (channel.Value.Type==DSharpPlus.ChannelType.Voice)
                    {
                        voicechannels = voicechannels + 1;
                    }
                }
            }
            embed.AddField("**Guilds:**", $"{ctx.Client.Guilds.Count}", true);
            if (Config.IsDevelopment)
            {
                embed.AddField("**Version:**", "Development", true);
            }
            else
            {
                embed.AddField("**Version:**", "Stable", true);
            }
            embed.AddField("**Users:**", $"{users}", true);
            embed.AddField("**Categories:**", $"{category}", true);
            embed.AddField("**Text Channels:**", $"{textchannels}", true);
            embed.AddField("**Voice Channels:**", $"{voicechannels}", true);
            await ctx.RespondAsync(null, false, embed.Build());
        }
    }
}
