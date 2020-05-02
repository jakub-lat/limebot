using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PotatoBot.Models
{
    public class Warn
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public GuildData Guild { get; set; }
        public ulong UserId { get; set; }
        public ulong AuthorId { get; set; }
        public string Reason { get; set; }
    }
}
