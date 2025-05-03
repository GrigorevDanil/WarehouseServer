using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseServer.Domain.Entities;

namespace WarehouseServer.Infrastructure.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(Resource.MAX_TITLE_LENGHT);

            builder.Property(e => e.Unit)
                .IsRequired()
                .HasMaxLength(Resource.MAX_UNIT_LENGHT);


            builder.HasMany(e => e.ProductResources)
                .WithOne(e => e.Resource)
                .HasForeignKey(pr => pr.ResourceId);
        }
    }
}
