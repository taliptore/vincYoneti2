using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class DailyWageRecordRepository : IDailyWageRecordRepository
{
    private readonly ApplicationDbContext _context;

    public DailyWageRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DailyWageRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DailyWageRecords
            .Include(d => d.User)
            .Include(d => d.ConstructionSite)
            .Include(d => d.WorkPlan)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<DailyWageRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DailyWageRecords
            .Include(d => d.User)
            .Include(d => d.ConstructionSite)
            .Include(d => d.WorkPlan)
            .ToListAsync(cancellationToken);
    }

    public async Task<DailyWageRecord> AddAsync(DailyWageRecord entity, CancellationToken cancellationToken = default)
    {
        _context.DailyWageRecords.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(DailyWageRecord entity, CancellationToken cancellationToken = default)
    {
        _context.DailyWageRecords.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(DailyWageRecord entity, CancellationToken cancellationToken = default)
    {
        _context.DailyWageRecords.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
