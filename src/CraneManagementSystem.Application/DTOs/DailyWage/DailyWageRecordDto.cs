namespace CraneManagementSystem.Application.DTOs.DailyWage;

public class DailyWageRecordDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public Guid? ConstructionSiteId { get; set; }
    public string? ConstructionSiteName { get; set; }
    public Guid? WorkPlanId { get; set; }
    public string? WorkPlanTitle { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
