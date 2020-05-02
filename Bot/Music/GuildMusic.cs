using PotatoBot;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Music
{
    public class GuildMusic
    {
        private Lavalink lava;
        private DiscordGuild guild;
        private DiscordChannel textChannel;

        private string prefix;

        private bool skipped = false;

        public LavalinkGuildConnection player;
        public List<LavalinkTrack> Queue { get; private set; } = new List<LavalinkTrack>();
        public int Index { get; private set; } = 0;

        public bool IsPaused { get; private set; } = false;

        public bool Is24_7 { get; set; } = false;

        public GuildMusic(Lavalink lava, DiscordGuild guild, DiscordChannel textChannel, string prefix = "$")
        {
            this.lava = lava;
            this.guild = guild;
            this.textChannel = textChannel;
            this.prefix = prefix;
        }

        public async Task InitPlayer(DiscordChannel vc)
        {
            player = await lava.node.ConnectAsync(vc);
            player.PlaybackFinished += PlaybackFinished;
        }

        private async Task PlaybackFinished(TrackFinishEventArgs e)
        {
            if (skipped) skipped = false;
            else if (Index >= Queue.Count - 1) {
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

        public async Task Next(bool skip = false)
        {
            if (Index < Queue.Count - 1)
            {
                Index++;
                if(skip) skipped = true;
                await Play();
                var embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Config.settings.embedColor),
                    Title = $"Now playing: **{Queue[Index].Title}**",
                    Url = Queue[Index].Uri.ToString()
                };
                await textChannel.SendMessageAsync(embed: embed);
            }
            else
            {
                await Stop();
            }
        }

        public async Task Stop()
        {
            await player.DisconnectAsync();
            lava.Delete(guild);
        }

        public async Task Disconnect()
        {
            await Stop();
            await textChannel.SendMessageAsync($"Disconnected. No one stayed with me :slight_frown: (to enable 24/7 music type `{prefix}24-7`)");
        }

        private static Random rng = new Random();
        public void Shuffle()
        {
            LavalinkTrack track = Queue[Index];
            Queue.RemoveAt(Index);

            int n = Queue.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                LavalinkTrack value = Queue[k];
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
