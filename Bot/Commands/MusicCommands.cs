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
using Bot.Utils;

namespace PotatoBot.Bot.Commands
{
    [Category("Music")]
    public class MusicCommands : MyCommandModule
    {
        private Lavalink lava;
        private YoutubeSearch yt;
        public MusicCommands(GuildContext db, Lavalink lava) : base(db) {
            this.lava = lava;
            this.yt = new YoutubeSearch();
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
            await ctx.RespondAsync($"Added **{trackLoad.Tracks.First().Title}** to queue");
        }

        [Command("play")]
        public async Task PlaySearch(CommandContext ctx, [RemainingText] string query)
        {
            var gm = await BeforePlay(ctx);

            var result = await yt.Search(query);
            if(result == null)
            {
                await ctx.RespondAsync(":warning: Nothing found :(");
            } else
            {
                var trackLoad = await lava.node.Rest.GetTracksAsync("https://youtube.com/watch?v=" + result.id.videoId);
                if (trackLoad.LoadResultType == LavalinkLoadResultType.LoadFailed || !trackLoad.Tracks.Any())
                {
                    await ctx.RespondAsync($":warning: No tracks were found!");
                    return;
                }

                await gm.Add(trackLoad.Tracks.First());
                await ctx.RespondAsync($"Added **{trackLoad.Tracks.First().Title}** to queue");
            }
        }
        [Command("play")]
        public async Task PlayResume(CommandContext ctx)
        {
            await Resume(ctx);
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
            if (gm == null || gm.player.CurrentState.CurrentTrack == null) await ctx.RespondAsync("Not playing rn!");
            else
            {
                var queue = gm.Queue.Take(25).Select((item, index) => {
                    string x = index == gm.Index ? "▶️" : $"{index + 1}.";
                    return $"{x} {item.Title}";
                }).ToList();

                var curr = gm.player.CurrentState;

                var duration = $"{curr.PlaybackPosition.Minutes}:{curr.PlaybackPosition.Seconds} / {curr.CurrentTrack.Length.Minutes}:{curr.CurrentTrack.Length.Seconds}";

                var percent = (curr.PlaybackPosition / curr.CurrentTrack.Length) * 20;
                var position = "[";
                
                for(var i=0; i<percent-1; i++) position += "=";
                position += ">";

                for(var y=0; y<20-percent-1; y++) position += " ";
                position += "]";

                var embed = new DiscordEmbedBuilder
                {
                    Title = $":notes: Queue for {ctx.Guild.Name}",
                    Description = $@"```
                    {string.Join("\n", queue)}
                    {(gm.Queue.Count > 25 ? "..." : "")}
                    ```
                    Now playing: **{gm.Queue[gm.Index].Title}**
                    ```{(gm.isPaused ? "⏸️" : "▶️")} {duration} {position}```
                    "
                };
                await ctx.RespondAsync(embed: embed);
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
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":stop_button:"));
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

        [Command("pause"), Description("Pause the track.")]
        public async Task Pause(CommandContext ctx)
        {
            var gm = lava.Get(ctx.Guild);
            if (gm != null)
            {
                await gm.Pause();
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pause_button:"));
            }
        }

        [Command("resume"), Description("Resume the track.")]
        public async Task Resume(CommandContext ctx)
        {
            var gm = lava.Get(ctx.Guild);
            if (gm != null && gm.isPaused)
            {
                await gm.Resume();
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_forward:"));
            }
        }
    }
}
