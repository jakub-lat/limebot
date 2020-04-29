using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using PotatoBot.Middleware;
using PotatoBot.Models;

namespace PotatoBot.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class AuthController : Controller
    {
        private struct DiscordTokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }

        [HttpGet]
        public IActionResult Index()
        {
            var redirect = Request.Scheme + "://" + Request.Host + "/api/auth/callback";
            return Redirect($"https://discordapp.com/api/oauth2/authorize?client_id={Config.settings.ClientID}&redirect_uri={redirect}&response_type=code&scope=guilds%20identify");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            
            using(var client = new HttpClient()) {
                var redirect = Request.Scheme + "://" + Request.Host + "/api/auth/callback";

                var byteArray = Encoding.ASCII.GetBytes($"{Config.settings.ClientID}:{Config.settings.ClientSecret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var resp = await client.PostAsync($"https://discordapp.com/api/oauth2/token?grant_type=authorization_code&code={code}&redirect_uri={redirect}", null);
                var data = JsonConvert.DeserializeObject<DiscordTokenResponse>(await resp.Content.ReadAsStringAsync());

                return Redirect($"/callback?token={data.AccessToken}");
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            if (!await Authentication.CheckAuth(HttpContext)) return Unauthorized();
            return Ok((DiscordUser)HttpContext.Items["User"]);
        }
    }
}