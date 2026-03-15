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

    /// <summary>Örnek senaryo verileri: Firma, Şantiye, Vinç, İş planı, Gelir-gider, Bakım, Hakediş, Slider, Haber, Galeri, Ayarlar.</summary>
    public static async Task EnsureSampleDataAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        if (await db.Companies.AnyAsync(cancellationToken) && await db.Cranes.AnyAsync(cancellationToken))
        {
            logger.LogDebug("Örnek veri zaten mevcut; sample data seed atlanıyor.");
            return;
        }

        var now = DateTime.UtcNow;

        var companies = await db.Companies.ToListAsync(cancellationToken);
        if (companies.Count == 0)
        {
            companies = new List<Company>
            {
                new() { Id = Guid.NewGuid(), Name = "ABC İnşaat A.Ş.", TaxNumber = "1234567890", Address = "İstanbul", Phone = "0212 111 11 11", Email = "info@abcinsaat.com", CreatedAt = now },
                new() { Id = Guid.NewGuid(), Name = "XYZ Yapı Ltd.", TaxNumber = "9876543210", Address = "Ankara", Phone = "0312 222 22 22", Email = "iletisim@xyzyapi.com", CreatedAt = now }
            };
            db.Companies.AddRange(companies);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seed örnek firmalar eklendi.");
        }

        var constructionSites = await db.ConstructionSites.ToListAsync(cancellationToken);
        if (constructionSites.Count == 0)
        {
            constructionSites = new List<ConstructionSite>
            {
                new() { Id = Guid.NewGuid(), Name = "Kartal Proje Sahası", Address = "Kartal, İstanbul", StartDate = now.AddMonths(-2), CreatedAt = now },
                new() { Id = Guid.NewGuid(), Name = "Çankaya Şantiye", Address = "Çankaya, Ankara", StartDate = now.AddMonths(-1), CreatedAt = now }
            };
            db.ConstructionSites.AddRange(constructionSites);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seed örnek şantiyeler eklendi.");
        }

        var cranes = await db.Cranes.ToListAsync(cancellationToken);
        var operatorUser = await db.Users.FirstOrDefaultAsync(u => u.Role == UserRole.Operatör, cancellationToken);
        if (cranes.Count == 0)
        {
            cranes = new List<Crane>
            {
                new() { Id = Guid.NewGuid(), Code = "VNC-001", Name = "Mobil Vinç 1", Type = "Mobil", Location = "Kartal", Status = "Aktif", ConstructionSiteId = constructionSites[0].Id, AssignedOperatorId = operatorUser?.Id, CreatedAt = now },
                new() { Id = Guid.NewGuid(), Code = "VNC-002", Name = "Kule Vinç 1", Type = "Kule", Location = "Çankaya", Status = "Aktif", ConstructionSiteId = constructionSites.Count > 1 ? constructionSites[1].Id : null, CreatedAt = now }
            };
            db.Cranes.AddRange(cranes);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seed örnek vinçler eklendi.");
        }

        if (!await db.WorkPlans.AnyAsync(cancellationToken))
        {
            var workPlans = new List<WorkPlan>
            {
                new() { Id = Guid.NewGuid(), Title = "Kartal A Blok Vinç Hizmeti", CraneId = cranes[0].Id, ConstructionSiteId = constructionSites[0].Id, PlannedStart = now.AddDays(7), PlannedEnd = now.AddMonths(2), Status = "Planlandı", CompanyId = companies[0].Id, CreatedAt = now },
                new() { Id = Guid.NewGuid(), Title = "Çankaya Temel Çalışması", CraneId = cranes.Count > 1 ? cranes[1].Id : cranes[0].Id, ConstructionSiteId = constructionSites.Count > 1 ? constructionSites[1].Id : constructionSites[0].Id, PlannedStart = now.AddDays(14), PlannedEnd = now.AddMonths(3), Status = "Planlandı", CompanyId = companies.Count > 1 ? companies[1].Id : companies[0].Id, CreatedAt = now }
            };
            db.WorkPlans.AddRange(workPlans);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seed örnek iş planları eklendi.");

            if (!await db.ProgressPayments.AnyAsync(cancellationToken))
            {
                var wp = await db.WorkPlans.FirstAsync(cancellationToken);
                var progressPayments = new List<ProgressPayment>
                {
                    new() { Id = Guid.NewGuid(), WorkPlanId = wp.Id, CompanyId = companies[0].Id, Amount = 50000m, Period = "2024-01", Status = "Beklemede", CreatedAt = now }
                };
                db.ProgressPayments.AddRange(progressPayments);
                await db.SaveChangesAsync(cancellationToken);
                logger.LogInformation("Seed örnek hakediş eklendi.");
            }
        }

        if (!await db.IncomeExpenses.AnyAsync(cancellationToken))
        {
            var incomeExpenses = new List<IncomeExpense>
            {
                new() { Id = Guid.NewGuid(), Type = IncomeExpenseType.Gelir, Category = "Kiralama", Amount = 25000m, Date = now.AddDays(-10), Description = "Ocak kira geliri", ReferenceType = ReferenceType.Manuel, CompanyId = companies[0].Id, CreatedAt = now },
                new() { Id = Guid.NewGuid(), Type = IncomeExpenseType.Gider, Category = "Bakım", Amount = 5000m, Date = now.AddDays(-5), Description = "Vinç periyodik bakım", ReferenceType = ReferenceType.Manuel, CreatedAt = now }
            };
            db.IncomeExpenses.AddRange(incomeExpenses);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seed örnek gelir-gider eklendi.");
        }

        if (!await db.MaintenanceRecords.AnyAsync(cancellationToken) && cranes.Count > 0)
        {
            var maintenanceRecords = new List<MaintenanceRecord>
            {
                new() { Id = Guid.NewGuid(), CraneId = cranes[0].Id, MaintenanceDate = now.AddDays(-15), Description = "Periyodik kontrol", Type = "Periyodik", NextDueDate = now.AddMonths(3), CreatedAt = now }
            };
            db.MaintenanceRecords.AddRange(maintenanceRecords);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seed örnek bakım kaydı eklendi.");
        }

        if (!await db.Sliders.AnyAsync(cancellationToken))
        {
            var sliders = new List<Slider>
            {
                new() { Id = Guid.NewGuid(), Title = "TORE VINC Hoş Geldiniz", ShortText = "Profesyonel vinç hizmeti", ImageUrl = "https://picsum.photos/1200/400", SortOrder = 1, IsActive = true, CreatedAt = now },
                new() { Id = Guid.NewGuid(), Title = "Güvenilir ve Hızlı", ShortText = "7/24 destek", ImageUrl = null, SortOrder = 2, IsActive = true, CreatedAt = now }
            };
            db.Sliders.AddRange(sliders);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seed örnek slider eklendi.");
        }

        if (!await db.News.AnyAsync(cancellationToken))
        {
            var news = new List<News>
            {
                new() { Id = Guid.NewGuid(), Title = "Yeni Projelerimiz", Summary = "2024 projeleri açıklandı.", Body = "İçerik metni...", IsPublished = true, PublishedAt = now.AddDays(-3), CreatedAt = now },
                new() { Id = Guid.NewGuid(), Title = "Bakım Duyurusu", Summary = "Periyodik bakım programı.", Body = "Detaylar...", IsPublished = false, CreatedAt = now }
            };
            db.News.AddRange(news);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seed örnek haber eklendi.");
        }

        if (!await db.GalleryItems.AnyAsync(cancellationToken))
        {
            var galleryItems = new List<GalleryItem>
            {
                new() { Id = Guid.NewGuid(), Title = "Proje 1", ImageUrl = "https://picsum.photos/400/300", SortOrder = 1, IsActive = true, CreatedAt = now },
                new() { Id = Guid.NewGuid(), Title = "Proje 2", ImageUrl = null, SortOrder = 2, IsActive = true, CreatedAt = now }
            };
            db.GalleryItems.AddRange(galleryItems);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seed örnek galeri eklendi.");
        }

        var settingKeys = new[] { "SiteAdi", "LogoUrl", "SiteAciklama", "SmtpHost", "FromAddress" };
        foreach (var key in settingKeys)
        {
            if (await db.SystemSettings.AnyAsync(s => s.Key == key, cancellationToken))
                continue;
            db.SystemSettings.Add(new SystemSetting { Id = Guid.NewGuid(), Key = key, Value = key == "SiteAdi" ? "TORE VINC" : "", CreatedAt = now });
        }
        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Seed örnek sistem ayarları eklendi.");
    }

    /// <summary>Tüm tablolara en az 5'er örnek veri ekler (eksik olanları tamamlar).</summary>
    public static async Task EnsureFullSampleDataAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        const int targetCount = 5;
        var now = DateTime.UtcNow;

        var companies = await db.Companies.ToListAsync(cancellationToken);
        var needCompanies = targetCount - companies.Count;
        if (needCompanies > 0)
        {
            var names = new[] { "Demir İnşaat A.Ş.", "Beta Yapı Ltd.", "Güneş Proje A.Ş.", "Yıldız Taahhüt", "Kartal Mühendislik" };
            for (var i = 0; i < needCompanies; i++)
            {
                db.Companies.Add(new Company
                {
                    Id = Guid.NewGuid(),
                    Name = names[i % names.Length] + (i >= names.Length ? " " + (i + 1) : ""),
                    TaxNumber = (1000000000 + i).ToString(),
                    Address = i % 2 == 0 ? "İstanbul" : "Ankara",
                    Phone = "0212 555 " + (i + 10).ToString("00 00"),
                    Email = "info@firma" + (i + 1) + ".com",
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            companies = await db.Companies.ToListAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} firma eklendi.", needCompanies);
        }

        var constructionSites = await db.ConstructionSites.ToListAsync(cancellationToken);
        var needSites = targetCount - constructionSites.Count;
        if (needSites > 0)
        {
            var siteNames = new[] { "Ataşehir Proje", "Kadıköy Şantiye", "Ümraniye İnşaat", "Beşiktaş Sahası", "Şişli Merkez" };
            for (var i = 0; i < needSites; i++)
            {
                db.ConstructionSites.Add(new ConstructionSite
                {
                    Id = Guid.NewGuid(),
                    Name = siteNames[i % siteNames.Length] + (i >= siteNames.Length ? " " + (i + 1) : ""),
                    Address = "İstanbul",
                    StartDate = now.AddMonths(-(i + 1)),
                    EndDate = now.AddMonths(6),
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            constructionSites = await db.ConstructionSites.ToListAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} şantiye eklendi.", needSites);
        }

        var cranes = await db.Cranes.ToListAsync(cancellationToken);
        var needCranes = targetCount - cranes.Count;
        if (needCranes > 0)
        {
            var operatorUser = await db.Users.FirstOrDefaultAsync(u => u.Role == UserRole.Operatör, cancellationToken);
            var types = new[] { "Mobil", "Kule", "Paletli", "Tekerlekli", "Seyyar" };
            for (var i = 0; i < needCranes; i++)
            {
                var idx = cranes.Count + i;
                db.Cranes.Add(new Crane
                {
                    Id = Guid.NewGuid(),
                    Code = "VNC-" + (idx + 1).ToString("000"),
                    Name = types[i % types.Length] + " Vinç " + (idx + 1),
                    Type = types[i % types.Length],
                    Location = "Şantiye " + (idx + 1),
                    Status = "Aktif",
                    ConstructionSiteId = constructionSites[idx % constructionSites.Count].Id,
                    AssignedOperatorId = operatorUser?.Id,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            cranes = await db.Cranes.ToListAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} vinç eklendi.", needCranes);
        }

        var workPlans = await db.WorkPlans.ToListAsync(cancellationToken);
        var needWorkPlans = targetCount - workPlans.Count;
        if (needWorkPlans > 0)
        {
            for (var i = 0; i < needWorkPlans; i++)
            {
                var idx = workPlans.Count + i;
                db.WorkPlans.Add(new WorkPlan
                {
                    Id = Guid.NewGuid(),
                    Title = "İş Planı " + (idx + 1) + " - " + constructionSites[idx % constructionSites.Count].Name,
                    CraneId = cranes[idx % cranes.Count].Id,
                    ConstructionSiteId = constructionSites[idx % constructionSites.Count].Id,
                    PlannedStart = now.AddDays(10 + idx),
                    PlannedEnd = now.AddMonths(2 + idx),
                    Status = idx % 3 == 0 ? "Planlandı" : idx % 3 == 1 ? "Devam Ediyor" : "Tamamlandı",
                    CompanyId = companies[idx % companies.Count].Id,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            workPlans = await db.WorkPlans.ToListAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} iş planı eklendi.", needWorkPlans);
        }

        var progressCount = await db.ProgressPayments.CountAsync(cancellationToken);
        var needProgress = targetCount - progressCount;
        if (needProgress > 0)
        {
            for (var i = 0; i < needProgress; i++)
            {
                var idx = progressCount + i;
                db.ProgressPayments.Add(new ProgressPayment
                {
                    Id = Guid.NewGuid(),
                    WorkPlanId = workPlans[idx % workPlans.Count].Id,
                    CompanyId = companies[idx % companies.Count].Id,
                    Amount = 30000m + (idx * 5000),
                    Period = "2024-" + (idx % 12 + 1).ToString("00"),
                    Status = idx % 3 == 0 ? "Beklemede" : idx % 3 == 1 ? "Onaylandı" : "Ödendi",
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} hakediş eklendi.", needProgress);
        }

        var incomeCount = await db.IncomeExpenses.CountAsync(cancellationToken);
        var needIncome = Math.Max(0, targetCount - incomeCount);
        if (needIncome > 0)
        {
            for (var i = 0; i < needIncome; i++)
            {
                var isGelir = i % 2 == 0;
                db.IncomeExpenses.Add(new IncomeExpense
                {
                    Id = Guid.NewGuid(),
                    Type = isGelir ? IncomeExpenseType.Gelir : IncomeExpenseType.Gider,
                    Category = isGelir ? "Kiralama" : "Bakım",
                    Amount = isGelir ? 20000m + (i * 1000) : 3000m + (i * 500),
                    Date = now.AddDays(-(i + 5)),
                    Description = (isGelir ? "Gelir " : "Gider ") + (i + 1),
                    ReferenceType = ReferenceType.Manuel,
                    CompanyId = companies[i % companies.Count].Id,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} gelir-gider eklendi.", needIncome);
        }

        var maintenanceCount = await db.MaintenanceRecords.CountAsync(cancellationToken);
        var needMaintenance = targetCount - maintenanceCount;
        if (needMaintenance > 0 && cranes.Count > 0)
        {
            var types = new[] { "Periyodik", "Arıza", "Revizyon", "Kontrol", "Yağlama" };
            for (var i = 0; i < needMaintenance; i++)
            {
                db.MaintenanceRecords.Add(new MaintenanceRecord
                {
                    Id = Guid.NewGuid(),
                    CraneId = cranes[i % cranes.Count].Id,
                    MaintenanceDate = now.AddDays(-(i + 10)),
                    Description = types[i % types.Length] + " bakım " + (i + 1),
                    Type = types[i % types.Length],
                    NextDueDate = now.AddMonths(3 + i),
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} bakım kaydı eklendi.", needMaintenance);
        }

        var sliderCount = await db.Sliders.CountAsync(cancellationToken);
        var needSliders = targetCount - sliderCount;
        if (needSliders > 0)
        {
            var titles = new[] { "Profesyonel Hizmet", "Güvenilir Ekip", "7/24 Destek", "Kalite Taahhüdü", "Müşteri Memnuniyeti" };
            for (var i = 0; i < needSliders; i++)
            {
                var idx = sliderCount + i;
                db.Sliders.Add(new Slider
                {
                    Id = Guid.NewGuid(),
                    Title = titles[i % titles.Length],
                    ShortText = "Slider açıklama " + (idx + 1),
                    ImageUrl = idx % 2 == 0 ? "https://picsum.photos/1200/400?r=" + idx : null,
                    SortOrder = idx + 1,
                    IsActive = true,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} slider eklendi.", needSliders);
        }

        var newsCount = await db.News.CountAsync(cancellationToken);
        var needNews = targetCount - newsCount;
        if (needNews > 0)
        {
            for (var i = 0; i < needNews; i++)
            {
                var idx = newsCount + i;
                db.News.Add(new News
                {
                    Id = Guid.NewGuid(),
                    Title = "Haber Başlığı " + (idx + 1),
                    Summary = "Özet metni " + (idx + 1),
                    Body = "İçerik gövdesi...",
                    ImageUrl = idx % 2 == 0 ? "https://picsum.photos/800/400?n=" + idx : null,
                    IsPublished = idx % 5 != 4,
                    PublishedAt = idx % 5 != 4 ? now.AddDays(-idx) : (DateTime?)null,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} haber eklendi.", needNews);
        }

        var galleryCount = await db.GalleryItems.CountAsync(cancellationToken);
        var needGallery = targetCount - galleryCount;
        if (needGallery > 0)
        {
            for (var i = 0; i < needGallery; i++)
            {
                var idx = galleryCount + i;
                db.GalleryItems.Add(new GalleryItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Galeri " + (idx + 1),
                    ImageUrl = "https://picsum.photos/400/300?g=" + idx,
                    SortOrder = idx + 1,
                    IsActive = true,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} galeri eklendi.", needGallery);
        }

        var settingKeysToEnsure = new[] { "SiteAdi", "LogoUrl", "SiteAciklama", "SmtpHost", "FromAddress", "SmtpPort", "FromName", "ApiBaseUrl" };
        var existingKeys = await db.SystemSettings.Select(s => s.Key).ToListAsync(cancellationToken);
        var keysToAdd = settingKeysToEnsure.Where(k => !existingKeys.Contains(k)).Take(Math.Max(0, targetCount - existingKeys.Count)).ToList();
        foreach (var key in keysToAdd)
        {
            db.SystemSettings.Add(new SystemSetting
            {
                Id = Guid.NewGuid(),
                Key = key,
                Value = key == "SiteAdi" ? "TORE VINC" : key == "SmtpPort" ? "587" : "",
                CreatedAt = now
            });
        }
        if (keysToAdd.Count > 0)
        {
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} sistem ayarı eklendi.", keysToAdd.Count);
        }

        var operators = await db.Users.Where(u => u.Role == UserRole.Operatör).ToListAsync(cancellationToken);
        var assignmentCount = await db.OperatorAssignments.CountAsync(cancellationToken);
        var needAssignments = targetCount - assignmentCount;
        if (needAssignments > 0 && operators.Count > 0 && cranes.Count > 0)
        {
            for (var i = 0; i < needAssignments; i++)
            {
                var idx = assignmentCount + i;
                db.OperatorAssignments.Add(new OperatorAssignment
                {
                    Id = Guid.NewGuid(),
                    UserId = operators[idx % operators.Count].Id,
                    CraneId = cranes[idx % cranes.Count].Id,
                    ConstructionSiteId = constructionSites[idx % constructionSites.Count].Id,
                    StartDate = now.AddDays(-30 + idx),
                    EndDate = now.AddMonths(2),
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} operatör ataması eklendi.", needAssignments);
        }

        var dailyCount = await db.DailyWageRecords.CountAsync(cancellationToken);
        var needDaily = targetCount - dailyCount;
        if (needDaily > 0 && operators.Count > 0)
        {
            for (var i = 0; i < needDaily; i++)
            {
                var idx = dailyCount + i;
                db.DailyWageRecords.Add(new DailyWageRecord
                {
                    Id = Guid.NewGuid(),
                    UserId = operators[idx % operators.Count].Id,
                    ConstructionSiteId = constructionSites[idx % constructionSites.Count].Id,
                    WorkPlanId = workPlans.Count > 0 ? workPlans[idx % workPlans.Count].Id : null,
                    Date = now.AddDays(-(idx + 5)),
                    Amount = 500m + (idx * 50),
                    Description = "Yevmiye " + (idx + 1),
                    Status = "Ödendi",
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} yevmiye eklendi.", needDaily);
        }

        var overtimeCount = await db.OvertimeRecords.CountAsync(cancellationToken);
        var needOvertime = targetCount - overtimeCount;
        if (needOvertime > 0 && operators.Count > 0)
        {
            for (var i = 0; i < needOvertime; i++)
            {
                var idx = overtimeCount + i;
                var hours = 2m + (idx % 4);
                var rate = 100m;
                db.OvertimeRecords.Add(new OvertimeRecord
                {
                    Id = Guid.NewGuid(),
                    UserId = operators[idx % operators.Count].Id,
                    Date = now.AddDays(-(idx + 3)),
                    Hours = hours,
                    Rate = rate,
                    Amount = hours * rate,
                    IsApproved = idx % 2 == 0,
                    ApprovedByUserId = null,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} mesai eklendi.", needOvertime);
        }

        var fuelCount = await db.FuelRecords.CountAsync(cancellationToken);
        var needFuel = targetCount - fuelCount;
        if (needFuel > 0 && cranes.Count > 0 && operators.Count > 0)
        {
            for (var i = 0; i < needFuel; i++)
            {
                var idx = fuelCount + i;
                db.FuelRecords.Add(new FuelRecord
                {
                    Id = Guid.NewGuid(),
                    CraneId = cranes[idx % cranes.Count].Id,
                    Quantity = 50m + (idx * 10),
                    Unit = "Litre",
                    Date = now.AddDays(-(idx + 2)),
                    OperatorId = operators[idx % operators.Count].Id,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} yakıt kaydı eklendi.", needFuel);
        }

        var announcementCount = await db.Announcements.CountAsync(cancellationToken);
        var needAnnouncements = targetCount - announcementCount;
        if (needAnnouncements > 0)
        {
            for (var i = 0; i < needAnnouncements; i++)
            {
                var idx = announcementCount + i;
                db.Announcements.Add(new Announcement
                {
                    Id = Guid.NewGuid(),
                    Title = "Duyuru " + (idx + 1),
                    Content = "Duyuru içeriği...",
                    IsPinned = idx == 0,
                    StartDate = now.AddDays(-idx),
                    EndDate = now.AddMonths(1),
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} duyuru eklendi.", needAnnouncements);
        }

        var contactCount = await db.ContactMessages.CountAsync(cancellationToken);
        var needContact = targetCount - contactCount;
        if (needContact > 0)
        {
            for (var i = 0; i < needContact; i++)
            {
                var idx = contactCount + i;
                db.ContactMessages.Add(new ContactMessage
                {
                    Id = Guid.NewGuid(),
                    Name = "Ziyaretçi " + (idx + 1),
                    Email = "ziyaretci" + (idx + 1) + "@test.com",
                    Phone = "0532 111 " + (idx + 10).ToString("00 00"),
                    Subject = "İletişim konu " + (idx + 1),
                    Message = "Mesaj metni...",
                    IsRead = idx % 2 == 0,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} iletişim mesajı eklendi.", needContact);
        }

        var appointmentCount = await db.Appointments.CountAsync(cancellationToken);
        var needAppointments = targetCount - appointmentCount;
        if (needAppointments > 0)
        {
            for (var i = 0; i < needAppointments; i++)
            {
                var idx = appointmentCount + i;
                db.Appointments.Add(new Appointment
                {
                    Id = Guid.NewGuid(),
                    CustomerName = "Müşteri " + (idx + 1),
                    Email = "musteri" + (idx + 1) + "@test.com",
                    Phone = "0533 222 " + (idx + 10).ToString("00 00"),
                    PreferredDate = now.AddDays(7 + idx),
                    Notes = "Randevu notu",
                    Status = idx % 3 == 0 ? "Beklemede" : "Onaylandı",
                    CompanyId = companies[idx % companies.Count].Id,
                    CreatedAt = now
                });
            }
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("EnsureFullSample: {Count} randevu eklendi.", needAppointments);
        }

        logger.LogInformation("EnsureFullSampleData tamamlandı.");
    }
}
