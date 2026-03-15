using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class OvertimeRecordConfiguration : IEntityTypeConfiguration<OvertimeRecord>
{
    public void Configure(EntityTypeBuilder<OvertimeRecord> builder)
    {
        builder.ToTable("OvertimeRecords");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.UserId, e.Date });
        builder.Property(e => e.Hours).HasPrecision(18, 2);
        builder.Property(e => e.Rate).HasPrecision(18, 2);
        builder.Property(e => e.Amount).HasPrecision(18, 2);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.ApprovedByUser)
            .WithMany()
            .HasForeignKey(e => e.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
