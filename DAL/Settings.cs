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
