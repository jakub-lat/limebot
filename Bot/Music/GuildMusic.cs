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
        public LavalinkGuildConnection player;
        public List<LavalinkTrack> Queue { get; private set; } = new List<LavalinkTrack>();
        public int Index { get; private set; } = 0;
        private bool skipped = false;

        public GuildMusic(Lavalink lava, DiscordGuild guild, DiscordChannel textChannel)
        {
            this.lava = lava;
            this.guild = guild;
            this.textChannel = textChannel;
        }

        public async Task InitPlayer(DiscordChannel vc)
        {
            player = await lava.node.ConnectAsync(vc);
            player.PlaybackFinished += PlaybackFinished;
        }

        private async Task PlaybackFinished(TrackFinishEventArgs e)
        {
            if (skipped) skipped = false;
            else await Skip();
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

        public async Task Skip()
        {
            if (Index < Queue.Count)
            {
                Index++;
                skipped = true;
                Play();
                await textChannel.SendMessageAsync($"Now playing: **{Queue[Index].Title}**");
            }
            else
            {
                await player.StopAsync();
                await textChannel.SendMessageAsync("Queue ended");
            }
        }

        public async Task Stop()
        {
            await player.DisconnectAsync();
            lava.Delete(guild);
        }
    }
}
