namespace CraneManagementSystem.Domain.Entities;

public class MaintenanceRecord : BaseEntity
{
    public Guid CraneId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public DateTime? NextDueDate { get; set; }

    public Crane Crane { get; set; } = null!;
}
