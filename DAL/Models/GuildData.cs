using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PotatoBot.Models
{
    public enum LogAction
    {
        Kick, Ban, Warn, Mute
    }
    public class GuildLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public GuildData GuildData { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public string Reason { get; set; }
        public string AuthorId { get; set; }
        public DateTime Date { get; set; }
    }
    public class GuildData
    {
        public string Id { get; set; }
        public string Prefix { get; set; }
        public List<GuildLog> Logs { get; set; }
        public string MutedRoleId { get; set; }
    }
}
