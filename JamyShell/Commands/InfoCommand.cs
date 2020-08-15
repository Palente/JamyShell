using Discord;
using Discord.Commands;
using JamyShell.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace JamyShell.Commands
{
    public class InfoCommand : ModuleBase<SocketCommandContext>
    {
        public DataService _database { get; set; }

        [Command("info")]
        public Task Info()
        {
            var proc = Process.GetCurrentProcess();
            //TODO: Rendre fonctionnel le uptime
            var uptime = proc.TotalProcessorTime.Days + " jour(s) " + proc.TotalProcessorTime.Hours + " heure(s) et " + proc.TotalProcessorTime.Minutes + " minute(s)";
            var embed = new EmbedBuilder()
                .WithColor(100, 100, 200)
                .WithTitle(":credit_card: Ticket Manager!")
                .WithDescription("> Développé par [**Palente**](https://github.com/Palente) & [**KingDeadKnight**](https://github.com/KingDeadKnight)")
                .AddField("Ram utilisé: ", ((proc.PrivateMemorySize64 / 1024) / 1024) + " MB")
                .AddField("Temps d'éxecution:", uptime)
                .WithCurrentTimestamp()
                .WithFooter("@TicketManager")
                .Build();
            Context.Channel.SendMessageAsync(embed: embed);
            return Task.CompletedTask;
        }
    }
}
