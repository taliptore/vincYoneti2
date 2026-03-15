using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class FuelRecordRepository : IFuelRecordRepository
{
    private readonly ApplicationDbContext _context;

    public FuelRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FuelRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.FuelRecords
            .Include(f => f.Crane)
            .Include(f => f.Operator)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<FuelRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.FuelRecords
            .Include(f => f.Crane)
            .Include(f => f.Operator)
            .ToListAsync(cancellationToken);
    }

    public async Task<FuelRecord> AddAsync(FuelRecord entity, CancellationToken cancellationToken = default)
    {
        _context.FuelRecords.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(FuelRecord entity, CancellationToken cancellationToken = default)
    {
        _context.FuelRecords.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(FuelRecord entity, CancellationToken cancellationToken = default)
    {
        _context.FuelRecords.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
