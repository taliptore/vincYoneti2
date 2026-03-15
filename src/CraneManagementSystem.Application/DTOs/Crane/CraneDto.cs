namespace CraneManagementSystem.Application.DTOs.Crane;

public class CraneDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public Guid? ConstructionSiteId { get; set; }
    public string? ConstructionSiteName { get; set; }
    public Guid? AssignedOperatorId { get; set; }
    public string? AssignedOperatorName { get; set; }
    public DateTime CreatedAt { get; set; }
}
