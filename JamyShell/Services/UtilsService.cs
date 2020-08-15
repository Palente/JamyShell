using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JamyShell.Services
{
    public class UtilsService
    {
        private DiscordSocketClient _client;
        private readonly IConfigurationRoot _config;

        public UtilsService(IServiceProvider services)
        {
            _config = services.GetRequiredService<IConfigurationRoot>();
            _client = services.GetRequiredService<DiscordSocketClient>();
        }
        /// <summary>
        /// Obtenir le Guild Secure Heberg
        /// </summary>
        /// <returns>Le Guild Secure-Heberg</returns>
        public SocketGuild GetGuild()
        {
            return _client.GetGuild(385875094955884544);
        }
        /// <summary>
        /// Obtenir le salon des Logs
        /// </summary>
        /// <returns>Le salon des Logs</returns>
        public ITextChannel GetLogChannel()
        {
            return (ITextChannel)GetGuild().GetChannel(542832493028704266);
        }
    }
}
