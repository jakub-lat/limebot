using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimeBot.DAL.Models
{
    public class GuildMember
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ulong UserId { get; set; }
        public GuildData Guild { get; set; }

        public int XP { get; set; } = 0;
        public DateTime LastMessaged { get; set; }
    }

    public class RankingMember
    {
        public int XP { get; set; }
        public int Level { get; set; }
        public int NextLevelPercent { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string AvatarURL { get; set; }
    }
}
