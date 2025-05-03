using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Infrastructure.Configurations
{
    public class DistanceConfiguration : IEntityTypeConfiguration<Distance>
    {
        public void Configure(EntityTypeBuilder<Distance> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Shop)
                .WithMany(s => s.Distances)
                .HasForeignKey(e => e.ShopId);

            builder.HasOne(e => e.Warehouse)
                .WithMany(w => w.Distances)
                .HasForeignKey(e => e.WarehouseId);

            builder.Property(e => e.Length)
                .IsRequired();
        }
    }
}
