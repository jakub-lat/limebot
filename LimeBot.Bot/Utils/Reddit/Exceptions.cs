using System;

namespace LimeBot.Bot.Utils.Reddit
{
    public class SubredditNotFoundException : Exception
    {
        public SubredditNotFoundException()
        {
        }

        public SubredditNotFoundException(string message)
            : base(message)
        {
        }

        public SubredditNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
