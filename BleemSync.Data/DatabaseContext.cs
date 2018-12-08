using BleemSync.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace BleemSync.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Disc> Discs { get; set; }
        public DbSet<Game> Games { get; set; }

        // Not used in US consoles
        public DbSet<Language> Languages { get; set; }

        private static bool _created = false;

        public DatabaseContext()
        {
            if (!_created)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var executingDirectory = Utilities.Filesystem.GetExecutingDirectory();

            Directory.CreateDirectory($"{executingDirectory}\\..\\System\\Databases");
            optionsBuilder.UseSqlite($"Data Source={executingDirectory}\\..\\System\\Databases\\regional.db");
            //optionsBuilder.UseSqlite($"Data Source={executingDirectory}\\..\\Games\\databases\\regional.db");
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
