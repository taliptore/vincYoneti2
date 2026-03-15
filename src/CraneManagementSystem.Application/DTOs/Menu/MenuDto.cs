namespace CraneManagementSystem.Application.DTOs.Menu;

public class MenuDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Route { get; set; }
    public Guid? ParentId { get; set; }
    public int OrderNo { get; set; }
    public string? ModuleName { get; set; }
    public bool IsActive { get; set; }
    public List<MenuDto> Children { get; set; } = new();
    public List<Guid>? RoleIds { get; set; }
}
