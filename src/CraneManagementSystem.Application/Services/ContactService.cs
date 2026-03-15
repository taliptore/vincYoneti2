using CraneManagementSystem.Application.DTOs.Contact;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class ContactService
{
    private readonly IContactMessageRepository _repository;

    public ContactService(IContactMessageRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactMessageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<ContactMessageDto>> GetAllAsync(bool? unreadOnly, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (unreadOnly == true)
            result = result.Where(x => !x.IsRead).ToList();
        return result;
    }

    public async Task<ContactMessageDto> CreateAsync(ContactMessageCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new ContactMessage
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Subject = dto.Subject,
            Message = dto.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> MarkAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        entity.IsRead = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(entity, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _repository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static ContactMessageDto MapToDto(ContactMessage e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Email = e.Email,
        Phone = e.Phone,
        Subject = e.Subject,
        Message = e.Message,
        IsRead = e.IsRead,
        CreatedAt = e.CreatedAt
    };
}
