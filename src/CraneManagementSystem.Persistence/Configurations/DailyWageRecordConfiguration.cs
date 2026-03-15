using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class DailyWageRecordConfiguration : IEntityTypeConfiguration<DailyWageRecord>
{
    public void Configure(EntityTypeBuilder<DailyWageRecord> builder)
    {
        builder.ToTable("DailyWageRecords");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.UserId, e.Date });
        builder.Property(e => e.Amount).HasPrecision(18, 2);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.Status).HasMaxLength(50);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.ConstructionSite)
            .WithMany()
            .HasForeignKey(e => e.ConstructionSiteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.WorkPlan)
            .WithMany()
            .HasForeignKey(e => e.WorkPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
