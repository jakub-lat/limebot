using System.Collections.Generic;
using Newtonsoft.Json;

namespace LimeBot.Bot.Utils.Reddit
{
    public class RedditPost
    {
        public string selftext;
        public string title;
        public string url;
        public string author;
        [JsonProperty("over_18")]
        public bool over18;
    }
    public class RedditChildren
    {
        public RedditPost data;
    }
    public class RedditData
    {
        public List<RedditChildren> children;
    }
    public class RedditResponse
    {
        public RedditData data;
        public List<RedditChildren> GetPosts()
        {
            return data.children;
        }
    }
}
