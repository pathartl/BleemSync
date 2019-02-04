using BleemSync.Central.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BleemSync.Central.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Models.PlayStation.Art> PlayStation_Art { get; set; }
        public DbSet<Models.PlayStation.Disc> PlayStation_Discs { get; set; }
        public DbSet<Models.PlayStation.Game> PlayStation_Games { get; set; }
        public DbSet<GameGenre> Genres { get; set; }

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
        }
    }
}
