using Discord;
using Discord.Commands;
using JamyShell.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JamyShell.Commands
{
    public class KickCommand : ModuleBase<SocketCommandContext>
    {
        public UtilsService _utils { get; set; }
        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers,ErrorMessage = "Utilisation: /kick <@mention> Raison")]
        public async Task Kick(IGuildUser member, [Remainder]string reason="No reason given")
        {
            var embedPM = new EmbedBuilder()
                .Build();
            await member.SendMessageAsync(embed: embedPM);
            var embedReply = new EmbedBuilder()
                .Build();
            _ = ReplyAsync(embed: embedReply);
            var logsEmbed = new EmbedBuilder()
                .Build();
            _ = _utils.GetLogChannel().SendMessageAsync(embed: logsEmbed);
            _ = member.KickAsync($"Kicked By {Context.User.Username} For: {reason}");
        }
    }
}
