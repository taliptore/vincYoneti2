using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class GalleryItemRepository : IGalleryItemRepository
{
    private readonly ApplicationDbContext _context;

    public GalleryItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GalleryItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.GalleryItems.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<GalleryItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.GalleryItems.OrderBy(g => g.SortOrder).ToListAsync(cancellationToken);
    }

    public async Task<GalleryItem> AddAsync(GalleryItem entity, CancellationToken cancellationToken = default)
    {
        _context.GalleryItems.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(GalleryItem entity, CancellationToken cancellationToken = default)
    {
        _context.GalleryItems.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(GalleryItem entity, CancellationToken cancellationToken = default)
    {
        _context.GalleryItems.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
