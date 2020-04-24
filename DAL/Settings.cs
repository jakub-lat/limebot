using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DAL
{
    public class Settings
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string BotToken { get; set; }
        public string LavalinkPassword { get; set; }
    }

    public static class Config
    {
        public static Settings settings;
        public static void Load()
        {
            settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("env.json"));
        }
    }
}
