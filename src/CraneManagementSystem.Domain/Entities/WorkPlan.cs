namespace CraneManagementSystem.Domain.Entities;

public class WorkPlan : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public Guid CraneId { get; set; }
    public Guid ConstructionSiteId { get; set; }
    public DateTime PlannedStart { get; set; }
    public DateTime PlannedEnd { get; set; }
    public string? Status { get; set; }
    public Guid? CompanyId { get; set; }

    public Crane Crane { get; set; } = null!;
    public ConstructionSite ConstructionSite { get; set; } = null!;
    public Company? Company { get; set; }
}
