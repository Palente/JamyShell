using Discord;
using Discord.Commands;
using JamyShell.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JamyShell.Commands
{
    class InfoCommand : Command
    {
        [Command("info")]
        public Task Info()
        {
            var embed = new EmbedBuilder()
                .WithColor(100, 100, 200)
                .WithTitle(":credit_card: Carte D'identité de Jamy Shell")
                .WithUrl("https://secure-heberg.com/")
                .WithDescription("> Bonjour! Je suis votre assistant personnalisé Jamy!\nPour m'utilisez il suffit de dire \"Ok Jamy\"")
                .WithCurrentTimestamp()
                .WithFooter("@Secure-Heberg")
                .Build();
            this.Context.Channel.SendMessageAsync(embed: embed);
            return Task.CompletedTask;
        }
        public override int GetCoolDown()
        {
            return 60;
        }

        public override string GetDescription()
        {
            return "Obtenir la carte d'identité!";
        }
    }
}
