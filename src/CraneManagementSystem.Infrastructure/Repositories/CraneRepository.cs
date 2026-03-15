using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class CraneRepository : ICraneRepository
{
    private readonly ApplicationDbContext _context;

    public CraneRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Crane?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cranes
            .Include(c => c.ConstructionSite)
            .Include(c => c.AssignedOperator)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Crane>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Cranes
            .Include(c => c.ConstructionSite)
            .Include(c => c.AssignedOperator)
            .ToListAsync(cancellationToken);
    }

    public async Task<Crane> AddAsync(Crane entity, CancellationToken cancellationToken = default)
    {
        _context.Cranes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Crane entity, CancellationToken cancellationToken = default)
    {
        _context.Cranes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Crane entity, CancellationToken cancellationToken = default)
    {
        _context.Cranes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
