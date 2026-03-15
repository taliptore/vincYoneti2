using CraneManagementSystem.Application.DTOs.Appointment;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class AppointmentService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentService(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<AppointmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<AppointmentDto>> GetAllAsync(Guid? filterByUserId, Guid? filterByCompanyId, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        var result = list.Select(MapToDto).ToList();
        if (filterByCompanyId.HasValue)
            result = result.Where(x => x.CompanyId == filterByCompanyId).ToList();
        return result;
    }

    public async Task<AppointmentDto> CreateAsync(AppointmentCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new Appointment
        {
            Id = Guid.NewGuid(),
            CustomerName = dto.CustomerName,
            Email = dto.Email,
            Phone = dto.Phone,
            PreferredDate = dto.PreferredDate,
            Notes = dto.Notes,
            Status = dto.Status ?? "Beklemede",
            CompanyId = dto.CompanyId,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<AppointmentDto?> UpdateAsync(Guid id, AppointmentUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.CustomerName = dto.CustomerName;
        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
        entity.PreferredDate = dto.PreferredDate;
        entity.Notes = dto.Notes;
        entity.Status = dto.Status ?? entity.Status;
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

    private static AppointmentDto MapToDto(Appointment e) => new()
    {
        Id = e.Id,
        CustomerName = e.CustomerName,
        Email = e.Email,
        Phone = e.Phone,
        PreferredDate = e.PreferredDate,
        Notes = e.Notes,
        Status = e.Status,
        CompanyId = e.CompanyId,
        CompanyName = e.Company?.Name,
        CreatedAt = e.CreatedAt
    };
}
