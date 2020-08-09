using Discord;
using Discord.Commands;
using Discord.WebSocket;
using JamyShell.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JamyShell.Commands
{
    public class MuteCommand : Command
    {
        [Command("mute")]
        [RequireBotPermission(ChannelPermission.ManageRoles)]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task Mute(IGuildUser user, int time, [Remainder] string reason = "Sans raison")
        {
            IRole mutedRole = null;
            foreach (IRole role in Context.Guild.Roles)
            {
                if (role.Name == "Muted") mutedRole = role;
            }
            if (!(mutedRole is IRole))
            {
                await ReplyAsync(":x:"+Context.User.Mention+", Le role 'Muted' est introuvable!");
                return;
            }
            var test = new DemuteTask
            {
                Context = this.Context,
                Time = time,
                Victime = user,
                LogsChannel = Main.GetLogChannel()
            };
            Thread task = new Thread(start: new ThreadStart(test.ExecuteCode));
            task.Start();
            await user.AddRoleAsync(mutedRole);
            var embed = new EmbedBuilder()
                .WithColor(Color.DarkRed)
                .WithTitle(":hammer_pick: Un utilisateur vient d'être rendu muet!")
                .WithDescription("**Un utilisateur vient d'être rendu muet!**\n> *Utilisateur muet*:\n" + user.Mention + "(" + user.Username + "#" + user.Discriminator + ") || " + user.Id.ToString() + " ||\n> **Modérateur:**\n"+Context.User.Mention+"\n> **Raison: **\n"+reason+"\n> Durée de la Sanction:\n"+time+"mins")
                .WithCurrentTimestamp()
                .WithFooter("@Secure-Heberg")
                .Build();
            await this.Context.Channel.SendMessageAsync(embed: embed);
            var embedUser = new EmbedBuilder()
                .WithColor(Color.DarkRed)
                .WithUrl("https://discord.com/terms")
                .WithTitle(":hammer_pick: Vous ête désormais muet sur Secure-Heberg")
                .WithDescription("**Vous ête désormais muet sur Secure-Heberg**\n> **Modérateur:**\n" + Context.User.Mention + "\n> **Raison: **\n" + reason + "\n> Durée de la Sanction:\n" + time + "mins")
                .WithCurrentTimestamp()
                .WithFooter("@Secure-Heberg")
                .Build();
            await user.SendMessageAsync(embed: embedUser);
            var embedLogs = new EmbedBuilder()
                .WithColor(Color.DarkRed)
                .WithTitle(":hammer_pick: Un utilisateur vient d'être rendu muet!")
                .WithDescription("**Un utilisateur vient d'être rendu muet!**\n> *Utilisateur muet*:\n" + user.Mention + "(" + user.Username + "#" + user.Discriminator + ") || " + user.Id.ToString() + " ||\n> **Modérateur:**\n" + Context.User.Mention + "\n> **Raison: **\n" + reason + "\n> Durée de la Sanction:\n" + time + "mins")
                .WithCurrentTimestamp()
                .WithFooter("@Secure-Heberg")
                .Build();
            await Main.GetLogChannel().SendMessageAsync(embed: embed);
        }
        public override string GetDescription()
        {
            throw new NotImplementedException();
        }

        public override int GetCoolDown()
        {
            throw new NotImplementedException();
        }
    }
    public class DemuteTask
    {
        public SocketCommandContext Context;
        public int Time;
        public IGuildUser Victime;
        public ISocketMessageChannel LogsChannel;

        internal void ExecuteCode()
        {
            Thread.Sleep(60000 * Time);
            IRole mutedRole = null;
            foreach (IRole role in Context.Guild.Roles)
            {
                if (role.Name == "Muted") mutedRole = role;
            }
            if (!(mutedRole is IRole))
            {
                Context.Channel.SendMessageAsync(Context.Message.Author.Mention + " Role 'Muted' is not available!");
                return;
            }
            Victime.RemoveRoleAsync(mutedRole);
            var embedUser = new EmbedBuilder()
                .WithColor(Color.DarkGreen)
                .WithUrl("https://discord.com/terms")
                .WithTitle(":hammer_pick: Vous n'êtes plus Muet!")
                .WithDescription("**Votre sanction à été levé sur Secure-Heberg!**")
                .WithCurrentTimestamp()
                .WithFooter("@Secure-Heberg")
                .Build();
            var embedLogs = new EmbedBuilder()
                .WithColor(Color.DarkGreen)
                .WithUrl("https://discord.com/terms")
                .WithTitle(":hammer_pick: La sanction d'un utilisateur a été levé!")
                .WithDescription("La sanction de "+Victime.Mention+" vient d'être levé!")
                .WithCurrentTimestamp()
                .WithFooter("@Secure-Heberg")
                .Build();
            LogsChannel.SendMessageAsync(embed: embedLogs);
            Victime.SendMessageAsync(embed: embedUser);
        }
    }
}
