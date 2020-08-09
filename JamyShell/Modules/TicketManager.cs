using System;
using System.Collections.Generic;
using System.Text;
using Discord;
namespace JamyShell.Modules
{
    class TicketManager
    {
        private IChannel Channel;
        private ulong AuthorId;
        private int TicketId;
        private int CreatedAt;
        private string Subject;
        public TicketManager(IChannel channel)
        {
            Channel = channel;

        }
        public TicketManager(int id)
        {
            TicketId = id;
            //TODO: 
        }
        public TicketManager(ulong author)
        {
            AuthorId = author;
            //TODO: 
        }
    }
}
