using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Icon).HasMaxLength(100);
        builder.Property(e => e.Route).HasMaxLength(500);
        builder.Property(e => e.ModuleName).HasMaxLength(100);
        builder.HasOne(e => e.Parent).WithMany(e => e.Children).HasForeignKey(e => e.ParentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.MenuRoles).WithOne(e => e.Menu).HasForeignKey(e => e.MenuId).OnDelete(DeleteBehavior.Cascade);
    }
}
