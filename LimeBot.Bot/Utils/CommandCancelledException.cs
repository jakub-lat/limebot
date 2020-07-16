using System;

namespace LimeBot.Bot.Utils
{
    public class CommandCanceledException : Exception
    {
        public CommandCanceledException() { }
        public CommandCanceledException(string msg) : base(msg) { }
    }
}
