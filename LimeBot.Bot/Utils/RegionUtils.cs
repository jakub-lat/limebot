namespace LimeBot.Bot.Utils
{
    public class RegionUtils
    {
        public static string getRegion(string regs)
        {
            switch (regs)
            {
                case "europe":
                case "Europe":
                    return "\uD83C\uDDEA\uD83C\uDDFA Europe";
                case "Brazil":
                case "brazil":
                    return "\uD83C\uDDE7\uD83C\uDDF7 Brazil";
                case "hongkong":
                case "Hong Kong":
                    return "\uD83C\uDDED\uD83C\uDDF0 Honk Kong";
                case "india":
                case "India":
                    return "\uD83C\uDDEE\uD83C\uDDF3 India";
                case "Japan":
                case "japan":
                    return "\uD83C\uDDEF\uD83C\uDDF5 Japan";
                case "Russia":
                case "russia":
                    return "\uD83C\uDDF7\uD83C\uDDFA Russia";
                default:
                    return "Unknow";
            }
        }
    }
}