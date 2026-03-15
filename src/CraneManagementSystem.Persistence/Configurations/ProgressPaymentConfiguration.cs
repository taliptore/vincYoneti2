using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class ProgressPaymentConfiguration : IEntityTypeConfiguration<ProgressPayment>
{
    public void Configure(EntityTypeBuilder<ProgressPayment> builder)
    {
        builder.ToTable("ProgressPayments");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.CompanyId);
        builder.HasIndex(e => e.Period);
        builder.Property(e => e.Amount).HasPrecision(18, 2);
        builder.Property(e => e.Period).HasMaxLength(100);
        builder.Property(e => e.Status).HasMaxLength(50);

        builder.HasOne(e => e.WorkPlan)
            .WithMany()
            .HasForeignKey(e => e.WorkPlanId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Company)
            .WithMany()
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
