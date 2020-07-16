using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LimeBot.Bot.Utils.Reddit
{
    public static class RedditHelper
    {
        private static HttpClient client = new HttpClient();
        private static string baseURL = "https://reddit.com/r";
        private static Random rnd = new Random();

        public static async Task<RedditResponse> Subreddit(string subreddit)
        {
            var response = await client.GetAsync($"{baseURL}/{subreddit}/hot.json");
            var status = (int)response.StatusCode;
            if(status < 200 || status > 299)
            {
                Console.WriteLine("Unsuccessful");
                throw new SubredditNotFoundException();
            }

            var body = await response.Content.ReadAsStringAsync();

            RedditResponse r = JsonConvert.DeserializeObject<RedditResponse>(body);
            return r;
        }

        public static async Task<RedditPost> GetRandom(string subreddit)
        {
            var response = await Subreddit(subreddit);
            var posts = response.GetPosts();
            if (posts.Count == 0) throw new SubredditNotFoundException();
            int n = rnd.Next(0, posts.Count);
            return posts[n].data;
        }
    }
}
