using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CraneManagementSystem.Web.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string AuthTokenKey = "AuthToken";
    private const string AuthRoleKey = "AuthRole";
    private const string AuthEmailKey = "AuthEmail";
    private const string AuthNameKey = "AuthName";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private string? GetTokenFromCookie()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.Request.Cookies[AuthTokenKey];
    }

    public string? GetStoredToken() => GetTokenFromCookie();

    private void ApplyAuthHeader()
    {
        var token = GetTokenFromCookie();
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        else
            _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public void SetBearerToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = null;
        else
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<T?> GetAsync<T>(string path, CancellationToken cancellationToken = default) where T : class
    {
        ApplyAuthHeader();
        var response = await _httpClient.GetAsync(path, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task<HttpResponseMessage> GetResponseAsync(string path, CancellationToken cancellationToken = default)
    {
        ApplyAuthHeader();
        return await _httpClient.GetAsync(path, cancellationToken);
    }

    public async Task<T?> PostAsync<T>(string path, object? body, CancellationToken cancellationToken = default) where T : class
    {
        ApplyAuthHeader();
        var content = body == null ? null : new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        var response = content == null
            ? await _httpClient.PostAsync(path, null, cancellationToken)
            : await _httpClient.PostAsync(path, content, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task<bool> PostAsync(string path, object? body, CancellationToken cancellationToken = default)
    {
        ApplyAuthHeader();
        var content = body == null ? null : new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        var response = content == null
            ? await _httpClient.PostAsync(path, null, cancellationToken)
            : await _httpClient.PostAsync(path, content, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<HttpResponseMessage> PostResponseAsync(string path, object? body, CancellationToken cancellationToken = default)
    {
        ApplyAuthHeader();
        var content = body == null ? null : new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        return content == null
            ? await _httpClient.PostAsync(path, null, cancellationToken)
            : await _httpClient.PostAsync(path, content, cancellationToken);
    }

    public static void SetAuthCookies(HttpContext context, string token, string email, string fullName, int role)
    {
        var roleName = role switch { 0 => "Admin", 1 => "Muhasebe", 2 => "Operatör", 3 => "Firma", _ => "Firma" };
        context.Response.Cookies.Append(AuthTokenKey, token, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax, Path = "/", MaxAge = TimeSpan.FromDays(1) });
        context.Response.Cookies.Append(AuthEmailKey, email, new CookieOptions { Path = "/", MaxAge = TimeSpan.FromDays(1) });
        context.Response.Cookies.Append(AuthNameKey, fullName ?? "", new CookieOptions { Path = "/", MaxAge = TimeSpan.FromDays(1) });
        context.Response.Cookies.Append(AuthRoleKey, roleName, new CookieOptions { Path = "/", MaxAge = TimeSpan.FromDays(1) });
    }

    public static void ClearAuthCookies(HttpContext context)
    {
        foreach (var key in new[] { AuthTokenKey, AuthRoleKey, AuthEmailKey, AuthNameKey })
            context.Response.Cookies.Delete(key, new CookieOptions { Path = "/" });
    }

    public static string? GetRoleFromCookie(HttpContext context) => context.Request.Cookies[AuthRoleKey];
    public static string? GetEmailFromCookie(HttpContext context) => context.Request.Cookies[AuthEmailKey];
    public static string? GetNameFromCookie(HttpContext context) => context.Request.Cookies[AuthNameKey];
}
