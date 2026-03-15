using System.ComponentModel.DataAnnotations;

namespace CraneManagementSystem.Web.ViewModels;

public class AppointmentFormViewModel
{
    [Required(ErrorMessage = "Ad Soyad gereklidir")]
    [Display(Name = "Ad Soyad")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta gereklidir")]
    [EmailAddress]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Telefon")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Tercih edilen tarih gereklidir")]
    [Display(Name = "Tercih edilen tarih")]
    [DataType(DataType.Date)]
    public DateTime PreferredDate { get; set; } = DateTime.Today;

    [Display(Name = "Notlar")]
    public string? Notes { get; set; }
}
