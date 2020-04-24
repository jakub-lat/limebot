using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotatoBot.Models
{
    public class DiscordGuild {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Permissions { get; set; }
        public bool BotOnGuild { get; set; } = false;
    }

    public class DiscordUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string Avatar { get; set; }
        public List<DiscordGuild> Guilds { get; set; }
    }
}
