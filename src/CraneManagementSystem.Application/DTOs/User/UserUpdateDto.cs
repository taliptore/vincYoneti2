namespace CraneManagementSystem.Application.DTOs.User;

public class UserUpdateDto
{
    public string FullName { get; set; } = string.Empty;
    public int Role { get; set; }
    public Guid? CompanyId { get; set; }
    public bool IsActive { get; set; }
    public string? NewPassword { get; set; }
}
