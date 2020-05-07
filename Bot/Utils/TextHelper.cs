using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Utils
{
    public static class TextHelper
    {
        public static string Truncate(this string s, int maxlength)
        {
            return s.Length <= maxlength ? s : s.Substring(0, maxlength) + "...";
        }
    }
}
