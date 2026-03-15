using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.ToTable("SystemSettings");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Key).IsUnique();
        builder.Property(e => e.Key).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Value).HasMaxLength(-1); // nvarchar(max)
    }
}
