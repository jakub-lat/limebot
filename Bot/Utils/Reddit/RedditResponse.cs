using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoBot.Bot.Utils
{
    public class RedditPost
    {
        public string selftext;
        public string title;
        public string url;
        public string author;
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
