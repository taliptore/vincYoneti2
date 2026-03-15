namespace CraneManagementSystem.Domain.Entities;

public class DailyWageRecord : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? ConstructionSiteId { get; set; }
    public Guid? WorkPlanId { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }

    public User User { get; set; } = null!;
    public ConstructionSite? ConstructionSite { get; set; }
    public WorkPlan? WorkPlan { get; set; }
}
