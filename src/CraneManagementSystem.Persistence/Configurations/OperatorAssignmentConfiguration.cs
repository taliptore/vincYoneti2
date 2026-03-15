using CraneManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraneManagementSystem.Persistence.Configurations;

public class OperatorAssignmentConfiguration : IEntityTypeConfiguration<OperatorAssignment>
{
    public void Configure(EntityTypeBuilder<OperatorAssignment> builder)
    {
        builder.ToTable("OperatorAssignments");
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Crane)
            .WithMany()
            .HasForeignKey(e => e.CraneId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.ConstructionSite)
            .WithMany()
            .HasForeignKey(e => e.ConstructionSiteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
