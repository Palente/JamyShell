using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using JamyShell;
using JamyShell.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace JamyShell
{
    public class Program
    {
        private static Program instance;
        public Logs Logs;
        static void Main() => new Program().RunBotAsync().GetAwaiter().GetResult();
        private DiscordSocketClient _client;
        private CommandService _commands;
        private DiscordSocketConfig _config;
        public SocketGuild Guild;
        public Dictionary<ulong, List<RestInviteMetadata>> InvitationsUser = new Dictionary<ulong, List<RestInviteMetadata>>();
        public Dictionary<ulong, int> Invitation = new Dictionary<ulong, int>();
        private IServiceProvider _services;
        private Config Config = new Config();
        public async Task RunBotAsync()
        {
            instance = this;
            _config = new DiscordSocketConfig { MessageCacheSize = 100};
            _client = new DiscordSocketClient(_config);
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            string token = "Njk0NTIzNzc5Nzg2NzM1Njk3.XrBHfg.Ek10f1p6ji3rI2M4pRktGPE1Xp8";
            Logs = new Logs();
            _client.Log += Logs.DisplayLogs;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
            Command.RegisterCommands();
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _client.MessageDeleted += OnMessageDeleted;
            _client.Ready += OnReady;
            _client.UserJoined += OnUserJoined;
            _client.LoggedOut += onLoggedOut;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private Task onLoggedOut()
        {
            Config.Close();
            return Task.CompletedTask;
        }

        private async Task OnUserJoined(SocketGuildUser newUser)
        {
            Logs.Info(newUser.Username + " just joined, now we are " + newUser.Guild.MemberCount + " members");
            Dictionary<ulong, int> oldInvite = Invitation;
            await MiseAJourInvite();
            SocketGuildUser user = null;
            
            foreach (KeyValuePair<ulong, int> inv in oldInvite)
            {
                if (Invitation[inv.Key] == inv.Value)
                {
                    Logs.Debug(inv.Key + " checked;");
                    continue;
                }
                else
                {
                    Logs.Debug("FOUND " + inv.Key);
                    user = Guild.GetUser(inv.Key);
                    break;
                }
            }
            Logs.Debug(newUser.Username+" à été invité par "+(!(user is null) ? user.Username: "UNKNOWN"));
        }

        private Task OnReady()
        {
            // ALL FUNCTION THAT MUST BE USED AFTER SETUP GO THERE!
            Logs.Info("Bot is now ready, trying to catch invites...");
            Guild = _client.GetGuild(385875094955884544);
            var test = new DiscordInvitation
            {
                Instance = this
            };
            Thread task = new Thread(start: new ThreadStart(test.ExecuteCode));
            task.Start();
            Config.TestInser();
            Logs.Info("Field Count: " +Config.TestGet().FieldCount);
            var rslt = Config.TestGet();
            /*
            while (rslt.Read())
            {
                Logs.Debug("SQL RESULT: " + rslt.GetString(3));
            }*/
            rslt.Read();
            Logs.Debug(rslt.GetString(3));
            return Task.CompletedTask;
        }

        private Task OnMessageDeleted(Cacheable<IMessage, ulong> msg, ISocketMessageChannel value)
        {
            Logs.Debug((msg.HasValue?msg.Value.Content:"Le Message supprimé ne rentre pas dans le cache!"));
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;
            int argPos = 0;
            if(message.HasStringPrefix("/", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                Console.WriteLine(message.Content);
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }
        public static Program GetInstance()
        {
            return instance;
        }
        public Logs GetLogs()
        {
            return Logs;
        }
        public ISocketMessageChannel GetLogChannel()
        {
            return _client.GetGuild(385875094955884544).GetChannel(542832493028704266) as ISocketMessageChannel;
        }
        public async Task MiseAJourInvite()
        {
            bool init = true;
            var inv = await Guild.GetInvitesAsync();
            foreach (RestInviteMetadata invite in inv)
            {
                if (Invitation.ContainsKey(invite.Inviter.Id))
                {
                    if (init)
                    {
                        Invitation[invite.Inviter.Id] = invite.Uses ?? 0;
                        init = false;
                    }
                    else Invitation[invite.Inviter.Id] += invite.Uses ?? 0;
                }
                else Invitation[invite.Inviter.Id] = (int)invite.Uses;
                if (!InvitationsUser.ContainsKey(invite.Inviter.Id)) InvitationsUser[invite.Inviter.Id] = new List<RestInviteMetadata>();
                InvitationsUser[invite.Inviter.Id].Add(invite);
                //Logs.Debug("(" + invite.Inviter.Username + ") " + invite.Url+ " => "+invite.Uses);
            }
            Logs.Debug("Adel a " + Invitation[260831072831406090] + " invités!");
        }
    }
}
public class DiscordInvitation
{
    public Program Instance;
    internal  void ExecuteCode()
    {
        _ = Instance.MiseAJourInvite();
    }
}
