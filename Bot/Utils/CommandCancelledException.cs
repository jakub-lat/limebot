using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Utils
{
    public class CommandCanceledException : Exception
    {
        public CommandCanceledException() { }
        public CommandCanceledException(string msg) : base(msg) { }
    }
}
