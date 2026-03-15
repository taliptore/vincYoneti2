namespace CraneManagementSystem.Application.DTOs.Home;

public class HomePageDto
{
    public List<HomeSliderItemDto> Sliders { get; set; } = new();
    public List<HomeNewsItemDto> LatestNews { get; set; } = new();
    public List<HomeAnnouncementItemDto> PinnedAnnouncements { get; set; } = new();
    public List<HomeGalleryItemDto> GalleryItems { get; set; } = new();
    public string? AboutContent { get; set; }
    public string? AboutImageUrl { get; set; }
    public string? ContactAddress { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? HeroTitle { get; set; }
    public string? HeroSubtitle { get; set; }
    public string? LogoUrl { get; set; }
    public string? MapEmbedUrl { get; set; }
    public string? SocialFacebook { get; set; }
    public string? SocialInstagram { get; set; }
    public string? SocialLinkedIn { get; set; }
    public List<string> ReferansLogos { get; set; } = new();
}

public class HomeSliderItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortText { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}

public class HomeGalleryItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
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
