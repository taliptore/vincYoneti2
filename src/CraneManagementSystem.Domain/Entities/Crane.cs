namespace CraneManagementSystem.Domain.Entities;

public class Crane : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public Guid? ConstructionSiteId { get; set; }
    public Guid? AssignedOperatorId { get; set; }

    public ConstructionSite? ConstructionSite { get; set; }
    public User? AssignedOperator { get; set; }
}
