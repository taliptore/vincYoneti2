namespace CraneManagementSystem.Application.DTOs.Maintenance;

public class MaintenanceRecordCreateDto
{
    public Guid CraneId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public DateTime? NextDueDate { get; set; }
}
