namespace CraneManagementSystem.Domain.Entities;

public class FuelRecord : BaseEntity
{
    public Guid CraneId { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public DateTime Date { get; set; }
    public Guid OperatorId { get; set; }

    public Crane Crane { get; set; } = null!;
    public User Operator { get; set; } = null!;
}
