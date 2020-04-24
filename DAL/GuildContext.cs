using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoBot.Models
{
    public class GuildContext : DbContext
    {
        string connectionString = null;

        public GuildContext(DbContextOptions opts) : base(opts) { }
        public GuildContext(string conn) => connectionString = conn;
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if(connectionString != null) options.UseMySql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildData>()
                .HasMany(g => g.Logs)
                .WithOne(l => l.GuildData)
                .IsRequired();
        }


        public DbSet<GuildData> Guilds { get; set; }
        public DbSet<GuildLog> Logs { get; set; }
    }
}
