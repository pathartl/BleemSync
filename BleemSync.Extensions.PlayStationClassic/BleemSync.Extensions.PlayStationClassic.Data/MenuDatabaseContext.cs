using BleemSync.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BleemSync.Data
{
    public class MenuDatabaseContext : DbContext
    {
        public DbSet<Disc> Discs { get; set; }
        public DbSet<Game> Games { get; set; }

        // Not used in US consoles
        public DbSet<Language> Languages { get; set; }

        private static bool _created = false;

        public MenuDatabaseContext(DbContextOptions<MenuDatabaseContext> options, IConfiguration configuration) : base(options)
        {
            Directory.CreateDirectory(
                Path.Combine(
                    configuration["BleemSync:Destination"],
                    configuration["BleemSync:Path"]
                )
            );

            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("MENU_ENTRIES");
            });


            modelBuilder.Entity<Disc>(entity =>
            {
                entity.ToTable("DISC");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("LANGUAGE_SPECIFIC");
            });
        }
    }
}
