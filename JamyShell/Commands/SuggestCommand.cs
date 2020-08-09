using Discord.Commands;
using JamyShell.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JamyShell.Commands
{
    public class SuggestCommand : Command
    {
        private readonly int CooldownTime = 3 * 60; //Time in Second
        private Dictionary<ulong, DateTime> _cooldown = new Dictionary<ulong, DateTime>();
        private List<string> _suggesteds = new List<string>();
        [Command("suggest")]
        public async Task Suggest([Remainder] string text)
        {
            Logs.Debug("Suggestion: "+_suggesteds.Contains(text.ToLower()));
            Logs.Debug(Logs.Var_Dump(_suggesteds, 0));
            if (_suggesteds.Contains(text.ToLower())){
                await ReplyAsync(":x: " + Context.User.Mention + ", Cette suggestion à déjà été faites récemment!");
                return;
            }
            if(_cooldown.ContainsKey(Context.User.Id))
            {
                Logs.Debug("Contain keys!");
                Logs.Debug(_cooldown[Context.User.Id].AddSeconds(CooldownTime) + " is < " + DateTime.UtcNow);
                //Verif heure
                if(_cooldown[Context.User.Id].AddSeconds(CooldownTime) < DateTime.UtcNow)
                {
                    Logs.Debug("COOLDOWNED");
                    await ReplyAsync(":x: " + Context.User.Mention + ", Cette commande possède un cooldown de 3 minutes");
                    return;
                }
                _cooldown[Context.User.Id] = DateTime.UtcNow;
            }
            else
            {
                Logs.Debug("Don't contain keys!");
                _cooldown.Add(Context.User.Id, DateTime.UtcNow);
            }
            _suggesteds.Add(text.ToLower());
            Logs.Debug("cooldown: " + _cooldown[Context.User.Id]);
            await ReplyAsync("sent");
            Logs.Debug(Logs.Var_Dump(_cooldown, 0));
            //TODO: Finish The command
        }
        public override string GetDescription()
        {
            throw new NotImplementedException();
        }

        public override int GetCoolDown()
        {
            return 3;
        }
    }
}
