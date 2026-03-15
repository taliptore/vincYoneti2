# TORE VINC – Vinç Yönetim Sistemi

**TORE VINC** firması için geliştirilen vinç yönetim sistemi: **Web API backend** + **yönetim paneli** (web) + **mobil uygulama** (iOS/Android). Backend: ASP.NET Core 8, Clean Architecture, EF Core, MSSQL, JWT, Swagger. Panel ve mobil aynı API'yi kullanır.

**Repository:** [https://github.com/taliptore/vincYoneti2](https://github.com/taliptore/vincYoneti2)

## İçerik

- **PLAN.md** — Detaylı mimari ve uygulama planı (katmanlar, entity'ler, modüller, rol bazlı yetkilendirme, yevmiye/mesai/hakediş/gelir-gider, **mobil uygulama** vb.).

## Teknolojiler

- ASP.NET Core 8
- Entity Framework Core
- MSSQL
- JWT Authentication
- Swagger

## Katmanlar

| Katman | Açıklama |
|--------|----------|
| **API** | Controllers, Middleware, Authentication |
| **Application** | DTOs, Services, Interfaces |
| **Domain** | Entity modelleri |
| **Infrastructure** | Repository, JWT, Email servisleri |
| **Persistence** | DbContext, Entity configuration |

## Roller

- **Admin** — Tam yetki
- **Muhasebe** — Hakediş, yevmiye, mesai, gelir-gider, raporlar
- **Operatör** — Vinç, şantiye, bakım, yakıt, kendi yevmiye/mesai
- **Firma** — Kendi firması, kendi hakedişleri, randevular

**Mobil uygulama:** Aynı API'ye bağlanır; teknoloji seçenekleri .NET MAUI, Flutter, React Native. Rol bazlı ekranlar (Operatör: yakıt, bakım, yevmiye/mesai; Firma: randevu, hakediş; Admin/Muhasebe: özet, onaylar).

Proje henüz uygulama aşamasına geçmemiştir; PLAN.md'deki adımlar takip edilerek geliştirme yapılacaktır.
