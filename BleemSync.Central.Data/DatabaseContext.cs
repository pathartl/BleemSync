using BleemSync.Central.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BleemSync.Central.Data
{
    public class DatabaseContext : IdentityDbContext
    {
        public DbSet<Models.PlayStation.Art> PlayStation_Art { get; set; }
        public DbSet<Models.PlayStation.Disc> PlayStation_Discs { get; set; }
        public DbSet<Models.PlayStation.Game> PlayStation_Games { get; set; }
        public DbSet<Models.PlayStation.GameRevision> PlayStation_GameRevisions { get; set; }
        public DbSet<GameGenre> Genres { get; set; }
        public DbSet<EsrbRatingDescriptor> EsrbRatingDescriptors { get; set; }
        public DbSet<PegiRatingDescriptor> PegiRatingDescriptors { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        private static bool _created = false;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            if (!_created)
            {
                Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.PlayStation.GameRevision>()
                .HasOne(gr => gr.Game)
                .WithMany(g => g.Revisions)
                .HasForeignKey(gr => gr.GameId);

            modelBuilder.Entity<Models.PlayStation.GameRevision>()
                .HasOne(gr => gr.RevisedGame)
                .WithOne(g => g.Revision)
                .HasForeignKey<Models.PlayStation.GameRevision>(g => g.RevisedGameId);
        }
    }
}
