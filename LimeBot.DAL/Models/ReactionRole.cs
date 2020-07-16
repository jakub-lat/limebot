using System.ComponentModel.DataAnnotations.Schema;

namespace LimeBot.DAL.Models
{
    public class ReactionRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ulong MessageId { get; set; }
        public string MessageJumpLink { get; set; }
        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong RoleId { get; set; }
        public string Emoji { get; set; }
    }
}
