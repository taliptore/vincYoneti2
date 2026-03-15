namespace CraneManagementSystem.Application.DTOs.SystemSetting;

public class SystemSettingCreateOrUpdateDto
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
}
