using LimeBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LimeBot.DAL
{
    public class GuildContext : DbContext
    {
        public GuildContext(DbContextOptions opts) : base(opts) { }
        public GuildContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Config.settings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<GuildData>()
                .Property(e => e.AutoRolesOnJoin)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList());
            */

            /*modelBuilder.Entity<GuildData>()
                .HasMany(g => g.Logs)
                .WithOne(l => l.GuildData)
                .IsRequired();*/
        }


        public DbSet<GuildData> Guilds { get; set; }
        public DbSet<MutedUser> MutedUsers { get; set; }
        public DbSet<Warn> Warns { get; set; }
        public DbSet<ReactionRole> ReactionRoles { get; set; }
        public DbSet<GuildMember> Members { get; set; }
    }
}
