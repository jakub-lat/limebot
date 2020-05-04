using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
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
}
