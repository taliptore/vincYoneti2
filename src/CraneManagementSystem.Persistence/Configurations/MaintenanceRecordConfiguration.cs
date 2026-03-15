using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class MaintenanceRecordConfiguration : IEntityTypeConfiguration<MaintenanceRecord>
{
    public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
    {
        builder.ToTable("MaintenanceRecords");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.CraneId);
        builder.HasIndex(e => e.NextDueDate);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.Type).HasMaxLength(100);

        builder.HasOne(e => e.Crane)
            .WithMany()
            .HasForeignKey(e => e.CraneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
