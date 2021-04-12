using LimeBot.Bot.Attributes;
using LimeBot.Bot.Music;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LimeBot.Bot.Utils;
using DSharpPlus.Interactivity;
using LimeBot.Bot.Entities;
using System.Text.RegularExpressions;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using LimeBot.DAL;

namespace LimeBot.Bot.Commands
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
            if (!lava?.node?.IsConnected != true)
            {
                await ctx.RespondAsync("Lavalink node not connected! Reconnecting...");
                throw new CommandCanceledException();
            }
            gm = null;
            var requireVc = ctx.Command.CustomAttributes.FirstOrDefault(i => i.GetType() == typeof(RequireVcAttribute));
            var beforePlay = ctx.Command.CustomAttributes.FirstOrDefault(i => i.GetType() == typeof(BeforePlayAttribute));
            if(requireVc != null)
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
                gm ??= await lava.InitGuildMusic(ctx.Guild, ctx.Member.VoiceState, ctx.Channel, ctx.Prefix);
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

        [Command("play"), RequireVc]
        public async Task PlayResume(CommandContext ctx)
        {
            await Resume(ctx);
        }
        
        private async Task AddTracksToQueue(CommandContext ctx, LavalinkLoadResult trackLoad)
        {
            if (trackLoad.LoadResultType == LavalinkLoadResultType.LoadFailed || !trackLoad.Tracks.Any())
            {
                await ctx.RespondAsync($":warning: No tracks were found!");
                return;
            }

            if (trackLoad.Tracks.Count() == 1)
            {
                var track = trackLoad.Tracks.First();
                await gm.Add(trackLoad.Tracks.First());
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"Added **{track.Title}** to queue.",
                    Url = track.Uri.ToString(),
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = track.Uri.ToString().Contains("youtube")
                            ? $"http://img.youtube.com/vi/{track.Identifier}/mqdefault.jpg"
                            : null
                    }
                };
                await ctx.RespondAsync(embed: embed);
            } else
            {
                foreach (var track in trackLoad.Tracks)
                {
                    await gm.Add(track);
                }
                await ctx.RespondAsync($"Added {trackLoad.Tracks.Count()} tracks to queue.");
            }
        }

        [Command("play"), Aliases("p"), Description("Plays music from URL or searches YouTube")]
        [RequireBotPermissions(Permissions.UseVoice), RequireVc, BeforePlay]
        public async Task Play(CommandContext ctx, [Description("Track URL")] Uri uri)
        {
            var trackLoad = await lava.node.Rest.GetTracksAsync(uri);
            await AddTracksToQueue(ctx, trackLoad);
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
                    return;
                }

                var trackLoad = await lava.node.Rest.GetTracksAsync(new Uri("https://youtube.com/watch?v=" + result.id.videoId));
                await AddTracksToQueue(ctx, trackLoad);
            } catch
            {
                await ctx.RespondAsync("Error :worried:. Please try again.");
            }
        }
        

        [Command("skip"), Aliases("s"), Description("Skip to next song"), RequireVc]
        public async Task Skip(CommandContext ctx)
        {
            await gm.Next(true);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":track_next:"));
        }

        [Command("queue"), Aliases("q", "np", "nowplaying"), Description("Music queue")]
        public async Task Queue(CommandContext ctx)
        {
            if (gm?.player.CurrentState.CurrentTrack == null) { await ctx.RespondAsync("Not playing rn!"); return; }

            var interactivity = ctx.Client.GetInteractivity();

            var queue = gm.Queue.Select((item, index) =>
            {
                string x = index == gm.Index ? "▶️" : $"{index + 1}.";
                return $"{x} {item.Title.Truncate(30)}";
            }).ToList();


            var curr = gm.player.CurrentState;
            var ppos = curr.PlaybackPosition;
            var ctl = curr.CurrentTrack.Length;

            var duration = $"{ppos.Hours:00}:{ppos.Minutes:00}:{ppos.Seconds:00} / {ppos.Hours:00}:{ctl.Minutes:00}:{ctl.Seconds:00}";

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

        [Command("lyrics"), Description("Show lyrics for current song"), RequireVc]
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

        [Command("stop"), Aliases("leave", "disconnect"), Description("Clear queue and disconnect from VC"), RequireVc]
        public async Task Stop(CommandContext ctx)
        {
            if(ctx.Member?.VoiceState?.Channel != gm?.player?.Channel && gm?.player?.Channel?.Users.Count() != 1)
            {
                await ctx.RespondAsync(":warning: You need to be in the same voice channel as me!");
                return;
            }
            await gm?.Stop();
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":stop_button:"));
        }

        [Command("shuffle"), Description("Shuffle queue"), RequireVc]
        public async Task Shuffle(CommandContext ctx)
        {
            gm.Shuffle();
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":twisted_rightwards_arrows:"));
        }

        [Command("pause"), Description("Pause the track."), RequireVc]
        public async Task Pause(CommandContext ctx)
        {
            if(!gm.IsPaused)
            {
                await gm.Pause();
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pause_button:"));
            }
        }

        [Command("resume"), Description("Resume the track."), RequireVc]
        public async Task Resume(CommandContext ctx)
        {
            if (gm.IsPaused)
            {
                await gm.Resume();
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_forward:"));
            }
        }


        [Command("seek"), Description("Seeks to specified time (example: 2m30s)"), RequireVc]
        public async Task Seek(CommandContext ctx, [RemainingText] TimeSpan position)
        {
            if(position == null) throw new ArgumentException();
            await gm.player?.SeekAsync(position);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":fast_forward:"));
        }
        [Command("seek"), RequireVc]
        public async Task Seek(CommandContext ctx, int seconds)
        {
            await gm.player?.SeekAsync(TimeSpan.FromSeconds(seconds));
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":fast_forward:"));
        }

        [Command("forward"), Aliases("fastforward", "ff"), Description("Forward the track by given amount (example: 2m30s)"), RequireVc]
        public async Task Forward(CommandContext ctx, [RemainingText] TimeSpan position)
        {
            await gm.player?.SeekAsync(gm.player.CurrentState.PlaybackPosition + position);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":fast_forward:"));
        }

        [Command("rewind"), Aliases("re", "rew"), Description("Rewind the track by given amount (example: 2m30s)"), RequireVc]
        public async Task Rewind(CommandContext ctx, [RemainingText] TimeSpan position)
        {
            await gm.player?.SeekAsync(gm.player.CurrentState.PlaybackPosition - position);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":fast_forward:"));
        }


        [Command("remove"), Aliases("rm"), Description("Remove a track from queue by index"), RequireVc]
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
                } else if (index < gm.Index)
                {
                    gm.Index--;
                }
                await ctx.RespondAsync($":ok_hand: Removed **{track.Title}** from queue.");
            } catch
            {
                await ctx.RespondAsync($"Index out of range!");
            }
        }

        [Command("undo"), Description("Removes last added track"), RequireVc]
        public async Task Undo(CommandContext ctx)
        {
            await Remove(ctx, (uint)gm.Queue.Count);
        }

        [Command("goto"), Description("Go to specified track by index"), RequireVc]
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

        [Command("restart"), Aliases("replay"), Description("Replay the queue"), RequireVc]
        public async Task Restart(CommandContext ctx)
        {
            await GoTo(ctx, 1);
        }

        [Command("loop"), Description("Enables queue looping"), RequireVc]
        public async Task Loop(CommandContext ctx)
        {
            gm.Loop = !gm.Loop;
            await ctx.RespondAsync(gm.Loop ? "Enabled looping :repeat:" : "Disabled looping");
        }

        [Command("24-7"), Description("Toggles 24/7 mode, so bot won't disconnect when everyone leaves the voice chat"), RequireVc]
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

        [Command("volume"), Aliases("vol"), Description("Set player volume"), RequireVc]
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
