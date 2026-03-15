namespace CraneManagementSystem.Web.ViewModels;

public class SliderItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortText { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SliderEditViewModel
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortText { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}

public class GalleryItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GalleryItemEditViewModel
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}

public class NewsItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Body { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class NewsEditViewModel
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Body { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
}

public class AboutEditViewModel
{
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
}

public class PagedResultViewModel<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
