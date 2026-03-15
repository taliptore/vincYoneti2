namespace CraneManagementSystem.Application.DTOs.User;

public class UserCreateDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int Role { get; set; }
    public Guid? CompanyId { get; set; }
    public bool IsActive { get; set; } = true;
}
