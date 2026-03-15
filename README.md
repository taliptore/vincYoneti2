# Vinç Yönetim Sistemi (Crane Management System)

ASP.NET Core 8 Web API backend projesi — Clean Architecture, EF Core, MSSQL, JWT, Swagger.

**Repository:** [https://github.com/taliptore/vincYoneti2](https://github.com/taliptore/vincYoneti2)

## İçerik

- **PLAN.md** — Detaylı mimari ve uygulama planı (katmanlar, entity'ler, modüller, rol bazlı yetkilendirme, yevmiye/mesai/hakediş/gelir-gider vb.).

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

Proje henüz uygulama aşamasına geçmemiştir; PLAN.md'deki adımlar takip edilerek geliştirme yapılacaktır.
