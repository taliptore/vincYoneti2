namespace CraneManagementSystem.Application.DTOs.ConstructionSite;

public class ConstructionSiteCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
