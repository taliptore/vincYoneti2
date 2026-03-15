using CraneManagementSystem.Application.DTOs.FuelTracking;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class FuelTrackingService
{
    private readonly IFuelRecordRepository _repository;

    public FuelTrackingService(IFuelRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<FuelRecordDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<FuelRecordDto>> GetAllAsync(Guid? filterByCraneId, Guid? filterByOperatorId, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (filterByCraneId.HasValue)
            result = result.Where(x => x.CraneId == filterByCraneId).ToList();
        if (filterByOperatorId.HasValue)
            result = result.Where(x => x.OperatorId == filterByOperatorId).ToList();
        return result;
    }

    public async Task<FuelRecordDto> CreateAsync(FuelRecordCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new FuelRecord
        {
            Id = Guid.NewGuid(),
            CraneId = dto.CraneId,
            Quantity = dto.Quantity,
            Unit = dto.Unit,
            Date = dto.Date,
            OperatorId = dto.OperatorId,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<FuelRecordDto?> UpdateAsync(Guid id, FuelRecordUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.CraneId = dto.CraneId;
        entity.Quantity = dto.Quantity;
        entity.Unit = dto.Unit;
        entity.Date = dto.Date;
        entity.OperatorId = dto.OperatorId;
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

    private static FuelRecordDto MapToDto(FuelRecord e) => new()
    {
        Id = e.Id,
        CraneId = e.CraneId,
        CraneName = e.Crane?.Name,
        Quantity = e.Quantity,
        Unit = e.Unit,
        Date = e.Date,
        OperatorId = e.OperatorId,
        OperatorName = e.Operator?.FullName,
        CreatedAt = e.CreatedAt
    };
}
