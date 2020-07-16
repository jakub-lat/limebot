using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using LimeBot.Bot.Attributes;
using LimeBot.Bot.Utils;
using LimeBot.Bot.Utils.Reddit;
using LimeBot.DAL;

namespace LimeBot.Bot.Commands
{
    [Category("Fun")]
    class FunCommands : MyCommandModule
    {
        public FunCommands(GuildContext db) : base(db) { }

        [Command("meme"), Description("Random meme from reddit")]
        public async Task Meme(CommandContext ctx)
        {
            var msg = await ctx.RespondAsync("Fetching meme...");

            var s = new Random().Next(3) switch
            {
                1 => "historymemes",
                2 => "prequelmemes",
                _ => "memes"
            };

            var post = await RedditHelper.GetRandom(s);
            var embed = new DiscordEmbedBuilder
            {
                Title = post.title,
                ImageUrl = post.url,
                Color = new DiscordColor("#daef39"),
                Timestamp = DateTime.UtcNow,
            }.WithFooter($"Posted by u/{post.author} in r/{s}");

            await msg.DeleteAsync();
            await ctx.RespondAsync(embed: embed.Build());
        }

        [Command("dadjoke"), Aliases("pun"), Description("Random dadjoke from r/dadjokes")]
        public async Task Dadjoke(CommandContext ctx)
        {
            var msg = await ctx.RespondAsync("Fetching dadjoke...");
            var post = await RedditHelper.GetRandom("dadjokes");
            var embed = new DiscordEmbedBuilder
            {
                Title = post.title,
                ImageUrl = post.url,
                Description = post.selftext,
                Color = new DiscordColor("#daef39"),
                Timestamp = DateTime.UtcNow
            }.WithFooter($"Posted by u/{post.author} in r/dadjokes");

            await msg.DeleteAsync();
            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
