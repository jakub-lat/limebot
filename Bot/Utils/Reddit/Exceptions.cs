using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoBot.Bot.Utils
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
