using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class OperatorAssignmentRepository : IOperatorAssignmentRepository
{
    private readonly ApplicationDbContext _context;

    public OperatorAssignmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperatorAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.OperatorAssignments
            .Include(o => o.User)
            .Include(o => o.Crane)
            .Include(o => o.ConstructionSite)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<OperatorAssignment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OperatorAssignments
            .Include(o => o.User)
            .Include(o => o.Crane)
            .Include(o => o.ConstructionSite)
            .ToListAsync(cancellationToken);
    }

    public async Task<OperatorAssignment> AddAsync(OperatorAssignment entity, CancellationToken cancellationToken = default)
    {
        _context.OperatorAssignments.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(OperatorAssignment entity, CancellationToken cancellationToken = default)
    {
        _context.OperatorAssignments.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(OperatorAssignment entity, CancellationToken cancellationToken = default)
    {
        _context.OperatorAssignments.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
