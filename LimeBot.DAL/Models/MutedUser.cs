using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimeBot.DAL.Models
{
    public class MutedUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }
        public DateTime Time { get; set; }
    }
}
