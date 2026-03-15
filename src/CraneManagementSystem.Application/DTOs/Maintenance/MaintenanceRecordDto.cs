namespace CraneManagementSystem.Application.DTOs.Maintenance;

public class MaintenanceRecordDto
{
    public Guid Id { get; set; }
    public Guid CraneId { get; set; }
    public string? CraneName { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public DateTime? NextDueDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
