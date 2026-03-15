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

## Adım 12.1 – Web: Modern Tasarım (Tüm Sayfalar)

**Prompt:**

```
CraneManagementSystem.Web projesinde tüm sayfalar için modern, tutarlı ve responsive bir tasarım uygula. PLAN.md "Web – Modern Tasarım" hedeflerine uy.

Tasarım ilkeleri:
- Tutarlı renk paleti (birincil, ikincil, arka plan, metin); TORE VINC kurumsal kimliğine uygun, sade ve profesyonel.
- Tipografi: Okunabilir font ailesi (ör. sistem fontları veya Google Fonts), başlık/gövde hiyerarşisi, uygun satır yüksekliği.
- Boşluk ve hizalama: Tutarlı padding/margin (örn. 8px grid), container max-width, kart ve form gruplarında düzenli aralıklar.
- Responsive: Mobil önce; tüm sayfalar küçük ekranda düzgün görünsün (navbar collapse, tablolar yatay kaydırma veya kart görünümü).
- Erişilebilirlik: Yeterli kontrast, focus stilleri, form etiketleri ve hata mesajları ilişkilendirilmiş olsun.
- Bileşenler: Butonlar, kartlar, form alanları, tablolar ve uyarılar (alert) proje genelinde aynı stilde.

Kapsanacak sayfalar (hepsini güncelle):

1. Ortak layout’lar
   - Views/Shared/_Layout.cshtml: Public site header/footer; logo alanı, navigasyon (Ana Sayfa, İletişim, Randevu Al, Yönetim Girişi); footer telif ve linkler; mobil menü.
   - Views/Shared/_PanelLayout.cshtml: Panel header (sidebar veya üst menü); rol bazlı menü öğeleri; kullanıcı adı ve Çıkış; aktif sayfa vurgusu.

2. Public sayfalar
   - Views/Home/Index.cshtml: Ana sayfa hero (başlık, alt başlık, CTA butonları); duyurular ve haberler bölümleri kart/ grid yapıda; başarı mesajları (TempData) için alert bileşeni.
   - Views/Home/Contact.cshtml: İletişim formu; etiketli alanlar, validation mesajları, Gönder/İptal butonları; form kart veya bölüm içinde.
   - Views/Home/Appointment.cshtml: Randevu formu; tarih seçici, not alanı; aynı form stili ve butonlar.
   - Views/Home/Privacy.cshtml: Varsa metin içeriği; okunabilir tipografi ve boşluk.
   - Views/Shared/Error.cshtml: Hata mesajı ve isteğe bağlı RequestId; sade ve anlaşılır görünüm.

3. Hesap
   - Views/Account/Login.cshtml: Merkezde giriş kartı; e-posta ve şifre alanları, Giriş butonu, “Ana sayfaya dön” linki; mobilde tam genişlik.

4. Yönetim paneli sayfaları
   - Views/Panel/Index.cshtml: Dashboard; özet kartlar (Firma, Vinç, Şantiye, vb.) grid’de; kartlar gölge/kenarlık ile ayrılsın; sayılar okunaklı.
   - Views/Panel/Companies.cshtml: Firmalar tablosu (veya mobilde kart listesi); başlıklar ve sayfalama (pagination) stilli; boş durum mesajı.

Stil dosyaları:
- wwwroot/css/site.css (veya benzeri) içinde: CSS değişkenleri (renkler, boşluk, font), ortak bileşen sınıfları, yardımcı sınıflar.
- Bootstrap 5 kullanılıyorsa mevcut sınıfları kullan; gerekiyorsa özelleştirme (override) ile markaya uyum sağla. Ek stiller site.css’e eklensin; tüm sayfalar bu stilleri kullansın.

Çıktı: Tüm listelenen view’lar ve _Layout/_PanelLayout güncellenmiş olmalı; site tek bir “modern” görünümde, mobil uyumlu ve tutarlı olmalı.
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

## Adım 14 – Ana Sayfa CMS, Menü Sistemi ve Rol Ekranları

**Prompt:**

```
PLAN.md'deki "Ana Sayfa İçerikleri (CMS ile yönetilen)", "Yönetim Paneli – Menü ve Rol Bazlı Ekranlar" ve "CMS Tasarımı (Vinç Temalı)" bölümlerine göre uygula.

1) Backend (API + Domain)
- Gerekli entity'ler: Slider (Title, ShortText, ImageUrl, SortOrder, IsActive, vb.), GalleryItem (Title, ImageUrl, SortOrder, IsActive veya Gallery grup). Hakkımızda için SystemSetting anahtarı (örn. "AboutUs") veya AboutPage entity. Migration ekle.
- API: GET/POST/PUT/DELETE /api/slider, /api/gallery (veya /api/gallery/items); hakkımızda için GET/PUT sayfa içeriği. Rol: Admin (veya belirlenen roller) CMS endpoint'lerine erişebilsin.

2) Ana sayfa (public)
- Slider: API'den aktif slider'ları çek; üstte carousel/slider bileşeni (başlık, kısa metin, görsel). Responsive.
- Haber: Mevcut /api/news veya home özeti; grid/kart listesi.
- Galeri: API'den galeri öğelerini çek; grid görünüm, isteğe bağlı lightbox.
- İletişim: Mevcut iletişim formu; adres/telefon/e-posta site ayarlarından veya sabit alanlardan gösterilsin.
- Hakkımızda: API'den hakkımızda metnini (ve varsa görsel) çek; ana sayfada "Hakkımızda" bölümü veya ayrı sayfa.

3) Yönetim paneli – menü sistemi
- Rol bazlı menü: JWT'deki role göre sol menü (sidebar) veya üst menü öğelerini filtrele. PLAN.md "7. Yönetim Paneli Modülleri" matrisine göre: Dashboard, Firmalar, Vinçler, Operatörler, Şantiyeler, İş Planlama, Hakediş, Yevmiye, Mesai, Gelir-Gider, Yakıt, Bakım, Raporlar, Kullanıcı Yönetimi (Admin), Sistem Ayarları (Admin). Yetkisiz modül linki gösterilmesin.
- Tüm roller için ekranlar: Her modül için en az liste (ve yetkiye göre ekleme/düzenleme) sayfası; menüden tıklanınca ilgili ekran açılsın. Yetkisiz URL'de 403 veya login'e yönlendir.

4) CMS ekranları (vinç temalı tasarım)
- Slider yönetimi: Listele (tablo/kart), sıra, başlık, görsel önizleme, aktif/pasif. Ekle/Düzenle formu (başlık, kısa metin, görsel URL veya yükleme).
- Haber yönetimi: Mevcut News CRUD'u panelde kullan veya genişlet; listeleme, ekleme, düzenleme.
- Galeri yönetimi: Listele, sırala, ekle/düzenle (başlık, görsel). Vinç/inşaat temalı sade arayüz.
- Hakkımızda: Tek sayfa form (metin alanı, isteğe bağlı görsel); kaydetme API'ye bağla.
- Tasarım: Kurumsal mavi/gri tonları, tutarlı kart ve form stili, vinç/şantiye ile uyumlu görsel dil.

Çıktı: Ana sayfada slider + haber + galeri + iletişim + hakkımızda; panelde rol bazlı menü ve tüm modül ekranları; Admin'de CMS (slider, haber, galeri, hakkımızda) yönetimi; vinç temalı CMS arayüzü.
```

---

## Adım 15 – Ana Sayfa Yapısı (Public Site Layout)

**Prompt:**

```
PLAN.md'deki "Ana Sayfa Yapısı (Public Site Layout)" bölümüne göre public ana sayfayı ve site iskeletini uygula. Mevcut CraneManagementSystem.Web ana sayfasını bu yapıya göre güncelle veya yeniden düzenle.

1) HEADER
- Logo: Sol tarafta site logosu (SiteSetting/SystemSetting’ten LogoUrl veya sabit ~/images/logo.png).
- Menü: Ana Sayfa, Hizmetler (dropdown veya alt linkler), Hakkımızda, Referanslar, Galeri, Haberler, İletişim. Anchor link (#hizmetler, #hakkimizda vb.) veya ayrı sayfalar.
- Telefon: Header’da görünür telefon (SiteSetting/ContactPhone).
- Teklif Al butonu: Sağ üstte veya menü yanında; Randevu/İletişim veya Teklif formu sayfasına yönlendir.

2) SLIDER
- Slider görselleri, başlık, açıklama (ShortText), buton (CTA). Mevcut Slider entity/API kullan; buton metni ve link alanı varsa kullan, yoksa “Teklif Al” ile randevu/iletişim sayfasına link ver.
- Carousel responsive; her slaytta: arka plan veya önde görsel, başlık, kısa metin, buton.

3) HİZMETLER
- Dört blok: Sepetli Vinç, Mobil Vinç, Platform, Kiralama. Kart veya ikonlu bölümler; her biri için başlık ve kısa açıklama. İçerik sabit (view’da) veya SystemSetting/Hizmet entity ile panelden yönetilebilir. İsteğe bağlı “Detay” veya “Teklif Al” linki.

4) HAKKIMIZDA
- Kısa tanıtım metni (API’den AboutUs’un ilk paragrafı veya özet alanı). “Devamını oku” linki → /Home/About veya #hakkimizda detay sayfası.

5) REFERANSLAR
- Firma logoları bölümü. Gerekirse Domain’e Referans/PartnerLogo entity (Ad, LogoUrl, SortOrder); API GET /api/referanslar; panelde Admin CRUD. Yoksa SystemSetting ile virgülle ayrılmış logo URL’leri veya JSON. Carousel veya grid; görseller yan yana.

6) GALERİ
- Proje fotoğrafları; mevcut /api/gallery veya home.GalleryItems. Grid görünüm; tıklanınca lightbox (CSS/JS) ile büyütme.

7) HABERLER / BLOG
- Firma haberleri; mevcut /api/news veya home.LatestNews. Kart listesi; başlık, özet, tarih, “Devamı” linki (isteğe bağlı /News/Detail/id).

8) İLETİŞİM
- Harita: Embed (Google Maps iframe veya MapUrl SystemSetting) veya statik harita.
- Adres, telefon: Site ayarlarından (ContactAddress, ContactPhone, ContactEmail).
- İletişim formu: Mevcut POST /api/contact; isim, e-posta, konu, mesaj.

9) FOOTER
- Sosyal medya: Facebook, Instagram, LinkedIn vb. ikonları; URL’ler SystemSetting (SocialFacebook, SocialInstagram, …) veya tek JSON ayarından.
- Hızlı linkler: Ana Sayfa, Hakkımızda, Hizmetler, Galeri, Haberler, İletişim, Gizlilik (varsa). Telif/metin: “© TORE VINC …”.

Teknoloji: Mevcut MVC View’lar (Razor), _Layout.cshtml, site.css. Responsive; mobilde menü hamburger. Çıktı: PLAN’daki tüm blokların tek sayfada (ve gerekirse Hakkımızda/İletişim ayrı sayfa) yer aldığı, header/footer’ın ortak olduğu public site.
```

---

## Adım 16 – Vue 3 Admin Panel ve Rol Bazlı Dinamik Menü Sistemi

**Prompt:**

```
Bir Vinç Yönetim Sistemi için **ASP.NET Core Web API + Vue 3 Admin Panel** mimarisinde çalışan **rol bazlı dinamik menü sistemi** oluştur.

GENEL MİMARİ
* Backend: ASP.NET Core Web API | Frontend: Vue 3 + Pinia + Vue Router
* Authentication: JWT | Authorization: RBAC
* Menü sistemi tamamen veritabanından yönetilmelidir; kullanıcının rolüne göre menüler otomatik oluşmalıdır

VERİTABANI TABLOLARI
* Users: Id, Name, Surname, Email, Phone, PasswordHash, RoleId, Status, CreatedDate
* Roles: Id, RoleName, Description
* Permissions: Id, ModuleName, ActionName (View, Create, Update, Delete, Export)
* RolePermissions: Id, RoleId, PermissionId
* Menus: Id, Title, Icon, Route, ParentId, OrderNo, ModuleName, IsActive

MENÜ SİSTEMİ (Ana + alt menüler)
1. Dashboard → Genel Durum, Günlük İşler
2. Vinç Yönetimi → Vinç Listesi, Vinç Ekle, Vinç Bakım Takibi, Vinç Kullanım Geçmişi
3. Operasyon Yönetimi → Kiralama Listesi, Yeni Kiralama, Görev Planlama, Operasyon Takibi
4. Müşteri Yönetimi → Müşteri Listesi, Müşteri Ekle, Müşteri Projeleri
5. Teklif Yönetimi → Teklif Listesi, Yeni Teklif, Teklif Onayları
6. Fatura ve Muhasebe → Fatura Listesi, Gelirler, Giderler
7. Bakım Yönetimi → Bakım Planı, Arıza Kayıtları, Servis Geçmişi
8. Operatör Yönetimi → Operatör Listesi, Operatör Ekle, Operatör Görevleri
9. Raporlama → Vinç Kullanım Raporu, Kiralama Raporu, Gelir Raporu, Operatör Performans Raporu
10. CMS Yönetimi → Slider, Haber, Galeri, Hizmetler, Referanslar, Sayfalar, İletişim Bilgileri
11. Sistem Yönetimi → Kullanıcı Yönetimi, Rol Yönetimi, Yetki Yönetimi, Menü Yönetimi, Log Kayıtları
12. Ayarlar → Genel Ayarlar, SMS Ayarları, Email Ayarları, API Ayarları

MENÜ ÖZELLİKLERİ
* ParentId ile hiyerarşik yapı; OrderNo ile sıralama; ikonlar sidebar'da
* Girişte API'den menüler çekilsin; Vue'da router dinamik oluşturulsun; rolüne göre yetkisiz menüler gizlensin

API ENDPOINTLERİ
* GET /api/menu/user → Rolüne göre menüler | GET /api/menu → Tüm menüler (Admin)
* POST /api/menu | PUT /api/menu/{id} | DELETE /api/menu/{id}

FRONTEND (Vue 3)
* SidebarMenu component; menüleri recursive göster; router guard ile yetkisiz sayfa engeli
* Menü ikonları: FontAwesome veya Material Icons | Pinia: auth + menu state

EKSTRA
* Menü tamamen dinamik; yeni modül menüden eklenebilmeli
* Admin panelinde menü yönetim ekranı; sürükle bırak ile sıralama (OrderNo güncelleme)
* Modüler, genişletilebilir, kurumsal seviyede sistem.
```

---

## Adım 17 – Vue 3 Admin Panel İç Sayfalarının İmplementasyonu

**Prompt:**

```
admin-vue projesinde şu an çoğu rota Placeholder.vue ile "Bu sayfa henüz implemente edilmedi" gösteriyor. Bu sayfaları, mevcut CraneManagementSystem.API endpoint'leriyle entegre ederek implemente et.

HEDEF
* Placeholder kullanan tüm rotalar için gerçek liste/form/rapor sayfaları yaz.
* API: src/CraneManagementSystem.API içindeki controller'lar (api/cranes, api/companies, api/work-plans, api/dashboard, api/news, api/sliders, api/gallery, api/users, api/menu, api/systemsettings, api/incomeexpense, api/maintenance, api/reports vb.) kullanılacak.
* Tasarım: Mevcut AdminLayout, Dashboard ve Login ile uyumlu; aynı renk paleti (örn. #111827, #374151, #6b7280, #dbeafe, #2563eb), kart yapısı ve buton stilleri.

ORTAK BİLEŞENLER (önce oluştur)
* DataTable: Tablo + sayfalama (page, pageSize) + sıralama (opsiyonel) + boş durum mesajı. Slot ile özel hücre desteği.
* FormCard: Başlık + form alanları + Kaydet / İptal butonları; loading ve hata mesajı alanı.
* PageHeader: Sayfa başlığı + isteğe bağlı "Yeni Ekle" veya aksiyon butonu.
* LoadingSpinner ve EmptyState (liste boşken) bileşenleri.

SAYFA GRUPLARI VE API EŞLEŞMESİ

1) Dashboard (zaten var; gerekirse genişlet)
* /dashboard, /dashboard/overview, /dashboard/daily → GET /api/dashboard/summary (veya ilgili endpoint). Özet kartlar (firma sayısı, vinç sayısı, vb.) ve günlük işler listesi.

2) Vinç Yönetimi
* /cranes → GET /api/cranes (sayfalı liste). Kolonlar: Kod, Ad, Tip, Konum, Durum, Atanan Operatör; Düzenle/Sil aksiyonları.
* /cranes/new → POST /api/cranes (form: Code, Name, Type, Location, Status, ConstructionSiteId, AssignedOperatorId).
* /cranes/maintenance → GET /api/maintenance (bakım kayıtları listesi).
* /cranes/history → Vinç kullanım/atama geçmişi (work-plans veya ilgili API ile).

3) Operasyon / Kiralama / İş Planı
* /operations/rentals, /operations/rentals/new → İş planı veya kiralama ile ilişkili (GET/POST /api/work-plans, /api/companies).
* /work-plans → GET /api/work-plans (sayfalı). Plan listesi + yeni plan formu veya ayrı sayfa.
* /operations/tracking → İş planı durumları veya operasyon takip listesi.

4) Müşteri (Firma) Yönetimi
* /companies → GET /api/companies (sayfalı liste). Kolonlar: Ad, Vergi No, İletişim; Düzenle/Sil.
* /companies/new → POST /api/companies (form: Name, TaxNumber, Address, Phone, Email).
* /companies/projects → Firma–proje ilişkisi (work-plans veya construction-sites ile; GET /api/construction-sites, /api/work-plans).

5) Teklif Yönetimi
* /quotes, /quotes/new, /quotes/approvals → Backend'de teklif endpoint'i yoksa "Yakında" bilgisi veya work-plans/progress-payments ile basit bir liste; varsa GET/POST ilgili API.

6) Fatura ve Muhasebe
* /finance/invoices → Hakediş/fatura benzeri (GET /api/progresspayments veya ilgili endpoint).
* /income-expense → GET /api/incomeexpense (sayfalı; gelir/gider filtre). Liste + isteğe bağlı ekleme formu.

7) Bakım Yönetimi
* /maintenance/plan, /maintenance/records → GET /api/maintenance (kayıt listesi; plan için aynı veya filtre).
* /maintenance/history → GET /api/maintenance (geçmiş listesi).

8) Operatör Yönetimi
* /operators → GET /api/users (rol veya tip filtre ile operatörler). Liste + düzenleme linki.
* /operators/new → POST /api/auth/register veya kullanıcı ekleme endpoint'i (varsa).
* /operators/assignments → GET /api/operatorassignments (veya atama listesi endpoint'i).

9) Raporlama
* /reports/crane-usage, /reports/rentals, /reports/income, /reports/operator → GET /api/reports (veya ilgili rapor endpoint'leri). Tarih aralığı filtresi + tablo/özet gösterimi; export (CSV/Excel) varsa buton.

10) CMS Yönetimi
* /cms/sliders → GET/POST/PUT/DELETE /api/sliders. Liste + ekleme/düzenleme formu (Title, ShortText, ImageUrl, SortOrder, IsActive).
* /cms/news → GET/POST/PUT/DELETE /api/news. Liste + form (Title, Summary, Body, ImageUrl, IsPublished, PublishedAt).
* /cms/gallery → GET/POST/PUT/DELETE /api/gallery. Liste + form (Title, ImageUrl, SortOrder, IsActive).
* /cms/services, /cms/references, /cms/pages → SystemSetting veya özel endpoint varsa kullan; yoksa "Yakında" veya basit anahtar-değer formu.
* /cms/contact → GET/PUT /api/about veya iletişim ayarları (system settings) formu.

11) Sistem Yönetimi
* /users → GET/POST/PUT/DELETE /api/users (Admin). Kullanıcı listesi + rol atama.
* /system/roles → Rol listesi (GET /api/menu veya Roles tablosu için endpoint yoksa "Yakında").
* /system/permissions → Yetki listesi (backend'de endpoint yoksa "Yakında").
* /system/menus → GET /api/menu (tüm menü ağacı) + düzenleme (PUT /api/menu/{id}), sıra (PUT /api/menu/{id}/reorder), yeni menü (POST /api/menu). Ağaç görünümü veya liste + ParentId/OrderNo düzenleme.
* /system/logs → Backend'de log endpoint'i yoksa "Yakında".

12) Ayarlar
* /settings/general, /settings/sms, /settings/email, /settings/api → GET/PUT /api/systemsettings (Key-Value). Her sayfa ilgili anahtar(lar) için form (örn. Genel Ayarlar: SiteAdi, LogoUrl; Email: SmtpHost, FromAddress).

TEKNİK KURALLAR
* Proje: admin-vue (Vue 3 + Vite + Pinia + Vue Router). API client: src/api/client.js (axios, baseURL = VITE_API_URL).
* Sayfa bileşenleri: src/views/ altında mantıksal gruplara göre (örn. cranes/CraneList.vue, cranes/CraneForm.vue; companies/CompanyList.vue; cms/SliderList.vue).
* Router: Mevcut router/index.js'teki path'ler aynı kalsın; sadece component import'ları yeni view'lara yönlendirilsin.
* Liste sayfalarında sayfalama: page, pageSize query veya state ile API'ye iletilmeli (API'de varsa).
* Form validasyonu: Basit required/format kontrolü; hata mesajları form altında veya alan yanında gösterilsin.
* 403/401: API hata dönerse kullanıcıyı login'e yönlendir veya "Yetkiniz yok" mesajı göster (api client'ta interceptor zaten 401'de login'e atıyor olabilir).

ÖNCELİK (istersen aşamalı uygula)
* Faz 1: Dashboard (genişlet), Vinç listesi/form, Firma listesi/form, Kullanıcı listesi, Menü yönetimi.
* Faz 2: İş planları, Gelir-gider, Bakım kayıtları, Operatör listesi/atamalar.
* Faz 3: CMS (Slider, Haber, Galeri), Raporlar, Ayarlar.
* Faz 4: Teklif, Fatura, Rol/Yetki, Log (veya "Yakında" sayfaları).

Çıktı: Placeholder.vue kullanan rotaların tamamı (veya belirlenen fazlar) gerçek içerikle çalışır; liste/form/rapor sayfaları ilgili API'lerle entegre; tasarım mevcut panel ile uyumlu.
```

---

## Kullanım Notu

- Her promptu sırayla kullan; bir sonraki adım öncekine bağlı olabilir.
- "PLAN.md" veya "Cursor’daki plan" derken workspace’teki [PLAN.md](PLAN.md) ve Cursor plan dosyasındaki detaylı bölümler kastedilmektedir.
- Gerekirse bir adımı "şu projede, şu klasörde" diye netleştirerek iki ayrı prompta bölebilirsin.
