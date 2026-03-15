using CraneManagementSystem.Application.DTOs.SystemSetting;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class SystemSettingService
{
    private readonly ISystemSettingRepository _repository;

    public SystemSettingService(ISystemSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<SystemSettingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<SystemSettingDto?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByKeyAsync(key, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<SystemSettingDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        return list.Select(MapToDto).ToList();
    }

    public async Task<SystemSettingDto> CreateOrUpdateAsync(SystemSettingCreateOrUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByKeyAsync(dto.Key, cancellationToken);
        if (existing != null)
        {
            existing.Value = dto.Value;
            existing.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(existing, cancellationToken);
            return MapToDto(existing);
        }

        var entity = new SystemSetting
        {
            Id = Guid.NewGuid(),
            Key = dto.Key,
            Value = dto.Value,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _repository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static SystemSettingDto MapToDto(SystemSetting e) => new()
    {
        Id = e.Id,
        Key = e.Key,
        Value = e.Value,
        CreatedAt = e.CreatedAt
    };
}
