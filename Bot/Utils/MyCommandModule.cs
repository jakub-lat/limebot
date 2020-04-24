using DSharpPlus.CommandsNext;
using PotatoBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotatoBot.Bot.Utils
{
    public class MyCommandModule : BaseCommandModule
    {
        protected GuildContext db;
        public MyCommandModule(GuildContext db)
        {
            this.db = db;
        }
    }
}
