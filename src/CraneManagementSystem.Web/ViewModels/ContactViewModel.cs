using System.ComponentModel.DataAnnotations;

namespace CraneManagementSystem.Web.ViewModels;

public class ContactFormViewModel
{
    [Required(ErrorMessage = "Ad Soyad gereklidir")]
    [Display(Name = "Ad Soyad")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta gereklidir")]
    [EmailAddress]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Telefon")]
    public string? Phone { get; set; }

    [Display(Name = "Konu")]
    public string? Subject { get; set; }

    [Display(Name = "Mesaj")]
    public string? Message { get; set; }
}
