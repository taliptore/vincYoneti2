using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CraneManagementSystem.API.IntegrationTests;

/// <summary>
/// Integration testler API'nin appsettings'ini kullanır (aynı veritabanı).
/// API en az bir kez çalıştırılmış olmalı ki seed kullanıcıları oluşsun.
/// </summary>
public class WebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
    }
}
