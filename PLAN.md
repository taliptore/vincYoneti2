# TORE VINC – Vinç Yönetim Sistemi | Backend + Panel + Mobil Planı

Bu sistem **TORE VINC** firması için geliştirilir ve firmaya aittir. **Web API backend** (tek kaynak), **web tarafı** (ana sayfa + yönetim paneli), **mobil uygulama** (iOS/Android).

## Mimari Özet

- **Bağımlılık yönü:** API → Application ve API → Infrastructure. Application → Domain. Infrastructure → Application (interface implementasyonları), Infrastructure → Persistence, Persistence → Domain. Domain hiçbir katmana bağımlı değil.

## 1. Solution ve Projeler

- **Solution adı:** `CraneManagementSystem.sln`
- **Projeler:** Domain (Class Library), Application (Class Library), Persistence (Class Library), Infrastructure (Class Library), API (ASP.NET Core Web API)

## 2. Domain Katmanı

- **Base entity:** BaseEntity (Id, CreatedAt, UpdatedAt)
- **Roller:** UserRole — Admin, Muhasebe, Operatör, Firma
- **Entity'ler:** User, Company, Crane, ConstructionSite, WorkPlan, ProgressPayment, DailyWageRecord (Yevmiye), OvertimeRecord (Mesai), IncomeExpense (Gelir gider), FuelRecord, MaintenanceRecord, SystemSetting, News, Announcement, ContactMessage, Appointment

## 3–6. Application, Persistence, Infrastructure, API

Detaylı DTO, Repository, Service, Controller listesi ve rol bazlı yetkilendirme planın tam sürümünde yer alır.

## Web Tarafı – Ana Sayfa ve Yönetim Paneli

- **Tek kaynak:** Mevcut CraneManagementSystem.API kullanılır; ayrı backend yok.
- **İki ayrı arayüz:**
  1. **Ana sayfa (public):** Ziyaretçiye açık landing sayfası. Haberler, duyurular, iletişim formu, randevu talebi. API: `/api/home`, `/api/news`, `/api/announcements`, `/api/contact`, `/api/appointments` (POST randevu).
  2. **Yönetim paneli:** Giriş (JWT) sonrası rol bazlı arayüz. Dashboard, firmalar, vinçler, operatörler, şantiyeler, iş planlama, hakediş, yevmiye, mesai, gelir-gider, yakıt, bakım, raporlar, kullanıcı yönetimi, sistem ayarları. Yetkiler “7. Yönetim Paneli Modülleri” rol matrisine göre.
- **Teknoloji seçenekleri:** ASP.NET Core MVC / Razor Pages (sunucu taraflı) veya SPA (React, Vue, Blazor) ayrı proje; API Base URL ayarlanabilir.
- **Konum:** `src/CraneManagementSystem.Web` veya `src/ToreVinc.Web`; solution’a eklenir. Geliştirme ortamında API (örn. `http://localhost:5116`) ile aynı veya farklı portta çalışır.

## Mobil Uygulama

- **Aynı Web API** kullanılır; ayrı backend yok. JWT ile giriş.
- **Teknoloji seçenekleri:** .NET MAUI, Flutter, React Native.
- **Rol bazlı mobil ekranlar:** Admin (özet, onaylar); Muhasebe (hakediş, yevmiye, mesai, gelir-gider); Operatör (vinç/şantiye, yakıt girişi, bakım, yevmiye/mesai); Firma (randevu, kendi hakedişleri).
- Proje: ayrı solution veya `src/ToreVinc.Mobile` (MAUI) / `mobile/` (Flutter/RN).

## 7. Yönetim Paneli Modülleri (Rol Matrisi)

| Modül | Admin | Muhasebe | Operatör | Firma |
|-------|:-----:|:--------:|:--------:|:-----:|
| Dashboard | Evet | Evet | Evet | Evet |
| Firmalar | Evet | Görüntüleme | Hayır | Kendi firması |
| Vinçler | Evet | Görüntüleme | Evet | Görüntüleme |
| Operatörler | Evet (atama sadece Admin) | Hayır | Görüntüleme | Hayır |
| Şantiyeler | Evet | Görüntüleme | Evet | Hayır |
| İş Planlama | Evet | Görüntüleme | Evet | Hayır |
| Hakediş | Evet | Evet | Görüntüleme | Kendi hakedişleri |
| Yevmiye | Evet | Evet | Kendi kayıtları | Hayır |
| Mesai | Evet | Evet | Kendi kayıtları | Hayır |
| Gelir gider | Evet | Evet | Görüntüleme | Hayır |
| Yakıt Takibi | Evet | Görüntüleme | Evet | Hayır |
| Bakım | Evet | Hayır | Evet | Hayır |
| Raporlar | Evet | Evet | Evet | Kendi raporları |
| Kullanıcı Yönetimi | Evet | Hayır | Hayır | Hayır |
| Sistem Ayarları | Evet | Hayır | Hayır | Hayır |

Tam plan metni (tüm bölümler, entity açıklamaları, plan analizi) Cursor plan dosyasında saklanmaktadır; uygulama sırasında referans alınacaktır.
