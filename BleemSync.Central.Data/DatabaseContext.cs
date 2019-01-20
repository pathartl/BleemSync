using BleemSync.Central.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BleemSync.Central.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Disc> Discs { get; set; }

        private static bool _created = false;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            if (!_created)
            {
                //Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Games");
            });

            modelBuilder.Entity<Disc>(entity =>
            {
                entity.ToTable("Discs");
            });
        }
    }
}
