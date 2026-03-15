using CraneManagementSystem.Domain;

namespace CraneManagementSystem.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Guid? CompanyId { get; set; }
    public bool IsActive { get; set; } = true;

    public Company? Company { get; set; }
}
