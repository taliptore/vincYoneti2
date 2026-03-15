using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class SliderConfiguration : IEntityTypeConfiguration<Slider>
{
    public void Configure(EntityTypeBuilder<Slider> builder)
    {
        builder.ToTable("Sliders");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(500);
        builder.Property(e => e.ShortText).HasMaxLength(1000);
        builder.Property(e => e.ImageUrl).HasMaxLength(500);
    }
}
