using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JamyShell.Services
{
    class GuildService
    {
        private DiscordSocketClient _client;
        private DataService _database;
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _config;

        public GuildService(IServiceProvider services)
        {
            _config = services.GetRequiredService<IConfigurationRoot>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<GuildService>>();
            _database = services.GetRequiredService<DataService>();
        }
    }
}
