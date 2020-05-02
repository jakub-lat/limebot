using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace TestBot
{
    // testowy bot do testów d#+ / komend
    class Program
    {
        static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NzAzMjkwMjM1NDM3NDQ5MzE3.XqMfYA.csv4tl3N8hTM8aorOI4HiVX9I5I",
                TokenType = TokenType.Bot
            });

            var cnext = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "_" }
            });

            cnext.RegisterCommands<TestCommands>();

            await discord.ConnectAsync();
            discord.Ready += async (ReadyEventArgs e) => Console.WriteLine("Ready");
            await Task.Delay(-1);
        }
    }
}
