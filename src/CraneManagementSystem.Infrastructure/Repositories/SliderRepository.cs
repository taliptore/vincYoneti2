using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class SliderRepository : ISliderRepository
{
    private readonly ApplicationDbContext _context;

    public SliderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Slider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sliders.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Slider>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sliders.OrderBy(s => s.SortOrder).ToListAsync(cancellationToken);
    }

    public async Task<Slider> AddAsync(Slider entity, CancellationToken cancellationToken = default)
    {
        _context.Sliders.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Slider entity, CancellationToken cancellationToken = default)
    {
        _context.Sliders.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Slider entity, CancellationToken cancellationToken = default)
    {
        _context.Sliders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
