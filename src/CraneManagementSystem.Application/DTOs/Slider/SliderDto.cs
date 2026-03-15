namespace CraneManagementSystem.Application.DTOs.Slider;

public class SliderDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortText { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
