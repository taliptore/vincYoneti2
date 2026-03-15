using CraneManagementSystem.Application.DTOs.Maintenance;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class MaintenanceService
{
    private readonly IMaintenanceRecordRepository _repository;

    public MaintenanceService(IMaintenanceRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<MaintenanceRecordDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<MaintenanceRecordDto>> GetAllAsync(Guid? filterByCraneId, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (filterByCraneId.HasValue)
            result = result.Where(x => x.CraneId == filterByCraneId).ToList();
        return result;
    }

    public async Task<MaintenanceRecordDto> CreateAsync(MaintenanceRecordCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new MaintenanceRecord
        {
            Id = Guid.NewGuid(),
            CraneId = dto.CraneId,
            MaintenanceDate = dto.MaintenanceDate,
            Description = dto.Description,
            Type = dto.Type,
            NextDueDate = dto.NextDueDate,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<MaintenanceRecordDto?> UpdateAsync(Guid id, MaintenanceRecordUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.CraneId = dto.CraneId;
        entity.MaintenanceDate = dto.MaintenanceDate;
        entity.Description = dto.Description;
        entity.Type = dto.Type;
        entity.NextDueDate = dto.NextDueDate;
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _repository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static MaintenanceRecordDto MapToDto(MaintenanceRecord e) => new()
    {
        Id = e.Id,
        CraneId = e.CraneId,
        CraneName = e.Crane?.Name,
        MaintenanceDate = e.MaintenanceDate,
        Description = e.Description,
        Type = e.Type,
        NextDueDate = e.NextDueDate,
        CreatedAt = e.CreatedAt
    };
}
