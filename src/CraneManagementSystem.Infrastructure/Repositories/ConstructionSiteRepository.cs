using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class ConstructionSiteRepository : IConstructionSiteRepository
{
    private readonly ApplicationDbContext _context;

    public ConstructionSiteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ConstructionSite?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ConstructionSites.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<ConstructionSite>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ConstructionSites.ToListAsync(cancellationToken);
    }

    public async Task<ConstructionSite> AddAsync(ConstructionSite entity, CancellationToken cancellationToken = default)
    {
        _context.ConstructionSites.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(ConstructionSite entity, CancellationToken cancellationToken = default)
    {
        _context.ConstructionSites.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ConstructionSite entity, CancellationToken cancellationToken = default)
    {
        _context.ConstructionSites.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
