using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace CraneManagementSystem.API.IntegrationTests;

public class ExceptionMiddlewareTests : IClassFixture<WebApplicationFactory>
{
    private readonly HttpClient _client;

    public ExceptionMiddlewareTests(WebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCompanyNonExistentId_Returns404()
    {
        string token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/companies/00000000-0000-0000-0000-000000000001");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Not Found", content);
    }

    [Fact]
    public async Task ResponseContentType_IsApplicationJson()
    {
        string token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/companies/00000000-0000-0000-0000-000000000001");
        var mediaType = response.Content.Headers.ContentType?.MediaType ?? "";
        Assert.True(mediaType == "application/json" || mediaType == "application/problem+json", $"Expected JSON content type, got: {mediaType}");
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var loginPayload = new { Email = "admin@torevinc.com", Password = "Admin123!" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginPayload);
        response.EnsureSuccessStatusCode();
        string body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("token").GetString() ?? string.Empty;
    }
}
