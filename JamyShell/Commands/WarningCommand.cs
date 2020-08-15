using Discord;
using Discord.Commands;
using Discord.WebSocket;
using JamyShell.Services;
using System.Threading.Tasks;

namespace JamyShell.Commands
{
    public class WarningCommand : ModuleBase<SocketCommandContext>
    {
        public DataService _database { get; set; }

        [Command("warn")]
        [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "Vous n'avez pas la permission d'utiliser cette commande!")]
        public async Task Warn(IGuildUser warnUser, [Remainder]string reason)
        {
            await _database.AddWarn(Context.User.Id, warnUser.Id, reason);
            await ReplyAsync(warnUser.Mention + " a été warn par " + Context.User.Mention + " pour la raison: " + reason);
            //Envoyer le warn!
            var embedWarned = new EmbedBuilder()
                .WithColor(Color.DarkRed)
                .WithTitle("Vous venez de recevoir un warn!")
                .WithUrl("https://secure-heberg.com")
                .WithAuthor(Context.User)
                .WithDescription("[Merci de respecter les règles de notre Discord!](https://discord.com/channels/385875094955884544/606231835340439553)")
                .AddField($"Vous venez de recevoir de recevoir un avertissement de {Context.User.Username + "#" +Context.User.Discriminator}", $"> **Raison: **\n __{reason}__")
                .WithFooter("@Secure-Heberg")
                .Build();
            _ = warnUser.SendMessageAsync(embed: embedWarned);
            //Afficher le warn dans logs
            var warnLogs = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithTitle("Un membre vient de recevoir un avertissement!")
                .WithUrl("https://secure-heberg.com")
                .WithAuthor(Context.User)
                .WithDescription($"{warnUser.Mention} || {warnUser.Username+"#"+warnUser.Discriminator} || {warnUser.Id} vient de recevoir un avertissement!")
                .AddField($"{Context.User.Mention} a donné un avertissement à {warnUser.Username + "#" + warnUser.Discriminator}", $"> **Raison: **\n __{reason}__")
                .WithFooter("@Secure-Heberg")
                .Build();
            _ = GetLogChannel().SendMessageAsync(embed: warnLogs);
        }
        [Command("seewarn")]
        public Task SeeWarn(IGuildUser user)
        {
            var warns = _database.GetWarnings(user.Id);
            if(warns is null)
            {
                ReplyAsync("Cet utilisateur n'a jamais été warn!");
                return Task.CompletedTask;
            }
            var rep = "";
            warns.ForEach(x => 
            {
                if (x != null)
                {
                    rep += "WarnID: " + x.WarnId + "\nSenceur: " + x.AuthorId + "\nRaison: " + x.Reason;
                }
            });
            if (rep == "") rep = "Aucun warn pour ce membre!";
            ReplyAsync("> Liste des warns:\n" + rep);
            return Task.CompletedTask;
        }

        private ITextChannel GetLogChannel()
        {
            return (ITextChannel) Context.Guild.GetChannel(542832493028704266);
        }

    }
}
