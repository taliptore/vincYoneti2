namespace CraneManagementSystem.Domain.Entities;

public class OperatorAssignment : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? CraneId { get; set; }
    public Guid? ConstructionSiteId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public User User { get; set; } = null!;
    public Crane? Crane { get; set; }
    public ConstructionSite? ConstructionSite { get; set; }
}
