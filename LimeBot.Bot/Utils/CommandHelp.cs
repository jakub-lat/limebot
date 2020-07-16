using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimeBot.DAL;

namespace LimeBot.Bot.Utils
{
    public static class CommandHelp
    {
        public static async Task SendCommandHelp(CommandContext ctx, Command cmd, bool invalidUsage = false)
        {
            var desc = new StringBuilder();
            desc.AppendLine("**Description:**").AppendLine(cmd.Description).AppendLine();
            if (cmd.Aliases.Any()) desc.AppendLine($"**Aliases:** `{string.Join(", ", cmd.Aliases)}`").AppendLine();

            desc.AppendLine("**Usage:**")
                .AppendLine($"```{string.Join("\n", cmd.Overloads.Select(o => $"{ctx.Prefix}{cmd.QualifiedName} {string.Join(" ", o.Arguments.Select(a => string.Format(a.IsOptional ? "[{0}]" : "<{0}>", a.Name)))}"))}```");
            var embed = new DiscordEmbedBuilder
            {
                Title = invalidUsage ? $"Proper usage of {cmd.Name}" : $"Command: {cmd.Name}",
                Color = new DiscordColor(Config.settings.embedColor),
                Description = desc.ToString()
            };
            await ctx.RespondAsync(embed: embed);
        }
    }
}
