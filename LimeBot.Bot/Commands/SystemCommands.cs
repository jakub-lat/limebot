using LimeBot.Bot.Attributes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using LimeBot.Bot.Utils;
using LimeBot.DAL;

namespace LimeBot.Bot.Commands
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

        [Command("prefix"), RequireUserPermissions(Permissions.ManageMessages), RequireGuild]
        public async Task SetPrefix(CommandContext ctx, string newPrefix)
        {
            guild.Prefix = newPrefix;
            await db.SaveChangesAsync();
            await ctx.RespondAsync($"Set the prefix to `{newPrefix}`");
        }

        [Command("help"), Description("If you are stuck")]
        public async Task Help(CommandContext ctx)
        {
            var url = Config.settings.DashboardUrl;

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
            await ctx.RespondAsync(embed.Build());
        }

        [Command("help")]
        public async Task HelpCommand(CommandContext ctx, [RemainingText] string command)
        {
            var cnext = ctx.Client.GetCommandsNext();
            var cmd = cnext.FindCommand(command, out var _);
            if(cmd == null)
                await ctx.RespondAsync("Command not found!");
            else
                await CommandHelp.SendCommandHelp(ctx, cmd);
        }

        /*[Command("emote")]
        public async Task test(CommandContext ctx)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DiscordEmoji emote in ctx.Guild.Emojis.Values)
            {
                /*var embed = new DiscordEmbedBuilder();
                embed.Description = $"{emote} [URL]({emote.Url})";
                await ctx.RespondAsync(embed: embed.Build());#1#
                builder.Append($"{emote} `{emote}`\n");
            }

            /*builder.Append(ctx.Guild.Emojis.Values.Where(s => s.Name.Contains("EarlySupporterLogo")).First().Url);
            #1#
            await ctx.RespondAsync(builder.ToString());
        }*/

        [Command("server"), Aliases("serverinfo", "guild", "guildinfo"), Description("Guild information")]
        public async Task ServerInfo(CommandContext ctx)
        {
            var dcGuild = ctx.Guild;
            var textChns = dcGuild.Channels.Values.Count(x => x.Type == ChannelType.Text || x.Type == ChannelType.News);
            var voiceChns = dcGuild.Channels.Values.Count(x => x.Type == ChannelType.Voice);
            var category = dcGuild.Channels.Values.Count(a => a.IsCategory);
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{dcGuild.Name}",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = dcGuild.IconUrl
                },
                Color = new DiscordColor(Config.settings.embedColor)
            };
            embed.AddField("**ID**", $"{dcGuild.Id}", true);
            embed.AddField("**Owner**", $"{dcGuild.Owner.Mention}", true);
            embed.AddField("**Region**", $"{RegionUtils.GetRegionPrettyName(dcGuild.VoiceRegion.Name)}", true);
            embed.AddField("**Categories**", $"{category}", true);
            embed.AddField("**Text channels**", $"{textChns}", true);
            embed.AddField("**Voice channels**", $"{voiceChns}", true);
            embed.AddField("**Roles**", $"{dcGuild.Roles.Count}", true);
            embed.AddField("**Tier**", $"{dcGuild.PremiumTier.ToString()}", true);
            embed.AddField("**Verification level**", $"{dcGuild.VerificationLevel}", true);
            embed.AddField("**Boost level**", $"<a:flip_boost:748811555851862048> {GetBoostedLevel(dcGuild.PremiumTier)}", true);
            embed.AddField("**Boost count**", $"<a:boost:748811565372801074> {dcGuild.PremiumSubscriptionCount ?? 0}", true);
            embed.AddField("**Members**",dcGuild.MemberCount.ToString(), true);
            var builder = new StringBuilder();
            foreach (DiscordEmoji emote in dcGuild.Emojis.Values.Take(100))
            {
                builder.Append(emote).Append(" ");
            }

            if (dcGuild.Emojis.Values.Count() > 100)
            {
                builder.Append("and more...");
            }
            embed.AddField("**Created at**", $"{dcGuild.CreationTimestamp.DateTime:dd MMM yyyy}", true);
            embed.AddField("**Emotes**", $"{builder}");
            await ctx.RespondAsync(embed: embed.Build());
        }

        private int GetBoostedLevel(PremiumTier pr)
        {
            return pr switch
            {
                PremiumTier.Tier_1 => 1,
                PremiumTier.Tier_2 => 2,
                PremiumTier.Tier_3 => 3,
                _ => 0
            };
        }


        [Command("ping"), Description("Bot ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        }

        [Command("user"), Aliases("userinfo"), Description("User information")]
        public async Task MemberInfo(CommandContext ctx, DiscordMember member = null)
        {
            await UserInfo(ctx, member ?? ctx.Member);
        }

        [Command("user")]
        private async Task UserInfo(CommandContext ctx, DiscordUser user) {
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{user.Username}#{user.Discriminator}",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = user.AvatarUrl
                },
                Color = new DiscordColor(Config.settings.embedColor),
            };
            
            embed.AddField("**ID**", ""+user.Id, true);
            embed.AddField("**Account created**", user.CreationTimestamp.DateTime.ToString("dd MMM yyyy"), true);
            
            if (user is DiscordMember member)
            {
                if (!string.IsNullOrEmpty(member.Nickname))
                {
                    embed.AddField("**Nickname**", member.Nickname, true);
                }
                embed.AddField("**Joined server**", member.JoinedAt.ToString("dd MMM yyyy"), true);
                embed.AddField("**Top role**", member.Roles.OrderByDescending(i => i.Position).FirstOrDefault()?.Mention ?? "None", true);
                
                if (!user.IsBot)
                {
                    var warnCount = db.Entry(guild).Collection(i => i.Warns).Query()
                        .Count(i => i.UserId == user.Id && i.Guild.Id == ctx.Guild.Id);
                    embed.AddField("**Warn count**", $"{warnCount}", true);
                }
            }
               
            //embed.AddField("**Status**", $"{GetUserStatus(member.Presence?.ClientStatus)}", true);


           
            embed.AddField("**Badges**", $"{GetBadges(user)}");

            await ctx.RespondAsync(embed: embed.Build());
        }

        private string GetUserStatus(DiscordClientStatus cs)
        {
            var sb = new StringBuilder();
            if (cs?.Desktop.HasValue == true)
            {
                sb.AppendLine($"Desktop: {GetStatus(cs.Desktop.Value)}");
            }

            if (cs?.Mobile.HasValue == true)
            {
                sb.AppendLine($"Mobile: {GetStatus(cs.Mobile.Value)}");
            }

            if (cs?.Web.HasValue == true)
            {
                sb.AppendLine($"Web: {GetStatus(cs.Web.Value)}");
            }

            var str = sb.ToString();
            return string.IsNullOrEmpty(str) ? "<:offline:748817759055642675> Offline" : str;
        }

        private string GetStatus(UserStatus s)
        {
            switch (s)
            {
                case UserStatus.Idle:
                    return "<:idle:748817759097323572> Idle";
                case UserStatus.Offline:
                case UserStatus.Invisible:
                    return "<:offline:748817759055642675> Offline";
                case UserStatus.Online:
                    return "<:online:748817758862442578> Online";
                case UserStatus.DoNotDisturb:
                    return "<:dnd:748817759302975578> Do not disturb";
                default:
                    return "Unknown";
            }
        }

        private string GetBadges(DiscordUser u)
        {
            var flag = u.Flags ?? 0;
            var b = new StringBuilder();
            if (flag.HasFlag(UserFlags.System))
            {
                b.Append("<:system:748818876686663711> System\n");
            }

            if (flag .HasFlag(UserFlags.DiscordEmployee))
            {
                b.Append("<a:staff:748817758208262247> Discord employee\n");
            }

            if (flag .HasFlag(UserFlags.DiscordPartner))
            {
                b.Append("<:partner:748820104829075477> Discord partner\n");
            }

            if (flag .HasFlag(UserFlags.EarlySupporter))
            {
                b.Append("<:earlysupporter:748827055759818863> Early supporter\n");
            }

            if (flag .HasFlag(UserFlags.HouseBalance))
            {
                b.Append("<:balancelogo:748827055335931946> Balance house\n");
            }

            if (flag .HasFlag(UserFlags.HouseBravery))
            {
                b.Append("<:braverylogo:748827053926645771> Bravery house\n");
            }
            if (flag .HasFlag(UserFlags.HouseBrilliance))
            {
                b.Append("<:brillancelogo:748827054627225711> Brilliance house\n");
            }

            if (flag .HasFlag(UserFlags.TeamUser))
            {
                b.Append("<a:staff:748817758208262247> Team user\n");
            }

            if (flag .HasFlag(UserFlags.VerifiedBot))
            {
                b.Append("<:verifybot:748828868470636584> Verified Bot\n");
            }

            if (flag .HasFlag(UserFlags.HypeSquadEvents))
            {
                b.Append("<:hypesquadEvent:748827055147450390> HypeSquad Events\n");
            }

            if (flag .HasFlag(UserFlags.VerifiedBotDeveloper))
            {
                b.Append("<:developer:748827056120266793> Verified Bot Developer\n");
            }

            if (flag .HasFlag(UserFlags.BugHunterLevelOne))
            {
                b.Append("<:bughunterlogo:748827053519798323> Bug Hunter Level One\n");
            }
            if (flag .HasFlag(UserFlags.BugHunterLevelTwo))
            {
                b.Append("<:bughunter2logo:748830200778850395> Bug Hunter Level Two\n");
            }
            
            return b.ToString();
        }


        [Command("info"), Aliases("botinfo"), Description("LimeBot information")]
        public async Task BotInfo(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "LimeBot info",
                Color = new DiscordColor(Config.settings.embedColor),
                Description = @"Lime is a professional, multi-purpose bot.
Website - [limebot.tk](https://limebot.tk)
Invite to your server - [Invite](https://limebot.tk/api/redirect/invite)
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


            await ctx.RespondAsync(embed.Build());
        }
    }
}
