using CraneManagementSystem.Application.DTOs.Crane;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class CraneService
{
    private readonly ICraneRepository _craneRepository;

    public CraneService(ICraneRepository craneRepository)
    {
        _craneRepository = craneRepository;
    }

    public async Task<CraneDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _craneRepository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<CraneDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _craneRepository.GetAllAsync(cancellationToken);
        return list.Select(MapToDto).ToList();
    }

    public async Task<CraneDto> CreateAsync(CraneCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new Crane
        {
            Id = Guid.NewGuid(),
            Code = dto.Code,
            Name = dto.Name,
            Type = dto.Type,
            Location = dto.Location,
            Status = dto.Status,
            ConstructionSiteId = dto.ConstructionSiteId,
            AssignedOperatorId = dto.AssignedOperatorId,
            CreatedAt = DateTime.UtcNow
        };
        await _craneRepository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<CraneDto?> UpdateAsync(Guid id, CraneUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _craneRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Code = dto.Code;
        entity.Name = dto.Name;
        entity.Type = dto.Type;
        entity.Location = dto.Location;
        entity.Status = dto.Status;
        entity.ConstructionSiteId = dto.ConstructionSiteId;
        entity.AssignedOperatorId = dto.AssignedOperatorId;
        entity.UpdatedAt = DateTime.UtcNow;

        await _craneRepository.UpdateAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _craneRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _craneRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static CraneDto MapToDto(Crane e) => new()
    {
        Id = e.Id,
        Code = e.Code,
        Name = e.Name,
        Type = e.Type,
        Location = e.Location,
        Status = e.Status,
        ConstructionSiteId = e.ConstructionSiteId,
        ConstructionSiteName = e.ConstructionSite?.Name,
        AssignedOperatorId = e.AssignedOperatorId,
        AssignedOperatorName = e.AssignedOperator?.FullName,
        CreatedAt = e.CreatedAt
    };
}
