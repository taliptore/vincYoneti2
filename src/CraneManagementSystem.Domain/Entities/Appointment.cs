namespace CraneManagementSystem.Domain.Entities;

public class Appointment : BaseEntity
{
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime PreferredDate { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
    public Guid? CompanyId { get; set; }

    public Company? Company { get; set; }
}
