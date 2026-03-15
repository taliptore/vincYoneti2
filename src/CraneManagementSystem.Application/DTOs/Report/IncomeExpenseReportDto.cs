namespace CraneManagementSystem.Application.DTOs.Report;

public class IncomeExpenseReportDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
    public List<IncomeExpenseReportItemDto> Items { get; set; } = new();
}

public class IncomeExpenseReportItemDto
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string? Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}
