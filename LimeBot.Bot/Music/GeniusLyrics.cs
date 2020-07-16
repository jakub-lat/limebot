using System;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using LimeBot.DAL;

namespace LimeBot.Bot.Music
{
    
    public static class GeniusLyrics
    {
        private class GeniusSong
        {
            public string Path { get; set; }
        }
        private class GeniusHit
        {
            public GeniusSong Result { get; set; }
        }
        private class GeniusResponse
        {
            public List<GeniusHit> Hits { get; set; }
        }
        private class GeniusData
        {
            public GeniusResponse Response { get; set; }
        }

        static HttpClient client = new HttpClient();
        static string baseUrl = "https://api.genius.com/";

        static GeniusLyrics()
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Config.settings.GeniusApiKey);
        }

        public static async Task<string> GetLyrics(string search)
        {
            var data = await client.GetStringAsync(baseUrl + "search?q=" + HttpUtility.UrlEncode(search));
            var resp = JsonConvert.DeserializeObject<GeniusData>(data);
            if (resp.Response.Hits.Count < 1) return null;

            var songPath = resp.Response.Hits[0].Result.Path;

            var web = new HtmlWeb();
            var doc = web.Load("https://genius.com" + songPath);

            var lyricsRaw = doc.DocumentNode.QuerySelector("div.lyrics")?.InnerText;
            
            if (lyricsRaw == null) return null;
            var lyrics = Regex.Replace(Regex.Replace(lyricsRaw, @"\n+", "\n"), @"^More on genius[\D\d]*", "", RegexOptions.Multiline).Trim();
            return lyrics;
        }
    }
}
