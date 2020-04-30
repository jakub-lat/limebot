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
using DSharpPlus.Interactivity;
using DAL;
using DSharpPlus.Interactivity.Enums;

namespace PotatoBot.Bot.Commands
{
    class CommandCanceledException : Exception
    {

    }

    [Category("Music")]
    public class MusicCommands : MyCommandModule
    {
        private Lavalink lava;
        private YoutubeSearch yt;
        private GuildMusic gm;
        public MusicCommands(GuildContext db, Lavalink lava) : base(db) {
            this.lava = lava;
            this.yt = new YoutubeSearch();
        }

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            var requireVC = ctx.Command.CustomAttributes.FirstOrDefault(i => i.GetType() == typeof(RequireVCAttribute));
            var beforePlay = ctx.Command.CustomAttributes.FirstOrDefault(i => i.GetType() == typeof(BeforePlayAttribute));
            if(requireVC != null)
            {
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    await ctx.RespondAsync(":warning: You need to be in a voice channel!");
                    throw new CommandCanceledException();
                }
            }

            if(beforePlay != null)
            {
                if (gm == null) await lava.InitGuildMusic(ctx.Guild, ctx.Member.VoiceState, ctx.Channel);
                if (gm?.player.Channel == null)
                {
                    var chn = ctx.Member.VoiceState.Channel;
                    await lava.node.ConnectAsync(chn);
                }
                else if (gm?.player.Channel != ctx.Member.VoiceState.Channel)
                {
                    await ctx.RespondAsync(":warning: Already playing on different channel");
                    throw new CommandCanceledException();
                }
            } else if (gm == null) throw new CommandCanceledException();
            
            gm = lava.Get(ctx.Guild);

            await base.BeforeExecutionAsync(ctx);
        }


        [Command("play"), Aliases("p"), Description("Plays music from URL or searches YouTube")]
        [RequireBotPermissions(Permissions.UseVoice), RequireVC, BeforePlay]
        public async Task Play(CommandContext ctx, [Description("Track URL")] Uri uri)
        {
            var trackLoad = await lava.node.Rest.GetTracksAsync(uri);
            if (trackLoad.LoadResultType == LavalinkLoadResultType.LoadFailed || !trackLoad.Tracks.Any())
            {
                await ctx.RespondAsync($":warning: No tracks were found!");
                return;
            }

            if(trackLoad.Tracks.Count() == 1)
            {
                await gm.Add(trackLoad.Tracks.First());
                await ctx.RespondAsync($"Added **{trackLoad.Tracks.First().Title}** to queue");
            } else
            {
                foreach(var track in trackLoad.Tracks)
                {
                    await gm.Add(track);
                }
                await ctx.RespondAsync($"Added {trackLoad.Tracks.Count()} tracks to queue");
            }
        }

        [Command("play"), BeforePlay]
        public async Task PlaySearch(CommandContext ctx, [RemainingText] string query)
        {
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
        [Command("play"), RequireVC]
        public async Task PlayResume(CommandContext ctx)
        {
            await Resume(ctx);
        }

        [Command("skip"), Aliases("s"), Description("Skip to next song"), RequireVC]
        public async Task Skip(CommandContext ctx)
        {
            await gm.Next();
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":track_next:"));
        }

        [Command("queue"), Aliases("q"), Description("Music queue")]
        public async Task Queue(CommandContext ctx)
        {
            if (gm == null || gm.player.CurrentState.CurrentTrack == null) { await ctx.RespondAsync("Not playing rn!"); return; }

            var interactivity = ctx.Client.GetInteractivity();

            var queue = gm.Queue.Select((item, index) =>
            {
                string x = index == gm.Index ? "▶️" : $" {index + 1}.";
                return $"{x} {(item.Title.Length <= 30 ? item.Title : item.Title.Substring(0, Math.Min(item.Title.Length, 30)) + "...")}";
            }).ToList();


            var curr = gm.player.CurrentState;
            var ppos = curr.PlaybackPosition;
            var ctl = curr.CurrentTrack.Length;

            var duration = $"{ppos.Minutes:00}:{ppos.Seconds:00} / {ctl.Minutes:00}:{ctl.Seconds:00}";

            var percent = (curr.PlaybackPosition / curr.CurrentTrack.Length) * 20;
            var position = "[";

            for (var i = 0; i < percent - 1; i++) position += "=";
            position += ">";

            for (var y = 0; y < 20 - percent - 1; y++) position += " ";
            position += "]";

            var pages = new List<Page>();
            var pageCount = (queue.Count - 1) / 10 + 1;
            for (var i = 0; i < queue.Count; i += 10)
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = $":notes: Queue for {ctx.Guild.Name} ({(i/10)+1}/{pageCount})",
                    Description = $@"```{string.Join("\n", queue.Skip(i).Take(10).ToList())} {(gm.Queue.Count > 10 + i ? "\n..." : "")}```
                    Now playing: **{gm.Queue[gm.Index].Title}**
                    ```{(gm.isPaused ? "⏸️" : "▶️")} {duration} {position}```",
                    Color = new DiscordColor(Config.settings.embedColor)
                };
                pages.Add(new Page(embed: embed));
            }

            var emojis = new PaginationEmojis
            {
                SkipLeft = null,
                SkipRight = null,
                Stop = DiscordEmoji.FromUnicode("⏹"),
                Left = DiscordEmoji.FromUnicode("⬆️"),
                Right = DiscordEmoji.FromUnicode("⬇️")
            };

            await interactivity.SendPaginatedMessageAsync(ctx.Channel, ctx.Member, pages, emojis, PaginationBehaviour.Ignore);
            
        }

        [Command("volume"), Aliases("vol"), Description("Set player volume"), RequireVC]
        public async Task Volume(CommandContext ctx, int volume)
        {
            await gm.player.SetVolumeAsync(volume);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":ok_hand:"));
        }

        [Command("stop"), Description("Clear queue and disconnect from VC")]
        public async Task Stop(CommandContext ctx)
        {
            await gm.Stop();
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":stop_button:"));
        }

        [Command("shuffle"), Description("Shuffle queue"), RequireVC]
        public async Task Shuffle(CommandContext ctx)
        {
            gm.Shuffle();
            await ctx.RespondAsync("Shuffled!");
        }

        [Command("pause"), Description("Pause the track."), RequireVC]
        public async Task Pause(CommandContext ctx)
        {
            if(!gm.isPaused)
            {
                await gm.Pause();
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pause_button:"));
            }
        }

        [Command("resume"), Description("Resume the track."), RequireVC]
        public async Task Resume(CommandContext ctx)
        {
            if (gm.isPaused)
            {
                await gm.Resume();
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_forward:"));
            }
        }

        [Command("seek"), Description("Seeks to specified time"), RequireVC]
        public async Task Seek(CommandContext ctx, [RemainingText] TimeSpan position)
        {
            Console.WriteLine(position);
            await gm.player?.SeekAsync(position);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":fast_forward:"));
        }
    }
}
