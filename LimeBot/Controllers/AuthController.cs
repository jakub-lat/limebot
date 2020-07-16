using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LimeBot.DAL;
using LimeBot.Middleware;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using LimeBot.DAL.Models;

namespace LimeBot.Controllers
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
        public IActionResult Index([FromQuery] string redirect = null)
        {
            if(redirect != null)
            {
                Response.Cookies.Append("redirect", redirect, new CookieOptions { Expires = DateTime.Now.AddMinutes(2) });
            }
            var redirectTo = Request.Scheme + "://" + Request.Host + "/api/auth/callback";
            return Redirect($"https://discordapp.com/api/oauth2/authorize?client_id={Config.settings.ClientId}&redirect_uri={redirectTo}&response_type=code&scope=guilds%20identify");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            using var client = new HttpClient();
            var redirect = Request.Scheme + "://" + Request.Host + "/api/auth/callback";

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", Config.settings.ClientId),
                new KeyValuePair<string, string>("client_secret", Config.settings.ClientSecret),
                new KeyValuePair<string, string>("redirect", redirect)
            });
            var resp = await client.PostAsync($"https://discordapp.com/api/oauth2/token?grant_type=authorization_code&code={code}&redirect_uri={redirect}", content);
            var data = JsonConvert.DeserializeObject<DiscordTokenResponse>(await resp.Content.ReadAsStringAsync());
                
            return Redirect($"/callback?token={data.AccessToken}&redirect={Request.Cookies["redirect"] ?? ""}");
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            if (!await Authentication.CheckAuth(HttpContext)) return Unauthorized();
            return Ok((DiscordUser)HttpContext.Items["User"]);
        }
    }
}