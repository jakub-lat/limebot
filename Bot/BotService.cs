using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PotatoBot.Bot.Commands;
using DSharpPlus.Entities;
using PotatoBot.Models;
using PotatoBot.Bot.Utils;
using Microsoft.Extensions.Configuration;
using PotatoBot;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Bot.Utils;
using Bot;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using Bot.Music;
using PotatoBot.Utils;


namespace PotatoBot.Bot
{
    public class BotService
    {
        public static BotService instance;

        public DiscordClient discord;
        CommandsNextExtension commands;
        InteractivityExtension interactivity;

        CommandList commandList;
        Lavalink lava;

        IServiceCollection services;

        BotEvents events;

        readonly string prefix = "$";
        readonly string dev_prefix = ".";

        string connectionString;

        public BotService(IServiceCollection services, string connectionString)
        {
            if(instance == null)
            {
                instance = this;
            } else
            {
                throw new Exception("Bot already running!");
            }
            this.services = services;
            this.connectionString = connectionString;


            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = Config.settings.BotToken,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            discord.UseLavalink();

            services.AddSingleton(new Lavalink(discord));

            var provider = services.BuildServiceProvider();
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                CaseSensitive = false,
                PrefixResolver = ResolvePrefixAsync,
                Services = provider,
                EnableMentionPrefix = true,
                EnableDefaultHelp = false
            });

            commands.RegisterCommands<FunCommands>();
            commands.RegisterCommands<SystemCommands>();
            commands.RegisterCommands<ModerationCommands>();
            commands.RegisterCommands<MusicCommands>();

            interactivity = discord.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(10)
            });

            discord.ConnectAsync();

            commandList = new CommandList(commands);

            events = new BotEvents(discord, connectionString);
            discord.GuildMemberAdded += events.MemberJoined;
            discord.GuildMemberRemoved += events.MemberLeft;
            discord.MessageUpdated += events.MessageEdited;
            discord.MessageDeleted += events.MessageDeleted;
        }

        private async Task<int> ResolvePrefixAsync(DiscordMessage msg)
        {
            if(Config.IsDevelopment)
            {
                return msg.GetStringPrefixLength(dev_prefix);
            }
            var guild = msg.Channel.Guild;
            if (guild == null) return -1;

            using(var ctx = new GuildContext(connectionString))
            {
                var data = await ctx.GetGuild(guild.Id);
                var pfx = data?.Prefix ?? prefix;

                if (msg.MentionedUsers.Any(i => i.Id == discord.CurrentUser.Id))
                {
                    _ = msg.RespondAsync($"Hey! My prefix here is `{pfx}`. Type `{pfx}help` if you are stuck.");
                }

                var prefixLocation = msg.GetStringPrefixLength(pfx);
                return prefixLocation;
            }
        }

        public bool IsOnGuild(ulong id)
        {
            return discord.Guilds.ContainsKey(id);
        }

        public IReadOnlyDictionary<string, List<CommandData>> GetCommands()
        {
            return commandList.list;
        }
    }
}