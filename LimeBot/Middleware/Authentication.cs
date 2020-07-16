using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using LimeBot.Bot;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LimeBot.DAL.Models;

namespace LimeBot.Middleware
{
    public class Authentication
    {
        public static async Task<bool> CheckAuth(HttpContext ctx, ulong? guildId=null)
        {
            var request = ctx.Request;
            if (!request.Headers.ContainsKey("Authorization")) return false;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Headers["Authorization"]);

                    var response = await client.GetAsync("https://discordapp.com/api/v7/users/@me");
                    response.EnsureSuccessStatusCode();
                    var str = await response.Content.ReadAsStringAsync();

                    var user = JsonConvert.DeserializeObject<DiscordUser>(str);

                    var guildsRes = await client.GetAsync("https://discordapp.com/api/v7/users/@me/guilds");
                    var guildsStr = await guildsRes.Content.ReadAsStringAsync();

                    user.Guilds = JsonConvert.DeserializeObject<List<DiscordGuild>>(guildsStr).Where(i=>(i.Permissions & 0x20) == 0x20).ToList();
                    foreach(var guild in user.Guilds)
                    {
                        guild.BotOnGuild = BotService.instance.IsOnGuild(ulong.Parse(guild.Id));
                    }

                    ctx.Items["User"] = user;

                    if(guildId==null) return true;
                    else
                    {
                        int? perms = user.Guilds.Find(i => i.Id == guildId.ToString())?.Permissions;
                        if (perms == null) return false;
                        else
                        {
                            return (perms & 0x20) == 0x20;
                        }
                    }
                }

            } catch
            {
                return false;
            }
        }
    }
}
