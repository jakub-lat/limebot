﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using LimeBot.Bot.Utils;
using DSharpPlus.Lavalink;
using LimeBot.Bot.Music;
using LimeBot.Bot.Commands;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using LimeBot.DAL;
using Microsoft.Extensions.Logging;

namespace LimeBot.Bot
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

        public BotService(IServiceCollection services)
        {
            if(instance == null)
            {
                instance = this;
            } else
            {
                throw new Exception("Bot already running!");
            }
            this.services = services;

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = Config.settings.BotToken,
                TokenType = TokenType.Bot,
                MinimumLogLevel = LogLevel.Warning,
                MessageCacheSize = 32,
                Intents = DiscordIntents.DirectMessages | DiscordIntents.GuildMessages | DiscordIntents.Guilds | DiscordIntents.GuildVoiceStates | DiscordIntents.GuildMessageReactions
            });

            events = new BotEvents(discord);

            discord.UseLavalink();

            lava = new Lavalink(discord);
            services.AddSingleton(lava);

            var provider = services.BuildServiceProvider();
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                CaseSensitive = false,
                PrefixResolver = ResolvePrefixAsync,
                Services = provider,
                EnableMentionPrefix = true,
                EnableDefaultHelp = false
            });

            commands.CommandErrored += events.CommandErrored;

            commands.RegisterCommands<ModerationCommands>();
            commands.RegisterCommands<MusicCommands>();
            commands.RegisterCommands<RankingCommands>();
            commands.RegisterCommands<ReactionRoleCommands>();
            commands.RegisterCommands<SystemCommands>();
            commands.RegisterCommands<FunCommands>();


            interactivity = discord.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(5)
            });

            discord.ConnectAsync();

            commandList = new CommandList(commands);

            discord.Ready += Ready;
            discord.MessageCreated += events.MessageCreated;
            discord.GuildMemberAdded += events.MemberJoined;
            discord.GuildMemberRemoved += events.MemberLeft;
            discord.MessageUpdated += events.MessageEdited;
            discord.MessageDeleted += events.MessageDeleted;
            discord.VoiceStateUpdated += lava.VoiceStateUpdated;
            discord.MessageReactionAdded += events.MessageReactionAdd;
            discord.MessageReactionRemoved += events.MessageReactionRemove;
        }

        private Task Ready(DiscordClient _, ReadyEventArgs e)
        {
            Console.WriteLine("LimeBot ready");
            return Task.CompletedTask;
        }

        private async Task<int> ResolvePrefixAsync(DiscordMessage msg)
        {
            string pfx = "";

            var guild = msg.Channel.Guild;
            if (guild == null) return msg.GetStringPrefixLength(Config.settings.DefaultPrefix);

            using var ctx = new GuildContext();
            var data = await ctx.GetGuild(guild.Id);
            pfx = Config.IsDevelopment || string.IsNullOrWhiteSpace(data?.Prefix) ? Config.settings.DefaultPrefix : data.Prefix;

            if (msg.MentionedUsers.Any(i => i.Id == discord.CurrentUser.Id))
            {
                _ = msg.RespondAsync($"Hey! My prefix here is `{pfx}` - type `{pfx}help` if you are stuck.");
            }

            return msg.GetStringPrefixLength(pfx);

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
