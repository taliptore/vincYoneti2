# Senaryo İncelemesi ve Örnek Veri

## Senaryoları Çalıştırma

- **Tek tık:** `SenaryolariCalistir.ps1` çalıştırın (PowerShell). API ve Vue yoksa açar; varsa adresleri yazar.
- **Manuel:**
  1. **API:** `src/CraneManagementSystem.API` → `dotnet run` (http://localhost:5116)
  2. **Admin panel:** `admin-vue` → `npm run dev` (http://localhost:5173 veya 5174)
- **Giriş:** `appsettings.json` içindeki `Seed:Admin:Email` ve `Seed:Admin:Password` ile giriş yapın.

## Örnek Veri (Seed)

API ilk çalıştırmada (veya veritabanında firma/vinç yoksa) **EnsureSampleDataAsync** ile aşağıdaki örnek veriler eklenir:

| Modül | Örnek veri |
|-------|------------|
| **Firmalar** | ABC İnşaat A.Ş., XYZ Yapı Ltd. |
| **Şantiyeler** | Kartal Proje Sahası, Çankaya Şantiye |
| **Vinçler** | VNC-001 (Mobil), VNC-002 (Kule); operatör atanmış olabilir |
| **İş planları** | Kartal A Blok, Çankaya Temel Çalışması |
| **Hakediş** | 1 adet (50.000 TL, Beklemede) |
| **Gelir-gider** | 1 gelir (25.000 TL), 1 gider (5.000 TL) |
| **Bakım** | 1 periyodik bakım kaydı |
| **Slider** | 2 slider (TORE VINC Hoş Geldiniz, Güvenilir ve Hızlı) |
| **Haber** | 2 haber (1 yayında, 1 taslak) |
| **Galeri** | 2 galeri öğesi |
| **Sistem ayarları** | SiteAdi (TORE VINC), LogoUrl, SiteAciklama, SmtpHost, FromAddress |

## Sistemi İnceleme Adımları

1. **API’yi çalıştır**
   - `src/CraneManagementSystem.API` → `dotnet run`
   - Veritabanı boşsa seed ile kullanıcılar + örnek veriler oluşur.

2. **Admin Vue’yu çalıştır**
   - `admin-vue` → `npm run dev`
   - Tarayıcıda panel adresi (örn. http://localhost:5173).

3. **Giriş**
   - `appsettings.json` veya ortam değişkenlerindeki Seed:Admin email/şifre ile giriş yap (örn. Seed:Admin:Email, Seed:Admin:Password).

4. **Kontrol edilebilecek senaryolar**
   - **Dashboard:** Özet kartlar (vinç, firma, gelir/gider vb.) ve sayılar.
   - **Vinçler:** Liste, yeni vinç, düzenle, şantiye/operatör atama.
   - **Firmalar:** Liste, yeni firma, düzenle.
   - **İş planları:** Liste, yeni plan, vinç/şantiye/firma seçimi, düzenle.
   - **Gelir-gider:** Liste, filtre (gelir/gider), yeni kayıt, düzenle.
   - **Bakım:** Liste, vinç filtresi, yeni bakım kaydı, düzenle.
   - **Operatörler:** Operatör listesi (rol=Operatör), atamalar (vinç–operatör).
   - **Teklif / Fatura:** Teklif listesi, Fatura listesi, yeni hakediş, düzenle.
   - **Raporlar:** Gelir raporu (tarih aralığı, özet + detay).
   - **CMS:** Slider, Haber, Galeri listesi ve formları (yeni/düzenle/sil).
   - **Ayarlar:** Genel, SMS, E-posta, API (key-value kaydet).
   - **Menü:** Menü ağacı, yeni menü, düzenle, sıra, sil.
   - **Kullanıcılar:** Kullanıcı listesi (Admin).

5. **Yakında sayfaları**
   - Raporlar: Vinç kullanım, Kiralama, Operatör (ComingSoon).
   - CMS: Hizmetler, Referanslar, Sayfalar, İletişim.
   - Sistem: Rol, Yetki, Log.
   - Teklif onayları.

## Notlar

- Seed kullanıcılar için `appsettings.json` içinde `Seed:Admin`, `Seed:Muhasebe`, `Seed:Operatör`, `Seed:Firma` altında Email, Password, FullName (isteğe bağlı) tanımlanmalıdır.
- Örnek veri yalnızca firma/vinç yoksa eklenir; tekrar çalıştırmada aynı veriler tekrar eklenmez.
