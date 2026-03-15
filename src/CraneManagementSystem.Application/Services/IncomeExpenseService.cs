using CraneManagementSystem.Application.DTOs.IncomeExpense;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class IncomeExpenseService
{
    private readonly IIncomeExpenseRepository _repository;

    public IncomeExpenseService(IIncomeExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IncomeExpenseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<IncomeExpenseDto>> GetAllAsync(Guid? filterByCompanyId, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (filterByCompanyId.HasValue)
            result = result.Where(x => x.CompanyId == filterByCompanyId).ToList();
        return result;
    }

    public async Task<IncomeExpenseDto> CreateAsync(IncomeExpenseCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new IncomeExpense
        {
            Id = Guid.NewGuid(),
            Type = (IncomeExpenseType)dto.Type,
            Category = dto.Category,
            Amount = dto.Amount,
            Date = dto.Date,
            Description = dto.Description,
            ReferenceType = (ReferenceType)dto.ReferenceType,
            ReferenceId = dto.ReferenceId,
            CompanyId = dto.CompanyId,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<IncomeExpenseDto?> UpdateAsync(Guid id, IncomeExpenseUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Type = (IncomeExpenseType)dto.Type;
        entity.Category = dto.Category;
        entity.Amount = dto.Amount;
        entity.Date = dto.Date;
        entity.Description = dto.Description;
        entity.ReferenceType = (ReferenceType)dto.ReferenceType;
        entity.ReferenceId = dto.ReferenceId;
        entity.CompanyId = dto.CompanyId;
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

    private static IncomeExpenseDto MapToDto(IncomeExpense e) => new()
    {
        Id = e.Id,
        Type = (int)e.Type,
        Category = e.Category,
        Amount = e.Amount,
        Date = e.Date,
        Description = e.Description,
        ReferenceType = (int)e.ReferenceType,
        ReferenceId = e.ReferenceId,
        CompanyId = e.CompanyId,
        CompanyName = e.Company?.Name,
        CreatedAt = e.CreatedAt
    };
}
