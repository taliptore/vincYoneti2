namespace CraneManagementSystem.Application.DTOs.Home;

public class HomePageDto
{
    public List<HomeNewsItemDto> LatestNews { get; set; } = new();
    public List<HomeAnnouncementItemDto> PinnedAnnouncements { get; set; } = new();
    public string? HeroTitle { get; set; }
    public string? HeroSubtitle { get; set; }
}

public class HomeNewsItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? PublishedAt { get; set; }
}

public class HomeAnnouncementItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
