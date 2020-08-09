using Discord.Commands;
using JamyShell.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace JamyShell.Modules
{
    public abstract class Command : ModuleBase<SocketCommandContext>
    {
        public Program Main => Program.GetInstance();
        public Logs Logs => Main.GetLogs();
        public static List<Command> Commands;
        public abstract string GetDescription();
        public abstract int GetCoolDown();
        public static void RegisterCommands()
        {
            Commands.Add(new PoggitCommand());
            Commands.Add(new PurgeCommand());
        }
    }
}
