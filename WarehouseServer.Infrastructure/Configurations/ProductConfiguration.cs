using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(Product.MAX_TITLE_LENGHT);

            builder.Property(e => e.Cost)
                .IsRequired();

            builder.HasMany(e => e.ProductResources)
                .WithOne(pr => pr.Product)
                .HasForeignKey(pr => pr.ProductId);

            builder.HasMany(e => e.ProductWarehouses)
                .WithOne()
                .HasForeignKey(pw => pw.ProductId);
        }
    }
}
