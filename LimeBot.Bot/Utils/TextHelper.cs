namespace LimeBot.Bot.Utils
{
    public static class TextHelper
    {
        public static string Truncate(this string s, int maxlength)
        {
            return s.Length <= maxlength ? s : s.Substring(0, maxlength) + "...";
        }
    }
}
