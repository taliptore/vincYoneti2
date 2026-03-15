using CraneManagementSystem.Application.DTOs.Home;
using CraneManagementSystem.Application.Interfaces;

namespace CraneManagementSystem.Application.Services;

public class HomePageService : IHomePageService
{
    private readonly INewsRepository _newsRepository;
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly ISystemSettingRepository _systemSettingRepository;
    private readonly ISliderRepository _sliderRepository;
    private readonly IGalleryItemRepository _galleryItemRepository;

    public HomePageService(
        INewsRepository newsRepository,
        IAnnouncementRepository announcementRepository,
        ISystemSettingRepository systemSettingRepository,
        ISliderRepository sliderRepository,
        IGalleryItemRepository galleryItemRepository)
    {
        _newsRepository = newsRepository;
        _announcementRepository = announcementRepository;
        _systemSettingRepository = systemSettingRepository;
        _sliderRepository = sliderRepository;
        _galleryItemRepository = galleryItemRepository;
    }

    public async Task<HomePageDto> GetHomePageAsync(CancellationToken cancellationToken = default)
    {
        var newsList = await _newsRepository.GetAllAsync(cancellationToken);
        var announcementList = await _announcementRepository.GetAllAsync(cancellationToken);
        var sliders = await _sliderRepository.GetAllAsync(cancellationToken);
        var galleryItems = await _galleryItemRepository.GetAllAsync(cancellationToken);

        var activeSliders = sliders
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .Select(x => new HomeSliderItemDto
            {
                Id = x.Id,
                Title = x.Title,
                ShortText = x.ShortText,
                ImageUrl = x.ImageUrl,
                SortOrder = x.SortOrder
            })
            .ToList();

        var publishedNews = newsList
            .Where(x => x.IsPublished)
            .OrderByDescending(x => x.PublishedAt ?? x.CreatedAt)
            .Take(10)
            .Select(x => new HomeNewsItemDto
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                ImageUrl = x.ImageUrl,
                PublishedAt = x.PublishedAt
            })
            .ToList();

        var now = DateTime.UtcNow;
        var activeAnnouncements = announcementList
            .Where(x => x.IsPinned && x.StartDate <= now && (x.EndDate == null || x.EndDate >= now))
            .OrderByDescending(x => x.StartDate)
            .Take(5)
            .Select(x => new HomeAnnouncementItemDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            })
            .ToList();

        var activeGallery = galleryItems
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .Take(24)
            .Select(x => new HomeGalleryItemDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                SortOrder = x.SortOrder
            })
            .ToList();

        var heroTitle = (await _systemSettingRepository.GetByKeyAsync("Home.HeroTitle", cancellationToken))?.Value;
        var heroSubtitle = (await _systemSettingRepository.GetByKeyAsync("Home.HeroSubtitle", cancellationToken))?.Value;
        var aboutContent = (await _systemSettingRepository.GetByKeyAsync("AboutUs", cancellationToken))?.Value;
        var aboutImageUrl = (await _systemSettingRepository.GetByKeyAsync("AboutUsImageUrl", cancellationToken))?.Value;
        var contactAddress = (await _systemSettingRepository.GetByKeyAsync("ContactAddress", cancellationToken))?.Value;
        var contactPhone = (await _systemSettingRepository.GetByKeyAsync("ContactPhone", cancellationToken))?.Value;
        var contactEmail = (await _systemSettingRepository.GetByKeyAsync("ContactEmail", cancellationToken))?.Value;
        var logoUrl = (await _systemSettingRepository.GetByKeyAsync("LogoUrl", cancellationToken))?.Value;
        var mapEmbedUrl = (await _systemSettingRepository.GetByKeyAsync("MapEmbedUrl", cancellationToken))?.Value;
        var socialFacebook = (await _systemSettingRepository.GetByKeyAsync("SocialFacebook", cancellationToken))?.Value;
        var socialInstagram = (await _systemSettingRepository.GetByKeyAsync("SocialInstagram", cancellationToken))?.Value;
        var socialLinkedIn = (await _systemSettingRepository.GetByKeyAsync("SocialLinkedIn", cancellationToken))?.Value;
        var referansLogosRaw = (await _systemSettingRepository.GetByKeyAsync("ReferansLogos", cancellationToken))?.Value;
        var referansLogos = new List<string>();
        if (!string.IsNullOrWhiteSpace(referansLogosRaw))
        {
            try
            {
                referansLogos = System.Text.Json.JsonSerializer.Deserialize<List<string>>(referansLogosRaw) ?? referansLogos;
            }
            catch
            {
                referansLogos = referansLogosRaw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            }
        }

        return new HomePageDto
        {
            Sliders = activeSliders,
            LatestNews = publishedNews,
            PinnedAnnouncements = activeAnnouncements,
            GalleryItems = activeGallery,
            AboutContent = aboutContent,
            AboutImageUrl = aboutImageUrl,
            ContactAddress = contactAddress,
            ContactPhone = contactPhone,
            ContactEmail = contactEmail,
            HeroTitle = heroTitle,
            HeroSubtitle = heroSubtitle,
            LogoUrl = logoUrl,
            MapEmbedUrl = mapEmbedUrl,
            SocialFacebook = socialFacebook,
            SocialInstagram = socialInstagram,
            SocialLinkedIn = socialLinkedIn,
            ReferansLogos = referansLogos
        };
    }
}
