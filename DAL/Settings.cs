using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace PotatoBot
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string BotToken { get; set; }
        public string LavalinkPassword { get; set; }
        public string YoutubeApiKey { get; set; }
        public string GeniusApiKey { get; set; }
        public string DashboardURL { get; set; }
        public string DefaultPrefix { get; set; }
        public string embedColor = "2fbb84";
    }

    public static class Config
    {
        public static Settings settings;

        //                                                      $Jakub$             MysterDead          Hyopplo
        public static List<ulong> developer = new List<ulong> { 268047533698318337, 604643444404649995, 353514719262408704 };
        public static bool IsDevelopment { get { return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"; } }
        public static void Load()
        {
            if(IsDevelopment)
            {
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("env.development.json"));
            } else
            {
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("env.json"));
            }
        }
    }
}
