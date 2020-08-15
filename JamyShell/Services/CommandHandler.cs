using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JamyShell.Services
{
    class CommandHandler
    {
        private CommandService _commands;
        private DiscordSocketClient _client;
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _services;
        private readonly DataService _database;
        private readonly UtilsService _utils;

        public CommandHandler(IServiceProvider services)
        {
            _services = services;
            _config = services.GetRequiredService<IConfigurationRoot>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _commands = services.GetRequiredService<CommandService>();
            _client.MessageReceived += HandleCommand;
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            _database = services.GetRequiredService<DataService>();
            _utils = services.GetRequiredService<UtilsService>();
        }

        public async Task HandleCommand(SocketMessage parameterMessage)
        {
            var message = parameterMessage as SocketUserMessage;
            if (message == null) return;

            // Don't listen to bots
            if (message.Source != MessageSource.User)
            {
                return;
            }

            // Mark where the prefix ends and the command begins
            int argPos = 0;

            // Create a Command Context
            var context = new SocketCommandContext(_client, message);

            string prefix = _config["Prefix"];
            var serverPrefix = _config["Prefix"];

            if (serverPrefix != null)
            {
                prefix = serverPrefix;
            }

            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasStringPrefix(prefix, ref argPos))) return;
            var result = await _commands.ExecuteAsync(context, argPos, _services);

            await LogCommandUsage(context, result);
            // If the command failed, notify the user
            if (!result.IsSuccess)
            {
                if (result.ErrorReason != "Unknown command.")
                {
                    await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
                }
            }
        }

        private async Task LogCommandUsage(SocketCommandContext context, IResult result)
        {
            await Task.Run(() =>
            {
                if (context.Channel is IGuildChannel)
                {
                    var logTxt = $"User: [{context.User.Username}]<->[{context.User.Id}] Discord Server: [{context.Guild.Name}] -> [{context.Message.Content}]";
                    _logger.LogInformation(logTxt);
                }
                else
                {
                    var logTxt = $"User: [{context.User.Username}]<->[{context.User.Id}] -> [{context.Message.Content}]";
                    _logger.LogInformation(logTxt);
                }
            });
        }
    }
}
