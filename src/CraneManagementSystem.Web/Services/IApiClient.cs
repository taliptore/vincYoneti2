namespace CraneManagementSystem.Web.Services;

public interface IApiClient
{
    Task<T?> GetAsync<T>(string path, CancellationToken cancellationToken = default) where T : class;
    Task<T?> PostAsync<T>(string path, object? body, CancellationToken cancellationToken = default) where T : class;
    Task<bool> PostAsync(string path, object? body, CancellationToken cancellationToken = default);
    Task<T?> PutAsync<T>(string path, object? body, CancellationToken cancellationToken = default) where T : class;
    Task<bool> PutAsync(string path, object? body, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string path, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> GetResponseAsync(string path, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> PostResponseAsync(string path, object? body, CancellationToken cancellationToken = default);
    void SetBearerToken(string? token);
    string? GetStoredToken();
}
