# Adım 11 – Test ve Doğrulama Rehberi

Bu dokümanda API'nin Swagger ve entegrasyon testleri ile nasıl doğrulanacağı anlatılır.

## Ön koşul

- Veritabanı (TOREVINCDB) erişilebilir olmalı.
- **Entegrasyon testleri çalıştırmadan önce API'yi durdurun** (testler kendi in-memory host'unu kullanır; build sırasında API çalışıyorsa DLL kilitlenir).

---

## 1. API'yi çalıştırma

```bash
cd d:\proje\VincYonetimiSistemi
dotnet run --project src/CraneManagementSystem.API/CraneManagementSystem.API.csproj
```

- Uygulama başlarken seed çalışır (Admin, Muhasebe, Operatör, Firma kullanıcıları).
- Örnek adres: `http://localhost:5116` (port farklı olabilir).
- Swagger: `https://localhost:7xxx/swagger` veya `http://localhost:5xxx/swagger`.

---

## 2. Swagger ile Login testi

1. Swagger UI'da **POST /api/auth/login** endpoint'ini açın.
2. **Try it out** → Request body:

```json
{
  "email": "admin@torevinc.com",
  "password": "Admin123!"
}
```

3. **Execute** → 200 dönmeli, response'ta `token`, `email`, `role` (0=Admin) olmalı.

---

## 3. JWT ile korumalı endpoint'leri test etme

1. Response'tan `token` değerini kopyalayın.
2. Swagger sayfasının üstündeki **Authorize** butonuna tıklayın.
3. **Value** alanına: `Bearer <token>` yazın (örnek: `Bearer eyJhbGc...`).
4. **Authorize** → **Close**.
5. **GET /api/dashboard/summary** veya **GET /api/companies** çalıştırın → 200 dönmeli.

---

## 4. Rol bazlı erişim (403) kontrolü

1. **Authorize** ile mevcut token'ı temizleyin veya Firma token kullanın.
2. Firma ile giriş: `firma@torevinc.com` / `Firma123!` → token alın.
3. Bu token ile **GET /api/users** çağırın → **403 Forbidden** dönmeli (Users sadece Admin).
4. Aynı token ile **GET /api/companies** → 200 (Firma kendi firmasını görür).

---

## 5. Exception middleware (hata formatı) kontrolü

1. Geçersiz bir GUID ile **GET /api/companies/00000000-0000-0000-0000-000000000001** → **404**, body'de `application/json` ve `ProblemDetails` (title, detail, status) olmalı.
2. Token olmadan **GET /api/dashboard/summary** → **401 Unauthorized**.

---

## 6. Entegrasyon testlerini çalıştırma

**API çalışırken test projesi build edilmez** (DLL kilitlenir). Önce API'yi durdurun (Ctrl+C), sonra:

```bash
cd d:\proje\VincYonetimiSistemi
dotnet test tests/CraneManagementSystem.API.IntegrationTests/CraneManagementSystem.API.IntegrationTests.csproj
```

Testler şunları doğrular:

- **Login_WithSeedAdmin_Returns200_AndToken**: Seed Admin ile giriş → 200 ve token.
- **Login_WithInvalidPassword_Returns401**: Yanlış şifre → 401.
- **Dashboard_WithValidAdminToken_Returns200**: Admin token ile dashboard → 200.
- **Dashboard_WithoutToken_Returns401**: Token yok → 401.
- **Users_WithFirmaToken_Returns403**: Firma token ile /api/users → 403.
- **Home_AllowAnonymous_Returns200**: /api/home → 200 (AllowAnonymous).
- **GetCompanyNonExistentId_Returns404**: Olmayan company → 404 ve JSON.

---

## Seed kullanıcıları (test için)

| Rol       | E-posta               | Şifre        |
|----------|------------------------|--------------|
| Admin    | admin@torevinc.com      | Admin123!    |
| Muhasebe | muhasebe@torevinc.com   | Muhasebe123! |
| Operatör | operator@torevinc.com  | Operator123! |
| Firma    | firma@torevinc.com     | Firma123!    |
