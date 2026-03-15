using CraneManagementSystem.Application.DTOs.Overtime;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class OvertimeService
{
    private readonly IOvertimeRecordRepository _repository;

    public OvertimeService(IOvertimeRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<OvertimeRecordDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<OvertimeRecordDto>> GetAllAsync(Guid? filterByUserId, Guid? filterByCompanyId, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (filterByUserId.HasValue)
            result = result.Where(x => x.UserId == filterByUserId).ToList();
        return result;
    }

    public async Task<OvertimeRecordDto> CreateAsync(OvertimeRecordCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new OvertimeRecord
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Date = dto.Date,
            Hours = dto.Hours,
            Rate = dto.Rate,
            Amount = dto.Amount,
            IsApproved = dto.IsApproved,
            ApprovedByUserId = dto.ApprovedByUserId,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<OvertimeRecordDto?> UpdateAsync(Guid id, OvertimeRecordUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Date = dto.Date;
        entity.Hours = dto.Hours;
        entity.Rate = dto.Rate;
        entity.Amount = dto.Amount;
        entity.IsApproved = dto.IsApproved;
        entity.ApprovedByUserId = dto.ApprovedByUserId;
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

    private static OvertimeRecordDto MapToDto(OvertimeRecord e) => new()
    {
        Id = e.Id,
        UserId = e.UserId,
        UserName = e.User?.FullName,
        Date = e.Date,
        Hours = e.Hours,
        Rate = e.Rate,
        Amount = e.Amount,
        IsApproved = e.IsApproved,
        ApprovedByUserId = e.ApprovedByUserId,
        ApprovedByUserName = e.ApprovedByUser?.FullName,
        CreatedAt = e.CreatedAt
    };
}
