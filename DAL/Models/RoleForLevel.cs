using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class RoleForLevel
    {
        public int Level { get; set; }
        public ulong Role { get; set; }
    }
}
