using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class GalleryItemConfiguration : IEntityTypeConfiguration<GalleryItem>
{
    public void Configure(EntityTypeBuilder<GalleryItem> builder)
    {
        builder.ToTable("GalleryItems");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(500);
        builder.Property(e => e.ImageUrl).HasMaxLength(500);
    }
}
