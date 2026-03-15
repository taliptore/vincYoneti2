using CraneManagementSystem.Application.DTOs.WorkPlan;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class WorkPlanService
{
    private readonly IWorkPlanRepository _workPlanRepository;

    public WorkPlanService(IWorkPlanRepository workPlanRepository)
    {
        _workPlanRepository = workPlanRepository;
    }

    public async Task<WorkPlanDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _workPlanRepository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<WorkPlanDto>> GetAllAsync(Guid? filterByCompanyId, CancellationToken cancellationToken = default)
    {
        var list = await _workPlanRepository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (filterByCompanyId.HasValue)
            result = result.Where(x => x.CompanyId == filterByCompanyId).ToList();
        return result;
    }

    public async Task<WorkPlanDto> CreateAsync(WorkPlanCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new WorkPlan
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            CraneId = dto.CraneId,
            ConstructionSiteId = dto.ConstructionSiteId,
            PlannedStart = dto.PlannedStart,
            PlannedEnd = dto.PlannedEnd,
            Status = dto.Status,
            CompanyId = dto.CompanyId,
            CreatedAt = DateTime.UtcNow
        };
        await _workPlanRepository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<WorkPlanDto?> UpdateAsync(Guid id, WorkPlanUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _workPlanRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Title = dto.Title;
        entity.CraneId = dto.CraneId;
        entity.ConstructionSiteId = dto.ConstructionSiteId;
        entity.PlannedStart = dto.PlannedStart;
        entity.PlannedEnd = dto.PlannedEnd;
        entity.Status = dto.Status;
        entity.CompanyId = dto.CompanyId;
        entity.UpdatedAt = DateTime.UtcNow;

        await _workPlanRepository.UpdateAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _workPlanRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _workPlanRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static WorkPlanDto MapToDto(WorkPlan e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        CraneId = e.CraneId,
        CraneName = e.Crane?.Name,
        ConstructionSiteId = e.ConstructionSiteId,
        ConstructionSiteName = e.ConstructionSite?.Name,
        PlannedStart = e.PlannedStart,
        PlannedEnd = e.PlannedEnd,
        Status = e.Status,
        CompanyId = e.CompanyId,
        CompanyName = e.Company?.Name,
        CreatedAt = e.CreatedAt
    };
}
