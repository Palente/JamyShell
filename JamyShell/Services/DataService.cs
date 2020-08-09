using Discord.WebSocket;
using JamyShell.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamyShell.Services
{
    public class DataService
    {
        private DiscordSocketClient _client;
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _config;

        public DataService(IServiceProvider services)
        {
            _config = services.GetRequiredService<IConfigurationRoot>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<DataService>>();
        }

        #region WARN
        public async Task AddWarn(ulong authorId, ulong victimId, string reason)
        {
            using var db = new DataContext();
            await db.Warning.AddAsync(new Warning
            {
                AuthorId = authorId,
                VictimeId = victimId,
                Reason = reason,
                Created = new DateTime()
            });
            await db.SaveChangesAsync();

        }
        public bool RemoveWarn(ulong warnId)
        {
            using var db = new DataContext();
            var rs = db.Warning.AsEnumerable().Where(x => x.WarnId == warnId).FirstOrDefault();
            if (rs is null) return false;
            db.Warning.Remove(rs);
            db.SaveChangesAsync();
            return true;
        }
        public List<Warning> GetWarnings(ulong victimId)
        {
            using var db = new DataContext();
            var warnings = db.Warning
                   .AsEnumerable()
                   .Where(t => t.VictimeId == victimId)
                   .DefaultIfEmpty(null);
            return warnings.ToList();
        }
        #endregion
    }
}
