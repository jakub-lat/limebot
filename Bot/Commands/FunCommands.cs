using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using PotatoBot.Bot.Utils;
using System.Threading.Tasks;
using PotatoBot.Models;
using Bot.Attributes;
using PotatoBot.Utils;


namespace PotatoBot.Bot.Commands
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

        [Command("reddit"), Description("Random post from specified subreddit")]
        public async Task Reddit(CommandContext ctx, string subreddit)
        {
            var msg = await ctx.RespondAsync("Loading...");
            try
            {
                var post = await RedditHelper.GetRandom(subreddit);

                var embed = new DiscordEmbedBuilder
                {
                    Title = post.title,
                    ImageUrl = post.url,
                    Description = post.selftext,
                    Color = new DiscordColor("#daef39"),
                    Timestamp = DateTime.UtcNow
                }.WithFooter($"Posted by u/{post.author} in r/{subreddit}");

                await msg.DeleteAsync();
                await ctx.RespondAsync(embed: embed.Build());
            } catch (SubredditNotFoundException)
            {
                await ctx.RespondAsync($"Subreddit `r/{subreddit}` not found!");
            } catch (Exception e)
            {
                await ctx.RespondAsync($"An unexpected error of type `{e.GetType().Name}` was thrown! Details: ```{e.Message}```");
            } finally
            {
                await msg.DeleteAsync();
            }

        }
    }
}
