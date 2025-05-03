using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Infrastructure.Configurations
{
    public class ProductWarehouseConfiguration : IEntityTypeConfiguration<ProductWarehouse>
    {
        public void Configure(EntityTypeBuilder<ProductWarehouse> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Quantity)
                .IsRequired();

            builder.HasOne(e => e.Product)
               .WithMany(p => p.ProductWarehouses)
               .HasForeignKey(e => e.ProductId);

            builder.HasOne(e => e.Warehouse)
                .WithMany(w => w.ProductWarehouses)
                .HasForeignKey(e => e.WarehouseId);
        }
    }
}
