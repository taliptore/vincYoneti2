using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ModuleName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.ActionName).IsRequired().HasMaxLength(50);
    }
}
