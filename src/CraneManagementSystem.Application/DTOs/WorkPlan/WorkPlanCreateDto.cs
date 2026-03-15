namespace CraneManagementSystem.Application.DTOs.WorkPlan;

public class WorkPlanCreateDto
{
    public string Title { get; set; } = string.Empty;
    public Guid CraneId { get; set; }
    public Guid ConstructionSiteId { get; set; }
    public DateTime PlannedStart { get; set; }
    public DateTime PlannedEnd { get; set; }
    public string? Status { get; set; }
    public Guid? CompanyId { get; set; }
}
