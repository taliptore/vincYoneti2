namespace CraneManagementSystem.Application.DTOs.FuelTracking;

public class FuelRecordUpdateDto
{
    public Guid CraneId { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public DateTime Date { get; set; }
    public Guid OperatorId { get; set; }
}
