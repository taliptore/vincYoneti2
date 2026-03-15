namespace CraneManagementSystem.Application.DTOs.FuelTracking;

public class FuelRecordDto
{
    public Guid Id { get; set; }
    public Guid CraneId { get; set; }
    public string? CraneName { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public DateTime Date { get; set; }
    public Guid OperatorId { get; set; }
    public string? OperatorName { get; set; }
    public DateTime CreatedAt { get; set; }
}
