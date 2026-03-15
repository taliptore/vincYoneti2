using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class IncomeExpenseConfiguration : IEntityTypeConfiguration<IncomeExpense>
{
    public void Configure(EntityTypeBuilder<IncomeExpense> builder)
    {
        builder.ToTable("IncomeExpenses");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Type);
        builder.HasIndex(e => e.Date);
        builder.Property(e => e.Category).HasMaxLength(100);
        builder.Property(e => e.Amount).HasPrecision(18, 2);
        builder.Property(e => e.Description).HasMaxLength(1000);

        builder.HasOne(e => e.Company)
            .WithMany()
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
