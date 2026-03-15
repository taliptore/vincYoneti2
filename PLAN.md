# Vinç Yönetim Sistemi – ASP.NET Core Web API Backend Planı

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

Detaylı DTO, Repository, Service, Controller listesi ve rol bazlı yetkilendirme PLAN.md'nin tam sürümünde (Cursor plan dosyasında) yer alır.

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
