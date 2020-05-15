using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.EventHandling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Entities
{
    /// <summary>
    /// An example implementation of the IPaginationRequest interface.
    /// Take a look at the IPaginationRequest interface in DSharpPlus.Interactivity for more information.
    /// </summary>
    public class MyPaginatedMessage : IPaginationRequest
    {
        private List<Page> pages;

        private TaskCompletionSource<bool> _tcs;
        private CancellationTokenSource _cts;

        private DiscordMessage msg;
        private int index = 0;
        private PaginationEmojis emojis;

        private DiscordUser user;
        private DiscordChannel channel;

        public MyPaginatedMessage(CommandContext ctx, List<Page> pages, PaginationEmojis emojis = null, int index = 0)
        {
            this.pages = pages;
            _tcs = new TaskCompletionSource<bool>();
            _cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            _cts.Token.Register(() => _tcs.TrySetResult(true));
            this.emojis = emojis != null ? emojis : new PaginationEmojis {
                SkipLeft = DiscordEmoji.FromUnicode("⏪"),
                SkipRight = DiscordEmoji.FromUnicode("⏩"),
                Left = DiscordEmoji.FromUnicode("⬅"),
                Right = DiscordEmoji.FromUnicode("➡️"),
                Stop = DiscordEmoji.FromUnicode("🛑")
            };
            this.index = index;
            user = ctx.Member;
            channel = ctx.Channel;
        }

        public async Task SendAsync()
        {
            msg = await channel.SendMessageAsync(embed: pages[index].Embed);
        }

        public async Task DoCleanupAsync()
        {
            await msg.DeleteAllReactionsAsync();
        }

        public async Task<PaginationEmojis> GetEmojisAsync()
        {
            await Task.Yield();
            return emojis;
        }

        public async Task<DiscordMessage> GetMessageAsync()
        {
            await Task.Yield();
            return msg;
        }

        public async Task<Page> GetPageAsync()
        {
            await Task.Yield();
            return pages[index];
        }

        public async Task<TaskCompletionSource<bool>> GetTaskCompletionSourceAsync()
        {
            await Task.Yield();
            return _tcs;
        }

        public async Task<DiscordUser> GetUserAsync()
        {
            await Task.Yield();
            return user;
        }

        public async Task NextPageAsync()
        {
            await Task.Yield();

            if (index < pages.Count - 1)
                index++;
        }

        public async Task PreviousPageAsync()
        {
            await Task.Yield();

            if (index > 0)
                index--;
        }

        public async Task SkipLeftAsync()
        {
            await Task.Yield();

            index = 0;
        }

        public async Task SkipRightAsync()
        {
            await Task.Yield();

            index = pages.Count - 1;
        }
    }
}