namespace CraneManagementSystem.Application.DTOs.Crane;

public class CraneCreateDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public Guid? ConstructionSiteId { get; set; }
    public Guid? AssignedOperatorId { get; set; }
}
