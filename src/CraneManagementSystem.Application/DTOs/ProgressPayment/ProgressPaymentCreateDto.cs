namespace CraneManagementSystem.Application.DTOs.ProgressPayment;

public class ProgressPaymentCreateDto
{
    public Guid? WorkPlanId { get; set; }
    public Guid CompanyId { get; set; }
    public decimal Amount { get; set; }
    public string? Period { get; set; }
    public string? Status { get; set; }
}
