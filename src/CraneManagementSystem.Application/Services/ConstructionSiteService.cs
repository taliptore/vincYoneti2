using CraneManagementSystem.Application.DTOs.ConstructionSite;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class ConstructionSiteService
{
    private readonly IConstructionSiteRepository _constructionSiteRepository;

    public ConstructionSiteService(IConstructionSiteRepository constructionSiteRepository)
    {
        _constructionSiteRepository = constructionSiteRepository;
    }

    public async Task<ConstructionSiteDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _constructionSiteRepository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<ConstructionSiteDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _constructionSiteRepository.GetAllAsync(cancellationToken);
        return list.Select(MapToDto).ToList();
    }

    public async Task<ConstructionSiteDto> CreateAsync(ConstructionSiteCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new ConstructionSite
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Address = dto.Address,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CreatedAt = DateTime.UtcNow
        };
        await _constructionSiteRepository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<ConstructionSiteDto?> UpdateAsync(Guid id, ConstructionSiteUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _constructionSiteRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Name = dto.Name;
        entity.Address = dto.Address;
        entity.StartDate = dto.StartDate;
        entity.EndDate = dto.EndDate;
        entity.UpdatedAt = DateTime.UtcNow;

        await _constructionSiteRepository.UpdateAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _constructionSiteRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _constructionSiteRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static ConstructionSiteDto MapToDto(ConstructionSite e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Address = e.Address,
        StartDate = e.StartDate,
        EndDate = e.EndDate,
        CreatedAt = e.CreatedAt
    };
}
