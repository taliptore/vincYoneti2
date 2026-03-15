using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class MenuRoleConfiguration : IEntityTypeConfiguration<MenuRole>
{
    public void Configure(EntityTypeBuilder<MenuRole> builder)
    {
        builder.ToTable("MenuRoles");
        builder.HasKey(e => new { e.MenuId, e.RoleId });
        builder.HasOne(e => e.Menu).WithMany(e => e.MenuRoles).HasForeignKey(e => e.MenuId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Role).WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);
    }
}
