using DAL;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using DSharpPlus.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Music
{
    public class Lavalink
    {
        public LavalinkNodeConnection node;
        private DiscordClient client;
        private Dictionary<ulong, GuildMusic> guildMusic = new Dictionary<ulong, GuildMusic>();

        public Lavalink(DiscordClient discord)
        {
            client = discord;
            discord.Ready += Connect;
        }

        public async Task Connect(ReadyEventArgs e)
        {
            var lavaconfig = new LavalinkConfiguration
            {
                RestEndpoint = new ConnectionEndpoint { Hostname = "40.118.96.158", Port = 2333 },
                SocketEndpoint = new ConnectionEndpoint { Hostname = "40.118.96.158", Port = 2333 },
                Password = Config.settings.LavalinkPassword
            };
            var lavalinkExt = client.GetLavalink();
            node = await lavalinkExt.ConnectAsync(lavaconfig);
            Console.WriteLine("Lavalink ready");
        }

        public GuildMusic Get(DiscordGuild guild)
        {
            if (!guildMusic.ContainsKey(guild.Id)) return null;
            return guildMusic[guild.Id];
        }
        public async Task InitGuildMusic(DiscordGuild guild, DiscordVoiceState vs, DiscordChannel chn)
        {
            var gm = new GuildMusic(this, guild,  chn);
            await gm.InitPlayer(vs.Channel);
            guildMusic.Add(guild.Id, gm);
        }
        public void Delete(DiscordGuild guild)
        {
            guildMusic.Remove(guild.Id);
        }
    }
}
