namespace CraneManagementSystem.Domain.Entities;

public class ConstructionSite : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
