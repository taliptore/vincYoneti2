using CraneManagementSystem.Application.DTOs.ProgressPayment;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class ProgressPaymentService
{
    private readonly IProgressPaymentRepository _progressPaymentRepository;

    public ProgressPaymentService(IProgressPaymentRepository progressPaymentRepository)
    {
        _progressPaymentRepository = progressPaymentRepository;
    }

    public async Task<ProgressPaymentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _progressPaymentRepository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<ProgressPaymentDto>> GetAllAsync(Guid? filterByCompanyId, CancellationToken cancellationToken = default)
    {
        var list = await _progressPaymentRepository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (filterByCompanyId.HasValue)
            result = result.Where(x => x.CompanyId == filterByCompanyId).ToList();
        return result;
    }

    public async Task<ProgressPaymentDto> CreateAsync(ProgressPaymentCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new ProgressPayment
        {
            Id = Guid.NewGuid(),
            WorkPlanId = dto.WorkPlanId,
            CompanyId = dto.CompanyId,
            Amount = dto.Amount,
            Period = dto.Period,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow
        };
        await _progressPaymentRepository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<ProgressPaymentDto?> UpdateAsync(Guid id, ProgressPaymentUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _progressPaymentRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.WorkPlanId = dto.WorkPlanId;
        entity.CompanyId = dto.CompanyId;
        entity.Amount = dto.Amount;
        entity.Period = dto.Period;
        entity.Status = dto.Status;
        entity.UpdatedAt = DateTime.UtcNow;

        await _progressPaymentRepository.UpdateAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _progressPaymentRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _progressPaymentRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static ProgressPaymentDto MapToDto(ProgressPayment e) => new()
    {
        Id = e.Id,
        WorkPlanId = e.WorkPlanId,
        WorkPlanTitle = e.WorkPlan?.Title,
        CompanyId = e.CompanyId,
        CompanyName = e.Company?.Name,
        Amount = e.Amount,
        Period = e.Period,
        Status = e.Status,
        CreatedAt = e.CreatedAt
    };
}
