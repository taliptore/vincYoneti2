# TORE VINC – Planı Uygulamak İçin Adım Adım Promptlar

Bu dosya, [PLAN.md](PLAN.md) ve Cursor’daki detaylı plana göre projeyi uygulamak için kullanabileceğin **kopyala-yapıştır promptları** içerir. Her adımı sırayla bir AI asistanına veya geliştiriciye verebilirsin.

---

## Adım 1 – Solution ve Projeler

**Prompt:**

```
TORE VINC Vinç Yönetim Sistemi için aşağıdaki solution ve projeleri oluştur:

- Solution adı: CraneManagementSystem.sln
- Klasör: src/ altında

Projeler:
1. CraneManagementSystem.Domain – Class Library (.NET 8), hiçbir paket referansı yok
2. CraneManagementSystem.Application – Class Library (.NET 8), referans: Domain
3. CraneManagementSystem.Persistence – Class Library (.NET 8), referans: Domain. Paketler: Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools
4. CraneManagementSystem.Infrastructure – Class Library (.NET 8), referans: Application, Persistence
5. CraneManagementSystem.API – ASP.NET Core Web API (.NET 8), referans: Application, Infrastructure

Workspace kökünde (VincYonetimiSistemi) src/ içinde oluştur. Proje referanslarını doğru ekle.
```

---

## Adım 2 – Domain Katmanı

**Prompt:**

```
CraneManagementSystem.Domain projesinde plandaki entity'leri oluştur. PLAN.md ve veritabanı tabloları bölümüne göre:

1. BaseEntity: Id (Guid), CreatedAt (DateTime), UpdatedAt (DateTime?)
2. UserRole enum: Admin, Muhasebe, Operatör, Firma
3. Entity'ler (hepsi BaseEntity'den türesin, gerekli FK'lar ile):
   - User: Email, PasswordHash, FullName, Role (UserRole), CompanyId (Guid?), IsActive
   - Company: Name, TaxNumber, Address, Phone, Email
   - Crane: Code, Name, Type, Location, Status, ConstructionSiteId (Guid?), AssignedOperatorId (Guid?)
   - ConstructionSite: Name, Address, StartDate, EndDate
   - OperatorAssignment: UserId, CraneId (Guid?), ConstructionSiteId (Guid?), StartDate, EndDate
   - WorkPlan: Title, CraneId, ConstructionSiteId, PlannedStart, PlannedEnd, Status, CompanyId (Guid?)
   - ProgressPayment: WorkPlanId (Guid?), CompanyId, Amount, Period, Status
   - DailyWageRecord: UserId, ConstructionSiteId (Guid?), WorkPlanId (Guid?), Date, Amount, Description, Status
   - OvertimeRecord: UserId, Date, Hours, Rate, Amount, IsApproved, ApprovedByUserId (Guid?)
   - IncomeExpense: Type (Gelir/Gider enum veya string), Category, Amount, Date, Description, ReferenceType, ReferenceId (Guid?), CompanyId (Guid?)
   - FuelRecord: CraneId, Quantity, Unit, Date, OperatorId
   - MaintenanceRecord: CraneId, MaintenanceDate, Description, Type, NextDueDate (DateTime?)
   - SystemSetting: Key, Value
   - News: Title, Summary, Body, ImageUrl, IsPublished, PublishedAt (DateTime?)
   - Announcement: Title, Content, IsPinned, StartDate, EndDate
   - ContactMessage: Name, Email, Phone, Subject, Message, IsRead
   - Appointment: CustomerName, Email, Phone, PreferredDate, Notes, Status, CompanyId (Guid?)

Navigation property'leri (ilişkiler) ekle. Domain projesine başka paket ekleme.
```

---

## Adım 3 – Persistence: DbContext ve Konfigürasyonlar

**Prompt:**

```
CraneManagementSystem.Persistence projesinde:

1. ApplicationDbContext oluştur. Tüm entity'ler için DbSet ekle (Users, Companies, Cranes, ConstructionSites, OperatorAssignments, WorkPlans, ProgressPayments, DailyWageRecords, OvertimeRecords, IncomeExpenses, FuelRecords, MaintenanceRecords, SystemSettings, News, Announcements, ContactMessages, Appointments). OnConfiguring veya constructor'da connection string alacak şekilde ayarla.

2. Her entity için IEntityTypeConfiguration<T> sınıfı oluştur (Configurations/ klasöründe). Fluent API ile tablo adı, gerekli index'ler (Users.Email unique, SystemSettings.Key unique vb.), ilişkileri tanımla.

3. DbContext.OnModelCreating içinde ApplyConfigurationsFromAssembly ile tüm konfigürasyonları uygula.

PLAN.md'deki "Veritabanı Tabloları" bölümündeki kolon ve FK'lara uy.
```

---

## Adım 4 – Persistence: İlk Migration

**Prompt:**

```
CraneManagementSystem.Persistence projesinde ilk EF Core migration'ı oluştur. Migration adı: InitialCreate. API projesinde appsettings.json ve appsettings.Development.json içinde ConnectionStrings:DefaultConnection ekle (MSSQL). API'nin Startup/Program.cs'inde DbContext'i bu connection string ile kaydet. Migration'ı veritabanına uygula (Update-Database veya dotnet ef database update).
```

---

## Adım 5 – Application: DTO'lar ve Interface'ler

**Prompt:**

```
CraneManagementSystem.Application projesinde plandaki DTO klasörlerini ve interface'leri oluştur:

1. DTOs: Auth (LoginRequest, LoginResponse, RegisterRequest), User, Company, Crane, ConstructionSite, WorkPlan, ProgressPayment, DailyWage, Overtime, IncomeExpense, FuelTracking, Maintenance, Report, SystemSetting, Dashboard, News, Announcement, Contact, Appointment, Home. Her modül için en azından liste/detay/create/update DTO'ları (ihtiyaca göre).

2. Interfaces: Her entity için I{X}Repository (GetById, GetAll, Add, Update, Delete veya ihtiyaca göre), IAuthService, ITokenService, IEmailService, IHomePageService, IDashboardService. Repository interface'leri Application'da, implementasyonlar Infrastructure'da olacak.

Application projesi sadece Domain'e referans versin.
```

---

## Adım 6 – Application: Servisler

**Prompt:**

```
CraneManagementSystem.Application projesinde servis sınıflarını oluştur (Services/ klasöründe). Her servis ilgili repository interface'lerini ve domain entity'lerini kullansın. Rol bazlı filtre: Firma rolü için CompanyId, Operatör için kendi UserId ile filtre uygula (DailyWage, Overtime, Appointment vb.).

Servisler: AuthService (login, register; ITokenService kullan), UserService, CompanyService, CraneService, ConstructionSiteService, WorkPlanService, ProgressPaymentService, DailyWageService, OvertimeService, IncomeExpenseService, FuelTrackingService, MaintenanceService, ReportService, SystemSettingService, DashboardService (özet istatistikler), NewsService, AnnouncementService, ContactService, AppointmentService, HomePageService.

Şifre hash için Microsoft.Extensions.Identity.Core veya BCrypt kullan. Application projesine EF Core veya Infrastructure referansı ekleme.
```

---

## Adım 7 – Infrastructure: Repository ve JWT/Email

**Prompt:**

```
CraneManagementSystem.Infrastructure projesinde:

1. Her I{X}Repository için concrete repository sınıfı (Repositories/). ApplicationDbContext inject et, CRUD ve plandaki sorguları implemente et.

2. JWT token üretimi: ITokenService/IAuthService implementasyonu. JWT ayarları (Secret, Issuer, Audience) appsettings'ten okunsun. Token'a "role" claim'i ekle (User.Role).

3. IEmailService implementasyonu (SMTP veya mock). IOptions<EmailSettings> kullan.

4. DependencyInjection.cs (veya Extensions/InfrastructureExtensions.cs): AddInfrastructure(IServiceCollection, IConfiguration) ile tüm repository'leri, AuthService/TokenService ve EmailService'i kaydet. Persistence'daki DbContext kaydı API'de yapılacak; Infrastructure sadece kendi servislerini eklesin.
```

---

## Adım 8 – API: Program.cs, Auth, Middleware, CORS

**Prompt:**

```
CraneManagementSystem.API projesinde:

1. Program.cs: Persistence'dan AddDbContext (ConnectionStrings:DefaultConnection), Infrastructure'dan AddInfrastructure(config). AddAuthentication(JwtBearer), AddAuthorization (policy'ler: AdminOnly, AdminOrMuhasebe, AdminOrOperator, RequireFirma). CORS policy ekle (bilinen origin'ler). UseAuthentication, UseAuthorization sırası doğru olsun.

2. Exception middleware: Tüm exception'ları yakala, ProblemDetails veya özel hata formatına çevir, uygun status code (401, 403, 404, 500) dön. Pipeline'da en üstte kullan.

3. Swagger: Swashbuckle.AspNetCore. JWT Bearer security definition ve requirement ekle; Swagger UI'da Authorize ile token girilebilsin.
```

---

## Adım 9 – API: Controller'lar (Auth ve Yönetim Paneli)

**Prompt:**

```
CraneManagementSystem.API projesinde Controller'ları oluştur. Plandaki rol matrisine göre [Authorize(Roles = "...")] veya [Authorize(Policy = "...")] kullan. Public endpoint'ler [AllowAnonymous].

1. AuthController: POST login (email, password → JWT + role), POST register (Admin veya [AllowAnonymous] ihtiyaca göre).
2. DashboardController: GET özet (tüm roller; rol bazlı veri).
3. CompaniesController: CRUD; Admin tam, Muhasebe/Firma görüntüleme veya kendi firması.
4. CranesController, ConstructionSitesController, WorkPlansController: Rol matrisine göre Admin, Operatör, Muhasebe.
5. OperatorsController veya UsersController (rol=Operatör): Admin tam (atama dahil), Operatör görüntüleme; atama endpoint'leri sadece Admin.
6. ProgressPaymentsController, DailyWageController, OvertimeController, IncomeExpenseController: Admin, Muhasebe; Operatör/Firma kendi veya görüntüleme.
7. FuelTrackingController, MaintenanceController: Admin, Operatör; Muhasebe görüntüleme.
8. ReportsController: Rol bazlı kapsam.
9. UsersController: Sadece Admin. SystemSettingsController: Sadece Admin.
10. HomeController (veya MainPageController): GET ana sayfa özeti [AllowAnonymous]. NewsController, AnnouncementsController: Public GET; CRUD [Authorize]. ContactController: POST mesaj [AllowAnonymous]; GET messages [Authorize]. AppointmentsController: POST randevu [AllowAnonymous]; GET/PATCH [Authorize], Firma kendi kayıtları.

Tüm listeleme endpoint'lerinde sayfalama (page, pageSize) ekle.
```

---

## Adım 10 – Seed ve İlk Admin Kullanıcısı

**Prompt:**

```
İlk çalıştırmada veritabanında en az bir Admin kullanıcısı olsun. Bunu ApplicationDbContext için HasData (seed) veya ayrı bir seed class/middleware ile uygula. Admin email ve varsayılan şifre appsettings'te veya environment variable'da tutulabilir (sadece ilk kurulumda). Seed'i migration'a ekleyebilir veya API başlarken "eğer hiç admin yoksa oluştur" şeklinde çalıştırabilirsin.
```

---

## Adım 11 – Test ve Doğrulama

**Prompt:**

```
Projeyi doğrula: API'yi çalıştır, Swagger'dan login endpoint'ini test et (seed Admin ile). JWT alındıktan sonra Authorize ile Bearer token girip diğer endpoint'leri (Dashboard, Companies, Cranes vb.) test et. Rol bazlı erişimi kontrol et (farklı rollerle token alıp yetkisiz endpoint'e 403 dönüyor mu). Exception middleware'i tetikleyerek hata formatını kontrol et.
```

---

## Adım 12 – Web Tarafı: Ana Sayfa ve Yönetim Paneli

**Prompt:**

```
PLAN.md'deki "Web Tarafı – Ana Sayfa ve Yönetim Paneli" bölümüne göre web arayüzünü ekle.

1. Yeni proje: src/CraneManagementSystem.Web (ASP.NET Core MVC veya Razor Pages) veya SPA (React/Vue/Blazor). Solution'a ekle. API Base URL (örn. http://localhost:5116) ayarlanabilir olsun.

2. Ana sayfa (public): Ziyaretçiye açık landing. API'den haberler (GET /api/news), duyurular (GET /api/announcements), ana sayfa özeti (GET /api/home). İletişim formu → POST /api/contact. Randevu talebi → POST /api/appointments. Giriş yapmadan erişilebilir.

3. Yönetim paneli: Login sayfası → POST /api/auth/login, JWT sakla (cookie veya session/localStorage). Giriş sonrası rol bazlı menü (Admin, Muhasebe, Operatör, Firma). PLAN.md "7. Yönetim Paneli Modülleri" rol matrisine göre: Dashboard, Firmalar, Vinçler, Operatörler, Şantiyeler, İş Planlama, Hakediş, Yevmiye, Mesai, Gelir-Gider, Yakıt, Bakım, Raporlar, Kullanıcı Yönetimi (Admin), Sistem Ayarları (Admin). Her sayfada ilgili API endpoint'lerini kullan (sayfalama, filtreleme). Yetkisiz sayfaya erişimde 403 veya login'e yönlendir.

4. En azından ana sayfa (haber/duyuru listesi, iletişim formu), login ve Dashboard (GET /api/dashboard/summary) ile bir yönetim listesi (örn. Firmalar) implemente et.
```

---

## Adım 13 (Opsiyonel) – Mobil Uygulama

**Prompt:**

```
TORE VINC mobil uygulaması: Mevcut Web API'yi (CraneManagementSystem.API) kullanacak. Teknoloji olarak .NET MAUI (veya Flutter / React Native tercihine göre) kullan.

Yapılacaklar:
1. Yeni proje: ToreVinc.Mobile (MAUI) veya ayrı solution. API Base URL ayarlanabilir olsun (Development/Production).
2. Login ekranı: POST /api/auth/login ile giriş, JWT'yi güvenli sakla (SecureStorage). Sonraki isteklerde Authorization: Bearer header ekle.
3. JWT'deki role claim'ine göre ana menüyü göster (Admin, Muhasebe, Operatör, Firma). Her rol için plandaki mobil özellikler: Operatör → yakıt girişi, bakım, yevmiye/mesai, atandığı vinçler/şantiyeler; Firma → randevu, kendi hakedişleri; Muhasebe → hakediş, yevmiye, mesai, gelir-gider listeleri; Admin → dashboard özeti, onaylar, listeler.
4. En azından Dashboard (özet), Login ve rol bazlı bir liste ekranı (örn. Operatör için yakıt girişi formu) implemente et.
```

---

## Kullanım Notu

- Her promptu sırayla kullan; bir sonraki adım öncekine bağlı olabilir.
- "PLAN.md" veya "Cursor’daki plan" derken workspace’teki [PLAN.md](PLAN.md) ve Cursor plan dosyasındaki detaylı bölümler kastedilmektedir.
- Gerekirse bir adımı "şu projede, şu klasörde" diye netleştirerek iki ayrı prompta bölebilirsin.
