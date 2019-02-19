using BleemSync.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BleemSync.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<GameGenre> Genres { get; set; }
        public DbSet<GameManagerFile> Files { get; set; }
        public DbSet<GameManagerNode> Nodes { get; set; }
        public DbSet<GameMeta> Meta { get; set; }
        public DbSet<GameSystem> Systems { get; set; }

        private readonly IConfiguration _config;

        public DatabaseContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;

            Directory.CreateDirectory(Path.Combine(_config["BleemSync:Destination"], _config["BleemSync:Path"]));
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringPath = Path.Combine(_config["BleemSync:Destination"], _config["BleemSync:Path"], _config["BleemSync:DatabaseFile"]);

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite($"Data Source={connectionStringPath}");
        }
    }
}
