﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LimeBot.DAL.Models
{
    public enum LogAction
    {
        Kick, Ban, Warn, Mute, Unmute, Unban
    }
    public class GuildLog
    {
        public GuildData GuildData { get; set; }

        public LogAction Action { get; set; }

        public string TargetUser { get; set; }
        public string Reason { get; set; }
        public ulong AuthorId { get; set; }
        public DateTime Date { get; set; }
    }

    public enum WarnAction
    {
        Kick, Ban, Mute1d, Mute2d, Mute3d, Mute7d
    }

    public class GuildData
    {

        public ulong Id { get; set; }
        public string Prefix { get; set; }

        public List<Warn> Warns { get; set; }

        public bool EnableModLogs { get; set; }
        public ulong ModLogsChannel { get; set; }

        public bool EnableMessageLogs { get; set; }
        public ulong MessageLogsChannel { get; set; }

        public ulong MutedRoleId { get; set; }

        public ulong[] AutoRolesOnJoin { get; set; }

        public bool EnableWelcomeMessages { get; set; }
        public ulong WelcomeMessagesChannel { get; set; }
        public string WelcomeMessage { get; set; } = "Welcome, **{user}**! You are the {members}. member.";
        public string LeaveMessage { get; set; } = "**{user}** left :(";

        public bool ReactionRolesNotifyDM { get; set; } = false;


        public bool EnableLeveling { get; set; } = true;
        public bool EnableLevelUpMessage { get; set; } = true;
        public string LevelUpMessage { get; set; } = "Gz {user}, you just got to level {level}!";
        public ulong? LevelUpMessageChannel { get; set; } = null; // null = current channel

        [Range(5, 1000)]
        public int RequiredXPToLevelUp { get; set; } = 350;

        [Range(2, 100)]
        public int MinMessageXP { get; set; } = 10;
        [Range(2, 100)]
        public int MaxMessageXP { get; set; } = 25;

        [Column(TypeName = "jsonb")]
        public List<RoleForLevel> RolesForLevel { get; set; } = new List<RoleForLevel>();

        public List<GuildMember> Members { get; set; }

        public bool EnableReputation { get; set; } = false;
        
        [Range(2, 100)]
        public int ReputationXP { get; set; } = 20;
    }

}
