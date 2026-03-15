using CraneManagementSystem.Application.DTOs.Home;
using CraneManagementSystem.Application.Interfaces;

namespace CraneManagementSystem.Application.Services;

public class HomePageService : IHomePageService
{
    private readonly INewsRepository _newsRepository;
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly ISystemSettingRepository _systemSettingRepository;

    public HomePageService(
        INewsRepository newsRepository,
        IAnnouncementRepository announcementRepository,
        ISystemSettingRepository systemSettingRepository)
    {
        _newsRepository = newsRepository;
        _announcementRepository = announcementRepository;
        _systemSettingRepository = systemSettingRepository;
    }

    public async Task<HomePageDto> GetHomePageAsync(CancellationToken cancellationToken = default)
    {
        var newsList = await _newsRepository.GetAllAsync(cancellationToken);
        var announcementList = await _announcementRepository.GetAllAsync(cancellationToken);

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

        var heroTitle = (await _systemSettingRepository.GetByKeyAsync("Home.HeroTitle", cancellationToken))?.Value;
        var heroSubtitle = (await _systemSettingRepository.GetByKeyAsync("Home.HeroSubtitle", cancellationToken))?.Value;

        return new HomePageDto
        {
            LatestNews = publishedNews,
            PinnedAnnouncements = activeAnnouncements,
            HeroTitle = heroTitle,
            HeroSubtitle = heroSubtitle
        };
    }
}
