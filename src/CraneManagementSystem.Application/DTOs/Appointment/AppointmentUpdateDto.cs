namespace CraneManagementSystem.Application.DTOs.Appointment;

public class AppointmentUpdateDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime PreferredDate { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
    public Guid? CompanyId { get; set; }
}
