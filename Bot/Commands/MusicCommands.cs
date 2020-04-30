using Bot;
using Bot.Attributes;
using Bot.Music;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using PotatoBot.Bot.Utils;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PotatoBot.Utils;


namespace PotatoBot.Bot.Commands
{
    [Category("Music")]
    public class MusicCommands : MyCommandModule
    {
        private Lavalink lava;
        public MusicCommands(GuildContext db, Lavalink lava) : base(db) {
            this.lava = lava;
        }

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            var chn = ctx.Member?.VoiceState?.Channel;
            if (chn == null)
            {
                await ctx.RespondAsync(":warning: You need to be in a voice channel!");
                throw new Exception();
            }
        }

        private async Task<GuildMusic> BeforePlay(CommandContext ctx)
        {
            var guildConn = lava.Get(ctx.Guild);

            if (guildConn?.player.Channel == null)
            {
                var chn = ctx.Member.VoiceState.Channel;
                await lava.node.ConnectAsync(chn);
            }
            else if (guildConn?.player.Channel != ctx.Member.VoiceState.Channel)
            {
                await ctx.RespondAsync(":warning: Already playing on different channel");
                throw new Exception();
            }
            if (guildConn == null) await lava.InitGuildMusic(ctx.Guild, ctx.Member.VoiceState, ctx.Channel);
            return lava.Get(ctx.Guild);
        }

        [Command("play"), Aliases("p"), Description("Plays music from URL or searches YouTube"), RequireBotPermissions(Permissions.UseVoice)]
        public async Task Play(CommandContext ctx, [Description("Track URL")] Uri uri)
        {
            var gm = await BeforePlay(ctx);

            var trackLoad = await lava.node.Rest.GetTracksAsync(uri);
            if (trackLoad.LoadResultType == LavalinkLoadResultType.LoadFailed || !trackLoad.Tracks.Any())
            {
                await ctx.RespondAsync($":warning: No tracks were found!");
                return;
            }

            await gm.Add(trackLoad.Tracks.First());
            await ctx.RespondAsync("Queued 1 track(s)");
        }

        [Command("skip"), Aliases("s"), Description("Skip to next song")]
        public async Task Skip(CommandContext ctx)
        {
            var gm = lava.Get(ctx.Guild);
            if(gm != null)
            {
                await gm.Skip();
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":track_next:"));
            }
        }

        [Command("queue"), Aliases("q"), Description("Music queue")]
        public async Task Queue(CommandContext ctx)
        {
            var gm = lava.Get(ctx.Guild);
            if (gm == null) await ctx.RespondAsync("Not playing rn!");
            else
            {
                var queue = gm.Queue.Select((item, index) => {
                    string x = index == gm.Index ? "▶️" : (index + 1).ToString();
                    return $"{x}.  {item.Title}";
                }).ToList();
                await ctx.RespondAsync($"```{string.Join("\n", queue)}```");
            }
        }

        [Command("volume"), Aliases("vol"), Description("Set player volume")]
        public async Task Volume(CommandContext ctx, int volume)
        {
            var gm = lava.Get(ctx.Guild);
            if(gm != null)
            {
                await gm.player.SetVolumeAsync(volume);
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":ok_hand:"));
            }
        }

        [Command("stop"), Description("Clear queue and disconnect from VC")]
        public async Task Stop(CommandContext ctx)
        {
            var gm = lava.Get(ctx.Guild);
            if (gm != null)
            {
                await gm.Stop();
                await ctx.RespondAsync(":octagonal_sign: Stopped");
            }
        }

        [Command("shuffle"), Description("Shuffle queue")]
        public async Task Shuffle(CommandContext ctx)
        {
            var gm = lava.Get(ctx.Guild);
            if (gm != null)
            {
                await gm.Shuffle();
                await ctx.RespondAsync("Shuffled!");
            }
        }
    }
}
