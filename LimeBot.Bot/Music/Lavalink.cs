using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DSharpPlus.Lavalink.EventArgs;
using LimeBot.DAL;
using Microsoft.EntityFrameworkCore;
using DiscordChannel = DSharpPlus.Entities.DiscordChannel;
using DiscordGuild = DSharpPlus.Entities.DiscordGuild;

namespace LimeBot.Bot.Music
{
    public class Lavalink
    {
        public LavalinkNodeConnection node;
        private DiscordClient client;
        private Dictionary<ulong, GuildMusic> guildMusic = new Dictionary<ulong, GuildMusic>();
        
        private readonly LavalinkConfiguration lavaconfig = new LavalinkConfiguration
        {
            RestEndpoint = new ConnectionEndpoint { Hostname = "40.118.96.158", Port = 2333 },
            SocketEndpoint = new ConnectionEndpoint { Hostname = "40.118.96.158", Port = 2333 },
            Password = Config.settings.LavalinkPassword
        };

        public Lavalink(DiscordClient discord)
        {
            client = discord;
            discord.Ready += Connect;
        }

        public async Task Connect(ReadyEventArgs e)
        {
            var lavalinkExt = client.GetLavalink();
            node = await lavalinkExt.ConnectAsync(lavaconfig);
            node.Disconnected += Disconnected;
            Console.WriteLine("Lavalink ready");
        }

        private async Task Disconnected(NodeDisconnectedEventArgs args)
        {
            Console.WriteLine("Reconnecting Lavalink...");
            foreach (var (_, gm) in guildMusic)
            {
                gm?.Disconnect("Lavalink node disconnected!");
            }
            
            
            while (!node.IsConnected)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                Console.WriteLine("Reconnecting Lavalink after 10s...");
                try
                {
                    await Connect(null);
                }
                catch
                {
                    // ignore
                }
            }
            
            
        }
        
        public GuildMusic Get(DiscordGuild guild)
        {
            return guildMusic.GetValueOrDefault(guild.Id);
        }
        public async Task<GuildMusic> InitGuildMusic(DiscordGuild guild, DiscordVoiceState vs, DiscordChannel chn, string prefix)
        {
            var gm = new GuildMusic(this, guild, chn, prefix);
            await gm.InitPlayer(vs.Channel);
            guildMusic.Add(guild.Id, gm);
            return gm;
        }
        public void Delete(DiscordGuild guild)
        {
            guildMusic.Remove(guild.Id);
        }

        public async Task VoiceStateUpdated(VoiceStateUpdateEventArgs e)
        {
            await using var ctx = new GuildContext();
            var gm = Get(e.Guild);
            if (!node.IsConnected || gm == null) return;

            if (e.Guild?.CurrentMember?.VoiceState?.Channel?.Id != gm?.player?.Channel?.Id)
            {
                Delete(e.Guild);
                return;
            }
            
            if (!gm.Is24_7 && e.Before?.Channel == gm?.player?.Channel && gm?.player?.Channel?.Users.Count() == 1)
            {
                var guildData = await ctx.Guilds.FirstOrDefaultAsync(x => x.Id == e.Guild.Id);
                await gm?.Disconnect($"No one stayed with me :slight_frown: (to enable 24/7 music type `{guildData?.Prefix ?? Config.settings.DefaultPrefix}24-7`)");
            }
        }
    }
}
