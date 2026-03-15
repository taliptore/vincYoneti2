using CraneManagementSystem.Domain;

namespace CraneManagementSystem.Domain.Entities;

public class IncomeExpense : BaseEntity
{
    public IncomeExpenseType Type { get; set; }
    public string? Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public ReferenceType ReferenceType { get; set; }
    public Guid? ReferenceId { get; set; }
    public Guid? CompanyId { get; set; }

    public Company? Company { get; set; }
}
