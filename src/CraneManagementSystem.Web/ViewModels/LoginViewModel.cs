using System.ComponentModel.DataAnnotations;

namespace CraneManagementSystem.Web.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "E-posta gereklidir")]
    [EmailAddress]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre gereklidir")]
    [DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseViewModel
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int Role { get; set; }
    public Guid? CompanyId { get; set; }
}
