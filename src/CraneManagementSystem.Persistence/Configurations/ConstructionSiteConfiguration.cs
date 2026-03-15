using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class ConstructionSiteConfiguration : IEntityTypeConfiguration<ConstructionSite>
{
    public void Configure(EntityTypeBuilder<ConstructionSite> builder)
    {
        builder.ToTable("ConstructionSites");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Address).HasMaxLength(500);
    }
}
