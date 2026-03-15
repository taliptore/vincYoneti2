using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class WorkPlanRepository : IWorkPlanRepository
{
    private readonly ApplicationDbContext _context;

    public WorkPlanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.WorkPlans
            .Include(w => w.Crane)
            .Include(w => w.ConstructionSite)
            .Include(w => w.Company)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<WorkPlan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.WorkPlans
            .Include(w => w.Crane)
            .Include(w => w.ConstructionSite)
            .Include(w => w.Company)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkPlan> AddAsync(WorkPlan entity, CancellationToken cancellationToken = default)
    {
        _context.WorkPlans.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(WorkPlan entity, CancellationToken cancellationToken = default)
    {
        _context.WorkPlans.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(WorkPlan entity, CancellationToken cancellationToken = default)
    {
        _context.WorkPlans.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
