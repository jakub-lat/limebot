using LimeBot.Bot.Attributes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using LimeBot.DAL.Models;
using System.Diagnostics;
using System.Text;
using LimeBot.Bot.Utils;
using LimeBot.DAL;
using DiscordChannel = DSharpPlus.Entities.DiscordChannel;
using DiscordGuild = DSharpPlus.Entities.DiscordGuild;
using DiscordUser = DSharpPlus.Entities.DiscordUser;

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
            await ctx.RespondAsync(null, false, embed.Build());
        }

        /*[Command("test")]
        public async Task ok(CommandContext ctx)
        {
            //for testing
        }*/

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

        [Command("serverinfo"), Aliases("server", "guild", "guildinfo"), Description("Guild information")]
        public async Task ServerInfo(CommandContext ctx)
        {
            var ds = ctx.Guild;
            var textchannels = ds.Channels.Values.Where(a => isTextChannel(a)).Count();
            var voicechannels = ds.Channels.Values.Where(a => isVoiceChannel(a)).Count();
            var category = ds.Channels.Values.Where(a => a.IsCategory).Count();
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{ds.Name}",
                ThumbnailUrl = ds.IconUrl,
                Color = new DiscordColor(Config.settings.embedColor)
            };
            embed.AddField("**Id:**", $"{ds.Id}", true);
            embed.AddField("**Owner:**", $"{ds.Owner.Mention}", true);
            embed.AddField("**Region:**", $"{RegionUtils.getRegion(ds.VoiceRegion.Name)}", true);
            embed.AddField("**Category**", $"{category}", true);
            embed.AddField("**Text channel**", $"{textchannels}", true);
            embed.AddField("**Voice channel**", $"{voicechannels}", true);
            embed.AddField("**Roles:**", $"{ds.Roles.Count()}", true);
            embed.AddField("**Tier:**", $"{ds.PremiumTier.ToString()}", true);
            embed.AddField("**Verification level:**", $"{ds.VerificationLevel}", true);
            embed.AddField("**Boost level**", $"<a:flip_boost:748811555851862048> {getBoostedLevel(ds.PremiumTier)}", true);
            embed.AddField("**Boost count**", $"<a:boost:748811565372801074> {ds.PremiumSubscriptionCount.Value}", true);
            embed.AddField("**Members:**", $"Bots: {ds.Members.Values.Where(a=>a.IsBot).Count()}\nPeople: {ds.Members.Values.Where(a=>!a.IsBot).Count()}", true);
            StringBuilder builder = new StringBuilder();
            foreach (DiscordEmoji emote in ds.Emojis.Values.Take(100))
            {
                builder.Append(emote).Append(" ");
            }

            if (ds.Emojis.Values.Count() > 100)
            {
                builder.Append("and more...");
            }
            embed.AddField("**Created at:**", $"{ds.CreationTimestamp.DateTime.ToString("dd MMM yyyy")}", true);
            embed.AddField("**Emotes:**", $"{builder}", false);
            await ctx.RespondAsync(embed: embed.Build());
        }

        public bool isTextChannel(DiscordChannel d)
        {
            switch (d.Type)
            {
                case ChannelType.News:
                case ChannelType.Text:
                    return true;
                default:
                    return false;
            }
        }

        public int getBoostedLevel(PremiumTier pr)
        {
            switch (pr)
            {
                case PremiumTier.Tier_1:
                    return 1;
                case PremiumTier.Tier_2:
                    return 2;
                case PremiumTier.Tier_3:
                    return 3;
                default:
                    return 0;
            }
        }
        
        public bool isVoiceChannel(DiscordChannel d)
        {
            switch (d.Type)
            {
                case ChannelType.Voice:
                    return true;
                default:
                    return false;
            }
        }


        [Command("ping"), Description("LimeBot.Bot ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        }

        [Command("userinfo"), Aliases("user"), Description("User information"), RequireGuild]
        public async Task UserInfo(CommandContext ctx, DiscordMember user = null) {
            user ??= ctx.Member;
            var warnCount = db.Entry(guild).Collection(i => i.Warns).Query().Where(i => i.UserId == user.Id)
                .Count();
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{user.Username}#{user.Discriminator}",
                ThumbnailUrl = user.AvatarUrl,
                Color = new DiscordColor(Config.settings.embedColor),
            };

            if (user.Nickname != null) 
                embed.AddField("**Nickname**", user.Nickname, true);
            embed.AddField("**Id:**", ""+user.Id, true);
            embed.AddField("**Account created:**", user.CreationTimestamp.DateTime.ToString("dd MMM yyyy"), true);
            embed.AddField("**Joined server:**", user.JoinedAt.ToString("dd MMM yyyy"), true);
            embed.AddField("**Top role:**", user.Roles.OrderByDescending(i => i.Position).FirstOrDefault()?.Mention ?? "None", true);
            try
            {
                embed.AddField("**Status:**", $"{getStatus(user.Presence.Status)}", true);
                embed.AddField("**Client status:**", $"{getClinetStatus(user.Presence.ClientStatus)}", true);
            }
            catch
            {
                embed.AddField("**Status:**", $"{getStatus(UserStatus.Offline)}", true);
                embed.AddField("**Client status:**", "Unknow", true);
            }

            if (!user.IsBot)
            {
                embed.AddField("**Warns count:**", $"{warnCount}", true);
            }
            embed.AddField("**Badge:**", $"{getBadges(user)}", true);
            //embed.AddField("**Badge:**", $"{user.}", true);

            await ctx.RespondAsync(embed: embed.Build());
        }

        public string getClinetStatus(DiscordClientStatus cs)
        {
            if (cs.Desktop.HasValue)
            {
                return "🖥️ Desktop";
            }
            if (cs.Mobile.HasValue)
            {
                return "📱 Mobile";
            }
            if (cs.Web.HasValue)
            {
                return "🌐 Web";
            }
            return "Unknow";
        }

        public string getStatus(UserStatus s)
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
                    return "Unknow";
            }
        }

        public string getBadges(DiscordMember s)
        {
            UserFlags flag = s.Flags.Value;
            StringBuilder b = new StringBuilder();
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


        [Command("info"), Aliases("botinfo"), Description("LimeBot.Bot information")]
        public async Task BotInfo(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Lime LimeBot.Bot info",
                Color = new DiscordColor(Config.settings.embedColor),
                Description = @"Lime is a professional, multi-purpose bot.
Website - [limebot.tk](https://limebot.tk)
Discord server - [Join Lime LimeBot.Bot support](https://discord.gg/9w9EfWh)"
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
