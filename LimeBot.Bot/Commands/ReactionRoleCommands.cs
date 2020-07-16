using LimeBot.DAL.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using LimeBot.Bot.Attributes;
using LimeBot.Bot.Utils;
using LimeBot.DAL;

namespace LimeBot.Bot.Commands
{
    [Group("rr"), Category("Reaction roles"), RequireGuild]
    public class ReactionRoleCommands : MyCommandModule
    {
        public ReactionRoleCommands(GuildContext db) : base(db) { }

        [GroupCommand, Command("help"), Description("Info about reaction roles")]
        public async Task Help(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Reaction roles help",
                Color = new DiscordColor(Config.settings.embedColor),
                Description = $@"**Creating** a new reaction role: `{ctx.Prefix}rr create <message link> <emoji> <role>` 
or `{ctx.Prefix}rr create <channel> <msg> <emoji> <role>`. 
To get message link or id, simply right click on a message, and select the required option.

**List** of reaction roles in this server: `{ctx.Prefix}rr list`

**Removing** a reaction role: `{ctx.Prefix}rr remove <index>`"
            };
            await ctx.RespondAsync(embed: embed);
        }

        [Command("create"), Description("Creates a reaction role for specified message and emoji"), RequirePermissions(DSharpPlus.Permissions.ManageRoles | DSharpPlus.Permissions.AddReactions)]
        public async Task Create(CommandContext ctx, DiscordMessage messageLink, DiscordEmoji emoji, DSharpPlus.Entities.DiscordRole role)
        {
            if(messageLink.Channel.GuildId != ctx.Guild.Id)
            {
                await ctx.RespondAsync("Invalid message");
                return;
            }
            if(role.Position > ctx.Guild.CurrentMember.Hierarchy)
            {
                await ctx.RespondAsync(":warning: I can't use that role - it's too high! Move the role below me, or use a different one.");
                return;
            } else if (role.Position > ctx.Member.Hierarchy)
            {
                await ctx.RespondAsync(":warning: The role is higher than you!");
                return;
            }

            var check = await db.ReactionRoles.Where(i => i.GuildId == ctx.Guild.Id && i.MessageId == messageLink.Id && i.Emoji == emoji.ToString()).FirstOrDefaultAsync();
            if(check != null)
            {
                await ctx.RespondAsync(":warning: This reaction role is already in use!");
                return;
            }
            
            try
            {
                await messageLink.CreateReactionAsync(emoji);
            } catch
            {
                await ctx.RespondAsync(":warning: I can't use that emoji! Try to specify a different one.");
                return;
            }

            db.ReactionRoles.Add(new ReactionRole
            {
                Emoji = emoji.ToString(),
                GuildId = ctx.Guild.Id,
                MessageId = messageLink.Id,
                MessageJumpLink = messageLink.JumpLink.ToString(),
                ChannelId = messageLink.ChannelId,
                RoleId = role.Id
            });
            await db.SaveChangesAsync();

            var embed = new DiscordEmbedBuilder
            {
                Title = "Successfully created a reaction role",
                Description = $@"The {emoji} emoji will now grant (or remove) **{role.Mention}** role.

*If you want to enable reaction role notifications, [click here and login to dashboard]({Config.settings.DashboardUrl}/manage/{ctx.Guild.Id}/reaction-roles).*",
                Color = new DiscordColor(Config.settings.embedColor)
            };
            await ctx.RespondAsync(embed: embed);
        }

        [Command("create")]
        public async Task CreateVariant(CommandContext ctx, DSharpPlus.Entities.DiscordChannel channel, ulong messageId, DiscordEmoji emoji, DSharpPlus.Entities.DiscordRole role)
        {
            var msg = await channel.GetMessageAsync(messageId);
            if(msg == null)
            {
                await ctx.RespondAsync(":warning: Message with specified ID was found!");
                return;
            }

            await Create(ctx, msg, emoji, role);
        }

        [Command("list"), Description("List all reaction roles for this server"), RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task List(CommandContext ctx)
        {
            var list = await db.ReactionRoles.Where(i => i.GuildId == ctx.Guild.Id).ToListAsync();

            var embed = new DiscordEmbedBuilder
            {
                Title = $"Reaction roles for {ctx.Guild.Name}",
                Color = new DiscordColor(Config.settings.embedColor),
                Description = list.Count == 0 
                ? $"There are no reaction roles! Type `{ctx.Prefix}rr help` to get started." 
                : string.Join("\n", list.Select((x, i) => $"{i+1}. [Message]({x.MessageJumpLink}): `{x.MessageId}`  |  Emoji: {x.Emoji}  |  Role: {ctx.Guild.GetRole(x.RoleId).Mention}"))
            };

            await ctx.RespondAsync(embed: embed);
        }

        [Command("remove"), Description("Remove a reaction role"), RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task Remove(CommandContext ctx, int index)
        {
            var list = await db.ReactionRoles.Where(i => i.GuildId == ctx.Guild.Id).ToListAsync();
            try
            {
                var item = list[index - 1];
                db.ReactionRoles.Remove(item);
                await db.SaveChangesAsync();

                try
                {
                    var msg = await (await ctx.Client.GetGuildAsync(item.GuildId)).GetChannel(item.ChannelId).GetMessageAsync(item.MessageId);
                    var emoji = (DiscordEmoji)await ctx.CommandsNext.ConvertArgument<DiscordEmoji>(item.Emoji, ctx);
                    await msg.DeleteOwnReactionAsync(emoji);
                }
                catch
                {
                    // ignored
                }


                var embed = new DiscordEmbedBuilder
                {
                    Title = "Reaction role removed",
                    Description = $"Removed reaction role for [message]({item.MessageJumpLink}) with id `{item.MessageId}` and emoji {item.Emoji}",
                    Color = new DiscordColor(Config.settings.embedColor)
                };
                await ctx.RespondAsync(embed: embed);
            } catch
            {
                await ctx.RespondAsync(":warning: This reaction role doesn't exist!");
            }
        }

        [Command("remove")]
        public async Task RemoveHelp(CommandContext ctx)
        {
            await ctx.RespondAsync($"Usage: `{ctx.Prefix}rr remove <index>`. To get all reaction roles (and indexes) type `{ctx.Prefix}rr list`");
        }
    }
}
