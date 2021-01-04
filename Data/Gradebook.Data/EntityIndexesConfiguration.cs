namespace Gradebook.Data
{
    using System.Linq;
    using Common;
    using Common.Models;
    using Microsoft.EntityFrameworkCore;

    internal static class EntityIndexesConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            // UniqueID index
            var personEntities = modelBuilder.Model.GetEntityTypes().Where(e => e.ClrType != null && typeof(BasePersonModel).IsAssignableFrom(e.ClrType));
            foreach (var entity in personEntities)
            {
                modelBuilder
                    .Entity(entity.ClrType)
                    .HasIndex(nameof(BasePersonModel.UniqueId))
                    .IsUnique();
            }

            // IDeletableEntity.IsDeleted index
            var deletableEntityTypes = modelBuilder.Model
                .GetEntityTypes()
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
            }
        }
    }
}
