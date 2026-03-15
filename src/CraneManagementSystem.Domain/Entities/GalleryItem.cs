namespace CraneManagementSystem.Domain.Entities;

public class GalleryItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
