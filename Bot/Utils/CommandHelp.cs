using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using PotatoBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Utils
{
    public static class CommandHelp
    {
        public static async Task SendCommandHelp(CommandContext ctx, Command cmd)
        {
            var desc = new StringBuilder();
            desc.AppendLine("**Description:**").AppendLine(cmd.Description).AppendLine();
            if (cmd.Aliases.Any()) desc.AppendLine($"**Aliases:** `{string.Join(", ", cmd.Aliases)}`").AppendLine();

            desc.AppendLine("**Usage:**")
                .AppendLine($"```{string.Join("\n", cmd.Overloads.Select(o => $"{ctx.Prefix}{cmd.QualifiedName} {string.Join(" ", o.Arguments.Select(a => string.Format(a.IsOptional ? "[{0}]" : "<{0}>", a.Name)))}"))}```");
            var embed = new DiscordEmbedBuilder
            {
                Title = $"Command: {cmd.Name}",
                Color = new DiscordColor(Config.settings.embedColor),
                Description = desc.ToString()
            };
            await ctx.RespondAsync(embed: embed);
        }
    }
}
