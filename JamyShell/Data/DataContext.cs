using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace JamyShell.Data
{
    class DataContext : DbContext
    {
        public DbSet<Warning> Warning { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=SecureHeberg.db");
        
    }
}
