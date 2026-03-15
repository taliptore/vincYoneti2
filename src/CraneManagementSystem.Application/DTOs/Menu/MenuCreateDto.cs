namespace CraneManagementSystem.Application.DTOs.Menu;

public class MenuCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Route { get; set; }
    public Guid? ParentId { get; set; }
    public int OrderNo { get; set; }
    public string? ModuleName { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Guid> RoleIds { get; set; } = new();
}
