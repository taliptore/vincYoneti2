using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class FuelRecordConfiguration : IEntityTypeConfiguration<FuelRecord>
{
    public void Configure(EntityTypeBuilder<FuelRecord> builder)
    {
        builder.ToTable("FuelRecords");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.CraneId, e.Date });
        builder.Property(e => e.Quantity).HasPrecision(18, 2);
        builder.Property(e => e.Unit).HasMaxLength(50);

        builder.HasOne(e => e.Crane)
            .WithMany()
            .HasForeignKey(e => e.CraneId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Operator)
            .WithMany()
            .HasForeignKey(e => e.OperatorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
