using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Interfaces;

public interface IIncomeExpenseRepository
{
    Task<IncomeExpense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<IncomeExpense>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IncomeExpense> AddAsync(IncomeExpense entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(IncomeExpense entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(IncomeExpense entity, CancellationToken cancellationToken = default);
}
