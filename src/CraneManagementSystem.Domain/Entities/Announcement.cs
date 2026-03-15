namespace CraneManagementSystem.Domain.Entities;

public class Announcement : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public bool IsPinned { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
