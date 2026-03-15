namespace CraneManagementSystem.Application.DTOs.Overtime;

public class OvertimeRecordUpdateDto
{
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public bool IsApproved { get; set; }
    public Guid? ApprovedByUserId { get; set; }
}
