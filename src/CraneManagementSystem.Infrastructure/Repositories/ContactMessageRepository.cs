using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CraneManagementSystem.Infrastructure.Repositories;

public class ContactMessageRepository : IContactMessageRepository
{
    private readonly ApplicationDbContext _context;

    public ContactMessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContactMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ContactMessages.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<ContactMessage>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ContactMessages.ToListAsync(cancellationToken);
    }

    public async Task<ContactMessage> AddAsync(ContactMessage entity, CancellationToken cancellationToken = default)
    {
        _context.ContactMessages.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(ContactMessage entity, CancellationToken cancellationToken = default)
    {
        _context.ContactMessages.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ContactMessage entity, CancellationToken cancellationToken = default)
    {
        _context.ContactMessages.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
