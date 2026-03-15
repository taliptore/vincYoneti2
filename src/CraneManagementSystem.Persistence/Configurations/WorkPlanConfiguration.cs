using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class WorkPlanConfiguration : IEntityTypeConfiguration<WorkPlan>
{
    public void Configure(EntityTypeBuilder<WorkPlan> builder)
    {
        builder.ToTable("WorkPlans");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.CraneId, e.PlannedStart });
        builder.Property(e => e.Title).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Status).HasMaxLength(50);

        builder.HasOne(e => e.Crane)
            .WithMany()
            .HasForeignKey(e => e.CraneId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.ConstructionSite)
            .WithMany()
            .HasForeignKey(e => e.ConstructionSiteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Company)
            .WithMany()
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
