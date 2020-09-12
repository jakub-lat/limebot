using System;

namespace LimeBot.Bot.Utils
{
    public class RegionUtils
    {
        public static string GetRegionEmoji(string region)
        {
            switch (region)
            {
                case "Europe":
                    return ":flag_eu:";
                case "Brazil":
                    return ":flag_br:";
                case "Hong Kong":
                    return ":flag_hk:";
                case "India":
                    return ":flag_in:";
                case "Japan":
                    return ":flag_jp:";
                case "Russia":
                    return ":flag_ru:";
                case "Singapore":
                    return ":flag_sg:";
                case "US South":
                case "US West":
                case "US Central":
                case "US East":
                    return ":flag_us:";
                case "South Africa":
                    return ":flag_za:";
                default:
                    return "";
            }
        }
        public static string GetRegionPrettyName(string region)
        {
            return $"{GetRegionEmoji(region)} {region}";
        }
    }
}