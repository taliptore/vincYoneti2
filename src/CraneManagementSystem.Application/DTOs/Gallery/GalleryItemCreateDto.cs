namespace CraneManagementSystem.Application.DTOs.Gallery;

public class GalleryItemCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
