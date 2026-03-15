using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class ProgressPaymentRepository : IProgressPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public ProgressPaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProgressPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProgressPayments
            .Include(p => p.WorkPlan)
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ProgressPayment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProgressPayments
            .Include(p => p.WorkPlan)
            .Include(p => p.Company)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProgressPayment> AddAsync(ProgressPayment entity, CancellationToken cancellationToken = default)
    {
        _context.ProgressPayments.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(ProgressPayment entity, CancellationToken cancellationToken = default)
    {
        _context.ProgressPayments.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ProgressPayment entity, CancellationToken cancellationToken = default)
    {
        _context.ProgressPayments.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
