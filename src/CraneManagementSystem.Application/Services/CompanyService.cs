using CraneManagementSystem.Application.DTOs.Company;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class CompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<CompanyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _companyRepository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _companyRepository.GetAllAsync(cancellationToken);
        return list.Select(MapToDto).ToList();
    }

    public async Task<CompanyDto> CreateAsync(CompanyCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new Company
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            TaxNumber = dto.TaxNumber,
            Address = dto.Address,
            Phone = dto.Phone,
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow
        };
        await _companyRepository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<CompanyDto?> UpdateAsync(Guid id, CompanyUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _companyRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.Name = dto.Name;
        entity.TaxNumber = dto.TaxNumber;
        entity.Address = dto.Address;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.UpdatedAt = DateTime.UtcNow;

        await _companyRepository.UpdateAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _companyRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _companyRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static CompanyDto MapToDto(Company e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        TaxNumber = e.TaxNumber,
        Address = e.Address,
        Phone = e.Phone,
        Email = e.Email,
        CreatedAt = e.CreatedAt
    };
}
