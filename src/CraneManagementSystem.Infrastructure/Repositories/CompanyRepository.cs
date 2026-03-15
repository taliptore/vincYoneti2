using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Companies.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Companies.ToListAsync(cancellationToken);
    }

    public async Task<Company> AddAsync(Company entity, CancellationToken cancellationToken = default)
    {
        _context.Companies.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Company entity, CancellationToken cancellationToken = default)
    {
        _context.Companies.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Company entity, CancellationToken cancellationToken = default)
    {
        _context.Companies.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
