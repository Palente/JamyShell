using Discord;
using Discord.Commands;
using JamyShell.Modules;
using System.Threading.Tasks;

namespace JamyShell.Commands
{
    public class PurgeCommand : Command
    {
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("purge")]
        public async Task Purge(int amount)
        {
            if(amount> 100)
            {
                await ReplyAsync(":x: " + Context.User.Mention + ", vous ne pouvez supprimez plus de 100 message à la fois!");
                return;
            }
            var messages = await Context.Channel.GetMessagesAsync(amount+1).FlattenAsync();
            var channel = Context.Channel as ITextChannel;
            await channel.DeleteMessagesAsync(messages);
            var m = await ReplyAsync(Context.User.Mention+", "+amount+" messages ont bien été supprimés!");
            await Task.Delay(2000);
            await m.DeleteAsync();
        }
        public override string GetDescription()
        {
            return "Purge Command!";
        }

        public override int GetCoolDown()
        {
            throw new System.NotImplementedException();
        }
    }
}
