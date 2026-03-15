namespace CraneManagementSystem.Domain.Entities;

public class Role : BaseEntity
{
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
}
