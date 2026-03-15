namespace CraneManagementSystem.Domain.Entities;

public class Menu : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Route { get; set; }
    public Guid? ParentId { get; set; }
    public int OrderNo { get; set; }
    public string? ModuleName { get; set; }
    public bool IsActive { get; set; } = true;

    public Menu? Parent { get; set; }
    public ICollection<Menu> Children { get; set; } = new List<Menu>();
    public ICollection<MenuRole> MenuRoles { get; set; } = new List<MenuRole>();
}
