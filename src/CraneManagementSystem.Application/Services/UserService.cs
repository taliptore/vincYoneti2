using CraneManagementSystem.Application.DTOs.User;
using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain;
using CraneManagementSystem.Domain.Entities;

namespace CraneManagementSystem.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _userRepository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _userRepository.GetAllAsync(cancellationToken);
        return list.Select(MapToDto).ToList();
    }

    public async Task<UserDto> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FullName = dto.FullName,
            Role = (UserRole)dto.Role,
            CompanyId = dto.CompanyId,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        await _userRepository.AddAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<UserDto?> UpdateAsync(Guid id, UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.FullName = dto.FullName;
        entity.Role = (UserRole)dto.Role;
        entity.CompanyId = dto.CompanyId;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        if (!string.IsNullOrEmpty(dto.NewPassword))
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        await _userRepository.UpdateAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;
        await _userRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }

    private static UserDto MapToDto(User e) => new()
    {
        Id = e.Id,
        Email = e.Email,
        FullName = e.FullName,
        Role = (int)e.Role,
        CompanyId = e.CompanyId,
        CompanyName = e.Company?.Name,
        IsActive = e.IsActive,
        CreatedAt = e.CreatedAt
    };
}
