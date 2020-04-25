using Microsoft.EntityFrameworkCore.Internal;
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
        public ulong AuthorId { get; set; }
        public DateTime Date { get; set; }
    }
    public class GuildData
    {
        public ulong Id { get; set; }
        public string Prefix { get; set; }
        public List<GuildLog> Logs { get; set; }

        public ulong MutedRoleId { get; set; }

        public List<ulong> AutoRolesOnJoin { get; set; }

        public bool EnableWelcomeMessages { get; set; }
        public ulong WelcomeMessagesChannel { get; set; }
        public string WelcomeMessage { get; set; } = "Welcome, **{user}**!";
        public string LeaveMessage { get; set; } = "**{user}** left :(";
    }

}
