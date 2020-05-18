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
using PotatoBot;
using System.Diagnostics;
using PotatoBot.Utils;
using System.Reflection;
using System.Text;

namespace PotatoBot.Bot.Commands
{
    [Category("System")]
    public class SystemCommands : MyCommandModule
    {
        public SystemCommands(GuildContext db) : base(db) { }

        [Command("prefix"), Description("Get or set the prefix"), RequireGuild]
        public async Task GetPrefix(CommandContext ctx)
        {
            await ctx.RespondAsync($"The prefix is `{ctx.Prefix}`");
        }

        [Command("prefix"), RequireUserPermissions(DSharpPlus.Permissions.ManageMessages), RequireGuild]
        public async Task SetPrefix(CommandContext ctx, string newPrefix)
        {
            guild.Prefix = newPrefix;
            await db.SaveChangesAsync();
            await ctx.RespondAsync($"Set the prefix to `{newPrefix}`");
        }

        [Command("help"), Description("If you are stuck")]
        public async Task Help(CommandContext ctx)
        {
            var url = Config.settings.DashboardURL;

            var description = new StringBuilder()
                .AppendLine("Hey! I am **LIME**. Nice to see you ;)")
                .AppendLine($"If you are stuck, **[here is a list of my commands]({url}/commands)**.");
            if (ctx.Guild != null) description.AppendLine($"If you want to configure me, **[login to dashboard]({url}/manage/{ctx.Guild.Id})**");
            description.AppendLine($"\n_Protip: type `{ctx.Prefix}help <command>` to get detailed info about specified command_");
              

            var embed = new DiscordEmbedBuilder {
                Title = "Lime help",
                Description = description.ToString(),
                Color = new DiscordColor(Config.settings.embedColor)
            };
            await ctx.RespondAsync(null, false, embed.Build());
        }

        [Command("help")]
        public async Task HelpCommand(CommandContext ctx, string command)
        {
            var cnext = ctx.Client.GetCommandsNext();
            var cmd = cnext.RegisteredCommands.GetValueOrDefault(command);
            if(cmd == null)
            {
                await ctx.RespondAsync("Command not found!");
            } else
            {
                var desc = new StringBuilder();
                desc.AppendLine("**Description:**").AppendLine(cmd.Description).AppendLine();
                if (cmd.Aliases.Any()) desc.AppendLine($"**Aliases:** `{string.Join(", ", cmd.Aliases)}`").AppendLine();
                desc.AppendLine("**Usage:**")
                    .AppendLine($"```{string.Join("\n", cmd.Overloads.Select(o => $"{ctx.Prefix}{cmd.Name} {string.Join(" ", o.Arguments.Select(a => string.Format(a.IsOptional ? "[{0}]" : "<{0}>", a.Name)))}"))}```");
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"Usage: {cmd.Name}",
                    Color = new DiscordColor(Config.settings.embedColor),
                    Description = desc.ToString()
                };
                await ctx.RespondAsync(embed: embed);
            }
        }


        [Command("ping"), Description("Bot ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        }

        [Command("userinfo"), Aliases("user"), Description("User information"), RequireGuild]
        public async Task UserInfo(CommandContext ctx, DiscordMember user = null) {
            user ??= ctx.Member;
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{user.Username}#{user.Discriminator}",
                ThumbnailUrl = user.AvatarUrl,
                Color = new DiscordColor(Config.settings.embedColor),
            };

            if (user.Nickname != null) 
                embed.AddField("**Nickname**", user.Nickname, true);

            embed.AddField("**Id:**", ""+user.Id, true);

            embed.AddField("**Account created**", user.CreationTimestamp.DateTime.ToString("dd MMM yyyy"), true);

            embed.AddField("**Joined server**", user.JoinedAt.ToString("dd MMM yyyy"), true);

            embed.AddField("**Top role**", user.Roles.OrderByDescending(i => i.Position).FirstOrDefault()?.Mention ?? "None", true);

            await ctx.RespondAsync(embed: embed.Build());
        }


        [Command("info"), Aliases("botinfo"), Description("Bot information")]
        public async Task BotInfo(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Lime Bot info",
                Color = new DiscordColor(Config.settings.embedColor),
                Description = @"Lime is a professional, multi-purpose bot.
Website - [limebot.tk](https://limebot.tk)
Discord server - [Join Lime Bot support](https://discord.gg/9w9EfWh)"
            };
            var users = 0;
            var channels = 0;
            foreach (var g in ctx.Client.Guilds)
            {
                users += g.Value.MemberCount;
                channels += g.Value.Channels.Count;
            }
            embed.AddField("**Stats**",
$@"Guilds: {ctx.Client.Guilds.Count}
Users: {users}
Channels: {channels}");

            using(var proc = Process.GetCurrentProcess())
            {
                var memory = Math.Round(proc.PrivateMemorySize64 / 1e+6, 2);

                var cpu = Math.Round(proc.TotalProcessorTime / (DateTime.Now - proc.StartTime) * 100);
                embed.AddField("**Resource usage**", $"Memory: {memory} MB\nCPU: {cpu}%");
            }

            embed.AddField("**Versions**", 
$@".Net Core: 3.1
Asp.Net Core: 2.2.0
DSharpPlus: {ctx.Client.VersionString}");


            await ctx.RespondAsync(null, false, embed.Build());
        }
    }
}
