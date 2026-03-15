using CraneManagementSystem.Application.Interfaces;
using CraneManagementSystem.Domain;
using CraneManagementSystem.Domain.Entities;
using CraneManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;
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

    public static async Task EnsureRolesAndMenusAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        var roleNames = new[] { "Admin", "Muhasebe", "Operatör", "Firma" };
        var roleIds = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);

        foreach (var name in roleNames)
        {
            var existing = await db.Roles.FirstOrDefaultAsync(r => r.RoleName == name, cancellationToken);
            if (existing != null)
            {
                roleIds[name] = existing.Id;
                continue;
            }
            var role = new Role
            {
                Id = Guid.NewGuid(),
                RoleName = name,
                Description = name + " rolü",
                CreatedAt = DateTime.UtcNow
            };
            db.Roles.Add(role);
            roleIds[name] = role.Id;
            logger.LogInformation("Seed rol oluşturuldu: {RoleName}", name);
        }
        await db.SaveChangesAsync(cancellationToken);

        if (await db.Menus.AnyAsync(cancellationToken))
        {
            logger.LogDebug("Menüler zaten mevcut; menü seed atlanıyor.");
            return;
        }

        var now = DateTime.UtcNow;
        var adminId = roleIds["Admin"];
        var menus = new List<Menu>();

        Menu NewMenu(string title, string? icon, string? route, Guid? parentId, int orderNo, string? moduleName = null)
        {
            var m = new Menu
            {
                Id = Guid.NewGuid(),
                Title = title,
                Icon = icon ?? "folder",
                Route = route,
                ParentId = parentId,
                OrderNo = orderNo,
                ModuleName = moduleName ?? title.Replace(" ", ""),
                IsActive = true,
                CreatedAt = now
            };
            menus.Add(m);
            return m;
        }

        var m1 = NewMenu("Dashboard", "speed", "/dashboard", null, 1);
        NewMenu("Genel Durum", "assessment", "/dashboard/overview", m1.Id, 1);
        NewMenu("Günlük İşler", "today", "/dashboard/daily", m1.Id, 2);

        var m2 = NewMenu("Vinç Yönetimi", "precision_manufacturing", "/cranes", null, 2);
        NewMenu("Vinç Listesi", "list", "/cranes", m2.Id, 1);
        NewMenu("Vinç Ekle", "add", "/cranes/new", m2.Id, 2);
        NewMenu("Vinç Bakım Takibi", "build", "/cranes/maintenance", m2.Id, 3);
        NewMenu("Vinç Kullanım Geçmişi", "history", "/cranes/history", m2.Id, 4);

        var m3 = NewMenu("Operasyon Yönetimi", "assignment", "/operations", null, 3);
        NewMenu("Kiralama Listesi", "list", "/operations/rentals", m3.Id, 1);
        NewMenu("Yeni Kiralama", "add", "/operations/rentals/new", m3.Id, 2);
        NewMenu("Görev Planlama", "schedule", "/work-plans", m3.Id, 3);
        NewMenu("Operasyon Takibi", "track_changes", "/operations/tracking", m3.Id, 4);

        var m4 = NewMenu("Müşteri Yönetimi", "business", "/companies", null, 4);
        NewMenu("Müşteri Listesi", "list", "/companies", m4.Id, 1);
        NewMenu("Müşteri Ekle", "add", "/companies/new", m4.Id, 2);
        NewMenu("Müşteri Projeleri", "folder", "/companies/projects", m4.Id, 3);

        var m5 = NewMenu("Teklif Yönetimi", "request_quote", "/quotes", null, 5);
        NewMenu("Teklif Listesi", "list", "/quotes", m5.Id, 1);
        NewMenu("Yeni Teklif", "add", "/quotes/new", m5.Id, 2);
        NewMenu("Teklif Onayları", "approval", "/quotes/approvals", m5.Id, 3);

        var m6 = NewMenu("Fatura ve Muhasebe", "receipt", "/finance", null, 6);
        NewMenu("Fatura Listesi", "list", "/finance/invoices", m6.Id, 1);
        NewMenu("Gelirler", "trending_up", "/income-expense", m6.Id, 2);
        NewMenu("Giderler", "trending_down", "/income-expense", m6.Id, 3);

        var m7 = NewMenu("Bakım Yönetimi", "build_circle", "/maintenance", null, 7);
        NewMenu("Bakım Planı", "schedule", "/maintenance/plan", m7.Id, 1);
        NewMenu("Arıza Kayıtları", "warning", "/maintenance/records", m7.Id, 2);
        NewMenu("Servis Geçmişi", "history", "/maintenance/history", m7.Id, 3);

        var m8 = NewMenu("Operatör Yönetimi", "people", "/operators", null, 8);
        NewMenu("Operatör Listesi", "list", "/operators", m8.Id, 1);
        NewMenu("Operatör Ekle", "person_add", "/operators/new", m8.Id, 2);
        NewMenu("Operatör Görevleri", "assignment_ind", "/operators/assignments", m8.Id, 3);

        var m9 = NewMenu("Raporlama", "summarize", "/reports", null, 9);
        NewMenu("Vinç Kullanım Raporu", "precision_manufacturing", "/reports/crane-usage", m9.Id, 1);
        NewMenu("Kiralama Raporu", "assignment", "/reports/rentals", m9.Id, 2);
        NewMenu("Gelir Raporu", "trending_up", "/reports/income", m9.Id, 3);
        NewMenu("Operatör Performans Raporu", "person", "/reports/operator", m9.Id, 4);

        var m10 = NewMenu("CMS Yönetimi", "web", "/cms", null, 10);
        NewMenu("Slider", "view_carousel", "/cms/sliders", m10.Id, 1);
        NewMenu("Haber", "article", "/cms/news", m10.Id, 2);
        NewMenu("Galeri", "photo_library", "/cms/gallery", m10.Id, 3);
        NewMenu("Hizmetler", "miscellaneous_services", "/cms/services", m10.Id, 4);
        NewMenu("Referanslar", "stars", "/cms/references", m10.Id, 5);
        NewMenu("Sayfalar", "description", "/cms/pages", m10.Id, 6);
        NewMenu("İletişim Bilgileri", "contact_mail", "/cms/contact", m10.Id, 7);

        var m11 = NewMenu("Sistem Yönetimi", "settings_applications", "/system", null, 11);
        NewMenu("Kullanıcı Yönetimi", "people", "/users", m11.Id, 1);
        NewMenu("Rol Yönetimi", "admin_panel_settings", "/system/roles", m11.Id, 2);
        NewMenu("Yetki Yönetimi", "security", "/system/permissions", m11.Id, 3);
        NewMenu("Menü Yönetimi", "menu", "/system/menus", m11.Id, 4);
        NewMenu("Log Kayıtları", "list_alt", "/system/logs", m11.Id, 5);

        var m12 = NewMenu("Ayarlar", "tune", "/settings", null, 12);
        NewMenu("Genel Ayarlar", "settings", "/settings/general", m12.Id, 1);
        NewMenu("SMS Ayarları", "sms", "/settings/sms", m12.Id, 2);
        NewMenu("Email Ayarları", "email", "/settings/email", m12.Id, 3);
        NewMenu("API Ayarları", "api", "/settings/api", m12.Id, 4);

        db.Menus.AddRange(menus);
        await db.SaveChangesAsync(cancellationToken);

        foreach (var menu in menus)
        {
            db.MenuRoles.Add(new MenuRole { MenuId = menu.Id, RoleId = adminId });
        }
        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Seed menüler ve Admin rol atamaları oluşturuldu.");
    }
}
