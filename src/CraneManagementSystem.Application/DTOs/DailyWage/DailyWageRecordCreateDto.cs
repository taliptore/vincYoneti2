namespace CraneManagementSystem.Application.DTOs.DailyWage;

public class DailyWageRecordCreateDto
{
    public Guid UserId { get; set; }
    public Guid? ConstructionSiteId { get; set; }
    public Guid? WorkPlanId { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}
