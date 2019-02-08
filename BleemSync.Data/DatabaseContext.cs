using BleemSync.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=BleemSync.db");
        }
    }
}
