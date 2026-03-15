using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class IncomeExpenseRepository : IIncomeExpenseRepository
{
    private readonly ApplicationDbContext _context;

    public IncomeExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IncomeExpense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.IncomeExpenses
            .Include(i => i.Company)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<IncomeExpense>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.IncomeExpenses
            .Include(i => i.Company)
            .ToListAsync(cancellationToken);
    }

    public async Task<IncomeExpense> AddAsync(IncomeExpense entity, CancellationToken cancellationToken = default)
    {
        _context.IncomeExpenses.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(IncomeExpense entity, CancellationToken cancellationToken = default)
    {
        _context.IncomeExpenses.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(IncomeExpense entity, CancellationToken cancellationToken = default)
    {
        _context.IncomeExpenses.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
