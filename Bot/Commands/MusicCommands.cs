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
using PotatoBot;
using DSharpPlus.Interactivity.Enums;
using Bot.Entities;
using System.Text.RegularExpressions;

namespace PotatoBot.Bot.Commands
{

    [Category("Music"), RequireGuild]
    public class MusicCommands : MyCommandModule
    {
        private Lavalink lava;
        private YoutubeSearch yt;
        private GuildMusic gm;
        public MusicCommands(GuildContext db, Lavalink lava) : base(db) {
            this.lava = lava;
            yt = new YoutubeSearch();
        }

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            gm = null;
            var requireVC = ctx.Command.CustomAttributes.FirstOrDefault(i => i.GetType() == typeof(RequireVCAttribute));
            var beforePlay = ctx.Command.CustomAttributes.FirstOrDefault(i => i.GetType() == typeof(BeforePlayAttribute));
            if(requireVC != null)
            {
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    await ctx.RespondAsync("You need to be in a voice channel!");
                    throw new CommandCanceledException();
                }
            }

            gm = lava.Get(ctx.Guild);
            if (beforePlay != null)
            {
                if (gm == null) gm = await lava.InitGuildMusic(ctx.Guild, ctx.Member.VoiceState, ctx.Channel, ctx.Prefix);
                if (gm?.player.Channel == null)
                {
                    var chn = ctx.Member.VoiceState.Channel;
                    await lava.node.ConnectAsync(chn);
                }
                else if (gm?.player.Channel != ctx.Member.VoiceState.Channel)
                {
                    throw new CommandCanceledException("Already playing on different channel");
                }
            }
            
            if (gm == null) throw new CommandCanceledException("Not playing anything on this server");

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
                await ctx.RespondAsync($"Added **{trackLoad.Tracks.First().Title}** to queue.");
            } else
            {
                foreach(var track in trackLoad.Tracks)
                {
                    await gm.Add(track);
                }
                await ctx.RespondAsync($"Added {trackLoad.Tracks.Count()} tracks to queue.");
            }
        }

        [Command("play"), RequireVC]
        public async Task PlayResume(CommandContext ctx)
        {
            await Resume(ctx);
        }

        [Command("play"), BeforePlay]
        public async Task PlaySearch(CommandContext ctx, [RemainingText] string query)
        {
            try
            {
                var result = await yt.Search(query);

                if (result == null)
                {
                    await ctx.RespondAsync(":warning: Nothing found :(");
                }
                else
                {
                    var trackLoad = await lava.node.Rest.GetTracksAsync("https://youtube.com/watch?v=" + result.id.videoId);
                    var track = trackLoad.Tracks.Where(x => x.Identifier == result.id.videoId).FirstOrDefault();
                    if (trackLoad.LoadResultType == LavalinkLoadResultType.LoadFailed || !trackLoad.Tracks.Any() || track == null)
                    {
                        await ctx.RespondAsync($":warning: Something went wrong :( Try again!");
                        return;
                    }
                    await gm.Add(track);
                    await ctx.RespondAsync($"Added **{track.Title}** to queue");
                }
            } catch
            {
                await ctx.RespondAsync("Error :worried:. Please try again.");
            }
        }
        

        [Command("skip"), Aliases("s"), Description("Skip to next song"), RequireVC]
        public async Task Skip(CommandContext ctx)
        {
            await gm.Next(true);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":track_next:"));
        }

        [Command("queue"), Aliases("q", "np", "nowplaying"), Description("Music queue")]
        public async Task Queue(CommandContext ctx)
        {
            if (gm == null || gm.player.CurrentState.CurrentTrack == null) { await ctx.RespondAsync("Not playing rn!"); return; }

            var interactivity = ctx.Client.GetInteractivity();

            var queue = gm.Queue.Select((item, index) =>
            {
                string x = index == gm.Index ? "▶️" : $"{index + 1}.";
                return $"{x} {item.Title.Truncate(30)}";
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
```{(gm.IsPaused ? "⏸️" : "▶️")} {duration} {position}```",
                    Color = new DiscordColor(Config.settings.embedColor)
                };
                pages.Add(new Page(embed: embed));
            }

            var emojis = new PaginationEmojis
            {
                SkipLeft = DiscordEmoji.FromUnicode("🏠"),
                SkipRight = null,
                Stop = DiscordEmoji.FromUnicode("🛑"),
                Left = DiscordEmoji.FromUnicode("⬆️"),
                Right = DiscordEmoji.FromUnicode("⬇️")
            };

            var paginated = new MyPaginatedMessage(ctx, pages, emojis, gm.Index / 10);
            await paginated.SendAsync();
            await interactivity.WaitForCustomPaginationAsync(paginated);
        }

        [Command("lyrics"), Description("Show lyrics for current song"), RequireVC]
        public async Task Lyrics(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var lyrics = await GeniusLyrics.GetLyrics(new Regex(@"\(([^\)]+)\)|\[([^\]]+)\]|ft\.[\d\D]*").Replace(gm.Queue[gm.Index].Title, ""));

            if(string.IsNullOrEmpty(lyrics))
            {
                await ctx.RespondAsync(":worried: Lyrics for this song not found!");
                return;
            }
            var pages = interactivity.GeneratePagesInEmbed(lyrics, SplitType.Line, new DiscordEmbedBuilder
            {
                Title = $"Lyrics for {gm.Queue[gm.Index].Title}",
                Color = new DiscordColor(Config.settings.embedColor)
            });
            var paginated = new MyPaginatedMessage(ctx, pages.ToList());
            await paginated.SendAsync();
            await interactivity.WaitForCustomPaginationAsync(paginated);
        }

        [Command("stop"), Aliases("leave", "disconnect"), Description("Clear queue and disconnect from VC")]
        public async Task Stop(CommandContext ctx)
        {
            if(ctx.Member?.VoiceState?.Channel != gm?.player?.Channel && gm?.player?.Channel?.Users.Count() != 1)
            {
                await ctx.RespondAsync(":warning: You need to be in the same voice channel as me!");
                return;
            }
            await gm.Stop();
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":stop_button:"));
        }

        [Command("shuffle"), Description("Shuffle queue"), RequireVC]
        public async Task Shuffle(CommandContext ctx)
        {
            gm.Shuffle();
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":twisted_rightwards_arrows:"));
        }

        [Command("pause"), Description("Pause the track."), RequireVC]
        public async Task Pause(CommandContext ctx)
        {
            if(!gm.IsPaused)
            {
                await gm.Pause();
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pause_button:"));
            }
        }

        [Command("resume"), Description("Resume the track."), RequireVC]
        public async Task Resume(CommandContext ctx)
        {
            if (gm.IsPaused)
            {
                await gm.Resume();
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_forward:"));
            }
        }


        [Command("seek"), Description("Seeks to specified time (example: 2m30s)"), RequireVC]
        public async Task Seek(CommandContext ctx, [RemainingText] TimeSpan position)
        {
            await gm.player?.SeekAsync(position);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":fast_forward:"));
        }

        [Command("forward"), Aliases("fastforward", "ff"), Description("Forward the track by given amount (example: 2m30s)"), RequireVC]
        public async Task Forward(CommandContext ctx, [RemainingText] TimeSpan position)
        {
            await gm.player?.SeekAsync(gm.player.CurrentState.PlaybackPosition + position);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":fast_forward:"));
        }

        [Command("rewind"), Aliases("re", "rew"), Description("Rewind the track by given amount (example: 2m30s)"), RequireVC]
        public async Task Rewind(CommandContext ctx, [RemainingText] TimeSpan position)
        {
            await gm.player?.SeekAsync(gm.player.CurrentState.PlaybackPosition - position);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":fast_forward:"));
        }


        [Command("remove"), Aliases("rm"), Description("Remove a track from queue by index"), RequireVC]
        public async Task Remove(CommandContext ctx, uint index)
        {
            index--;
            try
            {
                var track = gm.Queue?[(int)index];
                gm.Queue?.RemoveAt((int)index);
                if (index == gm.Index)
                {
                    await gm.Next(true);
                }
                await ctx.RespondAsync($":ok_hand: Removed **{track.Title}** from queue.");
            } catch
            {
                await ctx.RespondAsync($"Index out of range!");
            }
        }

        [Command("undo"), Description("Removes last added track"), RequireVC]
        public async Task Undo(CommandContext ctx)
        {
            await Remove(ctx, (uint)gm.Queue.Count);
        }

        [Command("goto"), Description("Go to specified track by index"), RequireVC]
        public async Task GoTo(CommandContext ctx, uint index)
        {
            index--;
            if(index < 1 || index >= gm.Queue.Count)
            {
                await ctx.RespondAsync("Index out of range!");
                return;
            }
            await gm.GoTo((int)index, true);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":track_next:"));
        }

        [Command("restart"), Aliases("replay"), Description("Replay the queue"), RequireVC]
        public async Task Restart(CommandContext ctx)
        {
            await GoTo(ctx, 1);
        }

        [Command("loop"), Description("Enables queue looping"), RequireVC]
        public async Task Loop(CommandContext ctx)
        {
            gm.Loop = !gm.Loop;
            await ctx.RespondAsync(gm.Loop ? "Enabled looping :repeat:" : "Disabled looping");
        }

        [Command("24-7"), Description("Toggles 24/7 mode, so bot won't disconnect when everyone leaves the voice chat"), RequireVC]
        public async Task Toggle24_7(CommandContext ctx)
        {
            gm.Is24_7 = !gm.Is24_7;
            if(gm.Is24_7)
            {
                await ctx.RespondAsync("Enabled **24/7 mode**! Now I won't leave when the channel will be empty.");
            } else
            {
                await ctx.RespondAsync("Disabled **24/7 mode**. I will now leave when the channel will be empty.");
            }
        }

        [Command("volume"), Aliases("vol"), Description("Set player volume"), RequireVC]
        public async Task Volume(CommandContext ctx, int volume)
        {
            if (volume < 0 || volume > 150)
            {
                await ctx.RespondAsync("Volume must be between 0 and 150");
                return;
            }
            await gm.player.SetVolumeAsync(volume);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":ok_hand:"));
        }
    }
}
