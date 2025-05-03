using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Infrastructure.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .HasMaxLength(Warehouse.MAX_TITLE_LENGHT)
                .IsRequired();

            builder.HasMany(e => e.ProductWarehouses)
                .WithOne(e => e.Warehouse)
                .HasForeignKey(e => e.WarehouseId);

        }
    }
}
