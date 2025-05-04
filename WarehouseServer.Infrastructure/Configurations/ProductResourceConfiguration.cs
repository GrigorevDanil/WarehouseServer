using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Infrastructure.Configurations
{
    public class ProductResourceConfiguration : IEntityTypeConfiguration<ProductResource>
    {
        public void Configure(EntityTypeBuilder<ProductResource> builder)
        {
            builder.HasKey(e => new { e.ProductId, e.ResourceId });

            builder.Property(e => e.Quantity)
                .IsRequired();

            builder.HasOne(e => e.Product)
                .WithMany(p => p.ProductResources)
                .HasForeignKey(e => e.ProductId);

            builder.HasOne(e => e.Resource)
                .WithMany(r => r.ProductResources)
                .HasForeignKey(e => e.ResourceId);
        }
    }
}
