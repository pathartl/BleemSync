using BleemSync.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BleemSync.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=BleemSync Library.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many to many relationship between collections and games
            // This is many to many because a game might exist under multiple collections
            // Join tables are currently a limitation of Entity Framework Core:
            // https://github.com/dotnet/efcore/issues/10508
            modelBuilder.Entity<CollectionGame>()
                .HasKey(cg => new { cg.CollectionId, cg.GameId });
            modelBuilder.Entity<CollectionGame>()
                .HasOne(cg => cg.Game)
                .WithMany(g => g.CollectionGames)
                .HasForeignKey(g => g.GameId);
            modelBuilder.Entity<CollectionGame>()
                .HasOne(cg => cg.Collection)
                .WithMany(c => c.CollectionGames)
                .HasForeignKey(c => c.CollectionId);
        }
    }
}