using CraneManagementSystem.Application.DTOs.DailyWage;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class DailyWageService
{
    private readonly IDailyWageRecordRepository _repository;

    public DailyWageService(IDailyWageRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<DailyWageRecordDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<DailyWageRecordDto>> GetAllAsync(Guid? filterByUserId, Guid? filterByCompanyId, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (filterByUserId.HasValue)
            result = result.Where(x => x.UserId == filterByUserId).ToList();
        if (filterByCompanyId.HasValue)
            result = result.Where(x => x.ConstructionSiteId.HasValue).ToList();
        return result;
    }

    public async Task<DailyWageRecordDto> CreateAsync(DailyWageRecordCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new DailyWageRecord
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            ConstructionSiteId = dto.ConstructionSiteId,
            WorkPlanId = dto.WorkPlanId,
            Date = dto.Date,
            Amount = dto.Amount,
            Description = dto.Description,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<DailyWageRecordDto?> UpdateAsync(Guid id, DailyWageRecordUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.UserId = dto.UserId;
        entity.ConstructionSiteId = dto.ConstructionSiteId;
        entity.WorkPlanId = dto.WorkPlanId;
        entity.Date = dto.Date;
        entity.Amount = dto.Amount;
        entity.Description = dto.Description;
        entity.Status = dto.Status;
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

    private static DailyWageRecordDto MapToDto(DailyWageRecord e) => new()
    {
        Id = e.Id,
        UserId = e.UserId,
        UserName = e.User?.FullName,
        ConstructionSiteId = e.ConstructionSiteId,
        ConstructionSiteName = e.ConstructionSite?.Name,
        WorkPlanId = e.WorkPlanId,
        WorkPlanTitle = e.WorkPlan?.Title,
        Date = e.Date,
        Amount = e.Amount,
        Description = e.Description,
        Status = e.Status,
        CreatedAt = e.CreatedAt
    };
}
