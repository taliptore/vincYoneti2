namespace CraneManagementSystem.Application.DTOs.ProgressPayment;

public class ProgressPaymentDto
{
    public Guid Id { get; set; }
    public Guid? WorkPlanId { get; set; }
    public string? WorkPlanTitle { get; set; }
    public Guid CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public decimal Amount { get; set; }
    public string? Period { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
