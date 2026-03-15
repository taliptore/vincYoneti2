using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class OvertimeRecordRepository : IOvertimeRecordRepository
{
    private readonly ApplicationDbContext _context;

    public OvertimeRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OvertimeRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.OvertimeRecords
            .Include(o => o.User)
            .Include(o => o.ApprovedByUser)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<OvertimeRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OvertimeRecords
            .Include(o => o.User)
            .Include(o => o.ApprovedByUser)
            .ToListAsync(cancellationToken);
    }

    public async Task<OvertimeRecord> AddAsync(OvertimeRecord entity, CancellationToken cancellationToken = default)
    {
        _context.OvertimeRecords.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(OvertimeRecord entity, CancellationToken cancellationToken = default)
    {
        _context.OvertimeRecords.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(OvertimeRecord entity, CancellationToken cancellationToken = default)
    {
        _context.OvertimeRecords.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
