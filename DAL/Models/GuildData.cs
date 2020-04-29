using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PotatoBot.Models
{
    public enum LogAction
    {
        Kick, Ban, Warn, Mute
    }
    [Keyless]
    public class GuildLog
    {
        public GuildData GuildData { get; set; }
        public string Action { get; set; }
        public string TargetUser { get; set; }
        public string Reason { get; set; }
        public ulong AuthorId { get; set; }
        public DateTime Date { get; set; }
    }
    public class GuildData
    {

        public ulong Id { get; set; }
        public string Prefix { get; set; }

        [Column(TypeName = "jsonb")]
        public List<GuildLog> Logs { get; set; } = new List<GuildLog>();

        public bool EnableModLogs { get; set; }
        public ulong ModLogsChannel { get; set; }

        public ulong MutedRoleId { get; set; }

        public ulong[] AutoRolesOnJoin { get; set; }

        public bool EnableWelcomeMessages { get; set; }
        public ulong WelcomeMessagesChannel { get; set; }
        public string WelcomeMessage { get; set; } = "Welcome, **{user}**! You are the {members}. member.";
        public string LeaveMessage { get; set; } = "**{user}** left :(";
    }

}
