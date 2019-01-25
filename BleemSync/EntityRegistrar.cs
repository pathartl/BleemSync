using BleemSync.Data.Entities;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace BleemSync.Data
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<GameManagerNode>(etb =>
            {
                etb.HasKey(e => e.Id);
                etb.Property(e => e.Id);
                etb.HasOne(e => e.Parent).WithMany(e => e.Children);
                etb.ToTable("GameManagerNodes");
            });

            modelbuilder.Entity<GameManagerFile>(etb =>
            {
                etb.HasKey(e => e.Id);
                etb.Property(e => e.Id);
                etb.HasOne(e => e.Node).WithMany(e => e.Files);
                etb.ToTable("GameManagerFiles");
            });
        }
    }
}
