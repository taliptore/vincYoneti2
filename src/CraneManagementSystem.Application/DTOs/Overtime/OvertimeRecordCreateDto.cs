namespace CraneManagementSystem.Application.DTOs.Overtime;

public class OvertimeRecordCreateDto
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public bool IsApproved { get; set; }
    public Guid? ApprovedByUserId { get; set; }
}
