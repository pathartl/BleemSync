using BleemSync.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BleemSync.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Disc> Discs { get; set; }
        public DbSet<Game> Games { get; set; }

        // Not used in US consoles
        public DbSet<Language> Languages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var executingDirectory = Utilities.Filesystem.GetExecutingDirectory();

            optionsBuilder.UseSqlite($"Data Source={executingDirectory}\\..\\Games\\databases\\regional.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("GAME");
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
