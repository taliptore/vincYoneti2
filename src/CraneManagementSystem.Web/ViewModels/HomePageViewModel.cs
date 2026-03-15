namespace CraneManagementSystem.Web.ViewModels;

public class HomePageViewModel
{
    public List<HomeNewsItemViewModel> LatestNews { get; set; } = new();
    public List<HomeAnnouncementItemViewModel> PinnedAnnouncements { get; set; } = new();
    public string? HeroTitle { get; set; }
    public string? HeroSubtitle { get; set; }
}

public class HomeNewsItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? PublishedAt { get; set; }
}

public class HomeAnnouncementItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
