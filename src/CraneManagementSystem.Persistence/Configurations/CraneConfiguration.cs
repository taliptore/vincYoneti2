using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class CraneConfiguration : IEntityTypeConfiguration<Crane>
{
    public void Configure(EntityTypeBuilder<Crane> builder)
    {
        builder.ToTable("Cranes");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.ConstructionSiteId);
        builder.HasIndex(e => e.Status);
        builder.Property(e => e.Code).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Type).HasMaxLength(100);
        builder.Property(e => e.Location).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(50);

        builder.HasOne(e => e.ConstructionSite)
            .WithMany()
            .HasForeignKey(e => e.ConstructionSiteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.AssignedOperator)
            .WithMany()
            .HasForeignKey(e => e.AssignedOperatorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
