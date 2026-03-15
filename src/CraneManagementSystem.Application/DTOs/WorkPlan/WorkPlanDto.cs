namespace CraneManagementSystem.Application.DTOs.WorkPlan;

public class WorkPlanDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid CraneId { get; set; }
    public string? CraneName { get; set; }
    public Guid ConstructionSiteId { get; set; }
    public string? ConstructionSiteName { get; set; }
    public DateTime PlannedStart { get; set; }
    public DateTime PlannedEnd { get; set; }
    public string? Status { get; set; }
    public Guid? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public DateTime CreatedAt { get; set; }
}
