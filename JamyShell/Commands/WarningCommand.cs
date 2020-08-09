using Discord;
using Discord.Commands;
using JamyShell.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JamyShell.Commands
{
    public class WarningCommand : ModuleBase<SocketCommandContext>
    {
        public DataService _database { get; set; }

        [Command("warn")]
        [RequireUserPermission(Discord.GuildPermission.KickMembers, ErrorMessage = "Vous n'avez pas la permission d'utiliser cette commande!")]
        public async Task Warn(IGuildUser warnUser, [Remainder]string reason)
        {
            _ = _database.AddWarn(Context.User.Id, warnUser.Id, reason);
            _ = ReplyAsync(warnUser.Mention + " a été warn par " + Context.User.Mention + "pour la raison: " + reason);
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

    }
}
