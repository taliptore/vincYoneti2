namespace CraneManagementSystem.Domain.Entities;

public class MenuRole
{
    public Guid MenuId { get; set; }
    public Guid RoleId { get; set; }

    public Menu Menu { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
