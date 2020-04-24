using DAL;
using DSharpPlus;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    class Lavalink
    {
        public static LavalinkNodeConnection node;
        private DiscordClient client;
        public Lavalink(DiscordClient discord)
        {
            client = discord;
            Connect();
        }

        public async Task Connect()
        {
            var lavaconfig = new LavalinkConfiguration
            {
                RestEndpoint = new ConnectionEndpoint { Hostname = "40.118.96.158", Port = 2333 },
                SocketEndpoint = new ConnectionEndpoint { Hostname = "40.118.96.158", Port = 80 },
                Password = Config.settings.LavalinkPassword
            };
            var lavalinkExt = client.UseLavalink();
            node = await lavalinkExt.ConnectAsync(lavaconfig);
        }
    }
}
