using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly ApplicationDbContext _context;

    public NewsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<News?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.News.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<News>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.News.ToListAsync(cancellationToken);
    }

    public async Task<News> AddAsync(News entity, CancellationToken cancellationToken = default)
    {
        _context.News.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(News entity, CancellationToken cancellationToken = default)
    {
        _context.News.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(News entity, CancellationToken cancellationToken = default)
    {
        _context.News.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
