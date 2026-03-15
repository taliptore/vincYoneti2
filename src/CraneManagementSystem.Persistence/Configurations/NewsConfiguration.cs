using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.ToTable("News");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.IsPublished, e.PublishedAt });
        builder.Property(e => e.Title).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Summary).HasMaxLength(1000);
        builder.Property(e => e.Body).HasMaxLength(8000);
        builder.Property(e => e.ImageUrl).HasMaxLength(500);
    }
}
