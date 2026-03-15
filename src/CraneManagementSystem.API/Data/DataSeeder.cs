using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain;
using CraneManagementSystem.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CraneManagementSystem.API.Data;

public static class DataSeeder
{
    public static async Task EnsureAdminUserAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var companyRepo = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        await EnsureSeedUserAsync(userRepo, companyRepo, config, logger, UserRole.Admin, "Seed:Admin", null, cancellationToken);
        await EnsureSeedUserAsync(userRepo, companyRepo, config, logger, UserRole.Muhasebe, "Seed:Muhasebe", null, cancellationToken);
        await EnsureSeedUserAsync(userRepo, companyRepo, config, logger, UserRole.Operatör, "Seed:Operatör", null, cancellationToken);
        await EnsureSeedUserAsync(userRepo, companyRepo, config, logger, UserRole.Firma, "Seed:Firma", config["Seed:Firma:CompanyName"], cancellationToken);
    }

    private static async Task EnsureSeedUserAsync(
        IUserRepository userRepo,
        ICompanyRepository companyRepo,
        IConfiguration config,
        ILogger logger,
        UserRole role,
        string configPrefix,
        string? companyNameForFirma,
        CancellationToken cancellationToken)
    {
        var email = config[$"{configPrefix}:Email"] ?? Environment.GetEnvironmentVariable($"SEED_{role}_EMAIL");
        var password = config[$"{configPrefix}:Password"] ?? Environment.GetEnvironmentVariable($"SEED_{role}_PASSWORD");
        var fullName = config[$"{configPrefix}:FullName"] ?? Environment.GetEnvironmentVariable($"SEED_{role}_FULLNAME") ?? role.ToString();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Seed: {Role} için Email veya Password ayarlanmamış; atlanıyor.", role);
            return;
        }

        if (await userRepo.GetByEmailAsync(email, cancellationToken) != null)
        {
            logger.LogDebug("Seed kullanıcı zaten mevcut: {Email}", email);
            return;
        }

        Guid? companyId = null;
        if (role == UserRole.Firma && !string.IsNullOrWhiteSpace(companyNameForFirma))
        {
            var companies = await companyRepo.GetAllAsync(cancellationToken);
            var seedCompany = companies.FirstOrDefault(c => c.Name == companyNameForFirma);
            if (seedCompany == null)
            {
                seedCompany = new Company
                {
                    Id = Guid.NewGuid(),
                    Name = companyNameForFirma,
                    CreatedAt = DateTime.UtcNow
                };
                await companyRepo.AddAsync(seedCompany, cancellationToken);
                logger.LogInformation("Seed firma oluşturuldu: {Name}", companyNameForFirma);
            }
            companyId = seedCompany.Id;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            FullName = fullName,
            Role = role,
            CompanyId = companyId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await userRepo.AddAsync(user, cancellationToken);
        logger.LogInformation("Seed kullanıcı oluşturuldu: {Email} ({Role})", email, role);
    }
}
