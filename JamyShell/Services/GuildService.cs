using Discord;
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
            _client.UserJoined += OnUserJoin;
            _client.UserLeft += OnUserLeft;
        }

        public Task OnUserLeft(SocketGuildUser member)
        {
            return Task.CompletedTask;
        }

        public Task OnUserJoin(SocketGuildUser member)
        {
            var embedChannel = new EmbedBuilder()
                .WithTitle($"{member.Username} vient de rejoindre Secure-Heberg")
                .WithUrl("https://secure-heberg.com")
                .WithColor(RandomColor())
                .WithThumbnailUrl(member.GetAvatarUrl())
                .AddField("Nom: ",$"{member.Mention} || {member.Username}#{member.Discriminator}")
                .AddField("Nombre de membres:", $"{member.Guild.MemberCount} Membres!")
                .WithFooter("@Secure-Heberg")
                .WithCurrentTimestamp()
                .Build();
            GetChannel(member).SendMessageAsync(embed: embedChannel);
            var embedMp = new EmbedBuilder()
                .WithTitle("Bienvenue sur le discord de Secure Heberg")
                .Build();
            member.SendMessageAsync(embed: embedMp);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Permet d'obtenir Le salon où sont annoncé les nouveaux arrivants.
        /// </summary>
        /// <param name="member">Membre d'un guild</param>
        /// <returns>Salon Des arrivants</returns>
        private ITextChannel GetChannel(SocketGuildUser member)
        {
            return (ITextChannel) member.Guild.GetChannel(463753111421321228);
        }
        /// <summary>
        /// Permet d'obtenir une couleur au hazard.
        /// </summary>
        /// <returns>Random Color</returns>
        private Color RandomColor()
        {
            var rnd = new Random();
            return new Color(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
        }
    }
}
