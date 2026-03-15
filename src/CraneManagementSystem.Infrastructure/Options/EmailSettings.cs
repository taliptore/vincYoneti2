namespace CraneManagementSystem.Infrastructure.Options;

public class EmailSettings
{
    public const string SectionName = "Email";
    public string SmtpHost { get; set; } = "localhost";
    public int SmtpPort { get; set; } = 25;
    public bool UseSsl { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string FromAddress { get; set; } = "noreply@torevinc.com";
    public string FromName { get; set; } = "TORE VINC";
}
