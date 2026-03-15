using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly ApplicationDbContext _context;

    public AnnouncementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Announcement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Announcements.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Announcement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Announcements.ToListAsync(cancellationToken);
    }

    public async Task<Announcement> AddAsync(Announcement entity, CancellationToken cancellationToken = default)
    {
        _context.Announcements.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Announcement entity, CancellationToken cancellationToken = default)
    {
        _context.Announcements.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Announcement entity, CancellationToken cancellationToken = default)
    {
        _context.Announcements.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
