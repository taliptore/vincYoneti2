namespace CraneManagementSystem.Application.DTOs.IncomeExpense;

public class IncomeExpenseCreateDto
{
    public int Type { get; set; }
    public string? Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public int ReferenceType { get; set; }
    public Guid? ReferenceId { get; set; }
    public Guid? CompanyId { get; set; }
}
