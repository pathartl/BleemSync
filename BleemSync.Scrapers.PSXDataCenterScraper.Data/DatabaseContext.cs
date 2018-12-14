using BleemSync.Scrapers.PSXDataCenterScraper.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace BleemSync.Scrapers.PSXDataCenterScraper.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Disc> Discs { get; set; }

        private static bool _created = false;

        public DatabaseContext()
        {
            if (!_created)
            {
                //Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var executingDirectory = Utilities.Filesystem.GetExecutingDirectory();

            var databasesDirectoryPath = Path.Combine(new[] { executingDirectory, ".." });
            var databasePath = Path.Join(databasesDirectoryPath, "psxdatacenter.db");

            Directory.CreateDirectory(databasesDirectoryPath);
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseMySql("Server=10.0.1.10;Port=3308;Database=psxdatacenter;User=psx;Password=psx;charset=utf8mb4");
                //.UseSqlite($"Data Source={databasePath}");
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
