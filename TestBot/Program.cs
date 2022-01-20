using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;

namespace TestBot
{
    // testowy bot do testów d#+ / komend
    internal static class Program
    {
        static DiscordClient _discord;

        static void Main()
        {
            MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "token",
                TokenType = TokenType.Bot
            });

            var cnext = _discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "_" }
            });

            cnext.RegisterCommands<TestCommands>();

            await _discord.ConnectAsync();
            _discord.Ready += async (c, _) => Console.WriteLine("Ready");
            await Task.Delay(-1);
        }
    }
}
