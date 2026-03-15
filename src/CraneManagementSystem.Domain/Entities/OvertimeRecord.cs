namespace CraneManagementSystem.Domain.Entities;

public class OvertimeRecord : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public bool IsApproved { get; set; }
    public Guid? ApprovedByUserId { get; set; }

    public User User { get; set; } = null!;
    public User? ApprovedByUser { get; set; }
}
