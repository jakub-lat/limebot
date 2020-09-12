using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LimeBot.DAL;
using DiscordChannel = DSharpPlus.Entities.DiscordChannel;
using DiscordGuild = DSharpPlus.Entities.DiscordGuild;

namespace LimeBot.Bot.Music
{
    public class GuildMusic
    {
        private Lavalink lava;
        private DiscordGuild guild;
        private DiscordChannel textChannel;
        private DiscordChannel vc;

        private string prefix;

        private bool skipped = false;

        public LavalinkGuildConnection player;
        
        public List<LavalinkTrack> Queue { get; private set; } = new List<LavalinkTrack>();
        public int Index { get; set; } = 0;

        public bool IsPaused { get; private set; } = false;

        // ReSharper disable once InconsistentNaming
        public bool Is24_7 { get; set; } = false;
        public bool Loop { get; set; } = false;

        public GuildMusic(Lavalink lava, DiscordGuild guild, DiscordChannel textChannel, string prefix = "$")
        {
            this.lava = lava;
            this.guild = guild;
            this.textChannel = textChannel;
            this.prefix = prefix;
        }

        public async Task InitPlayer(DiscordChannel vc)
        {
            this.vc = vc;
            player = await lava.node.ConnectAsync(vc);
            player.PlaybackFinished += PlaybackFinished;
            await player.SetVolumeAsync(50);
        }

        private async Task PlaybackFinished(TrackFinishEventArgs e)
        {
            if (skipped) skipped = false;
            else if (Index >= Queue.Count - 1 && !Loop)
            {
                await Stop();
                await textChannel.SendMessageAsync("Queue ended");
            }
            else await Next();
        }

        public async Task Add(LavalinkTrack track)
        {
            Queue.Add(track);
            if (Queue.Count == 1)
            {
                await Play();
            }
        }

        private async Task Play()
        {
            await player.PlayAsync(Queue[Index]);
        }

        public async Task GoTo(int i, bool skip = false)
        {
            if (i < Queue.Count)
            {
                Index = i;
                if (skip) skipped = true;
                await Play();
                var embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Config.settings.embedColor),
                    Title = $"Now playing: **{Queue[Index].Title}**",
                    Url = Queue[Index].Uri.ToString()
                };
                await textChannel.SendMessageAsync(embed: embed);
            }
            else if (Loop) await GoTo(0, skip);
            else await Stop();
        }

        public async Task Next(bool skip = false)
        {
            await GoTo(Index + 1, skip);
        }

        public async Task Stop()
        {
            if(lava.node.IsConnected) await player.DisconnectAsync();
            lava.Delete(guild);
        }

        public async Task Disconnect(string reason)
        {
            await Stop();
            await textChannel.SendMessageAsync($"Disconnected - {reason}");
        }

        private static Random rng = new Random();
        public void Shuffle()
        {
            var track = Queue[Index];
            Queue.RemoveAt(Index);

            int n = Queue.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = Queue[k];
                Queue[k] = Queue[n];
                Queue[n] = value;
            }

            Queue.Insert(Index, track);
        }

        public async Task Pause()
        {
            await player.PauseAsync();
            IsPaused = true;
        }

        public async Task Resume()
        {
            await player.ResumeAsync();
            IsPaused = false;
        }
    }
}
