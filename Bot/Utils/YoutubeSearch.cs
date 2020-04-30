using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Utils
{
    
    public class YoutubeItem
    {
        public class VideoId
        {
            public string videoId;
        }
        public class VideoSnippet {
            public class VideoThumbnails
            {
                public class VideoThumbnail
                {
                    public string url;
                }
                public VideoThumbnail @default;
            }

            public string title;
            public string channelTitle;
            public VideoThumbnails thumbnails;
        }

        public VideoId id;
        public VideoSnippet snippet;
    }
    public class YoutubeSearchResult
    {
        public List<YoutubeItem> items;
    }

    public class YoutubeSearch
    {
        string apiKey;
        HttpClient client;
        public YoutubeSearch()
        {
            apiKey = Config.settings.YoutubeApiKey;
            client = new HttpClient();
        }

        public async Task<YoutubeItem> Search(string query)
        {
            var resp = await client.GetAsync($"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=10&q={query}&key={apiKey}");
            resp.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<YoutubeSearchResult>(await resp.Content.ReadAsStringAsync());
            return result.items.Count > 0 ? result.items[0] : null;
        }
    }
}
