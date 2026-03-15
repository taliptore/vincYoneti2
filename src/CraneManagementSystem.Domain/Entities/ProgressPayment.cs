namespace CraneManagementSystem.Domain.Entities;

public class ProgressPayment : BaseEntity
{
    public Guid? WorkPlanId { get; set; }
    public Guid CompanyId { get; set; }
    public decimal Amount { get; set; }
    public string? Period { get; set; }
    public string? Status { get; set; }

    public WorkPlan? WorkPlan { get; set; }
    public Company Company { get; set; } = null!;
}
