using DAL.Models;
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

    public class DiscordRole
    { 
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Position { get; set; }
    }

    public class DiscordChannel
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Parent { get; set; }
    }

    public class GuildInfo
    {
        public IEnumerable<DiscordRole> Roles { get; set; }
        public IEnumerable<DiscordChannel> Channels { get; set; }
    }

    public class GuildResult
    {
        public ulong Id { get; set; }
        public GuildData Settings { get; set; }
        public GuildInfo Info { get; set; }
    }
}
