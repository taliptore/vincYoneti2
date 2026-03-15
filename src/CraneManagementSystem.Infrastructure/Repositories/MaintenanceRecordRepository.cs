using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class MaintenanceRecordRepository : IMaintenanceRecordRepository
{
    private readonly ApplicationDbContext _context;

    public MaintenanceRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MaintenanceRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.MaintenanceRecords
            .Include(m => m.Crane)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<MaintenanceRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MaintenanceRecords
            .Include(m => m.Crane)
            .ToListAsync(cancellationToken);
    }

    public async Task<MaintenanceRecord> AddAsync(MaintenanceRecord entity, CancellationToken cancellationToken = default)
    {
        _context.MaintenanceRecords.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(MaintenanceRecord entity, CancellationToken cancellationToken = default)
    {
        _context.MaintenanceRecords.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(MaintenanceRecord entity, CancellationToken cancellationToken = default)
    {
        _context.MaintenanceRecords.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
