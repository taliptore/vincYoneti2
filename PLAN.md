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
  1. **Ana sayfa (public):** Ziyaretçiye açık landing. Haberler, duyurular, iletişim formu, randevu talebi. API: `/api/home`, `/api/news`, `/api/announcements`, `/api/contact`, `/api/appointments` (POST randevu).
  2. **Yönetim paneli:** Giriş (JWT) sonrası rol bazlı arayüz. Dashboard, firmalar, vinçler, operatörler, şantiyeler, iş planlama, hakediş, yevmiye, mesai, gelir-gider, yakıt, bakım, raporlar, kullanıcı yönetimi, sistem ayarları. Yetkiler “7. Yönetim Paneli Modülleri” rol matrisine göre.
- **Teknoloji seçenekleri:** ASP.NET Core MVC / Razor Pages veya SPA; API Base URL ayarlanabilir.
- **Konum:** `src/CraneManagementSystem.Web`; solution’a eklenir.

### Ana Sayfa İçerikleri (CMS ile yönetilen)

Ana sayfada aşağıdaki bölümler yer alır; **tüm içerikler yönetim panelinden (Admin / yetkili roller) eklenip düzenlenir:**

| Bölüm | Açıklama | Panel’den yönetim |
|-------|----------|-------------------|
| **Slider** | Üstte dönen banner’lar (başlık, kısa metin, görsel, sıra, aktif/pasif) | Slider CRUD (Admin) |
| **Haber** | Son haberler listesi / grid; ana sayfada öne çıkanlar | Mevcut News CRUD |
| **Galeri** | Vinç/şantiye/proje görselleri; grid veya lightbox | Galeri CRUD (Admin) |
| **İletişim** | İletişim formu + adres/telefon/e-posta bilgisi | Form API + Site ayarları veya İletişim sayfası içeriği |
| **Hakkımızda** | Firma tanıtım metni, misyon, vizyon (metin/görsel) | Hakkımızda sayfası içeriği (tek sayfa CMS) |

- **Backend:** Gerekirse Domain’e Slider, Gallery (GalleryItem) entity’leri; API’de ilgili controller’lar (örn. `/api/slider`, `/api/gallery`). Hakkımızda için SystemSetting veya ayrı AboutPage entity.
- **Public site:** Ana sayfa bu API’lerden veriyi çekerek slider, haber, galeri, iletişim ve hakkımızda bölümlerini gösterir.

### Ana Sayfa Yapısı (Public Site Layout)

Ana sayfa ve site iskeleti aşağıdaki bloklardan oluşur; içerikler CMS/panelden yönetilir.

| Bölüm | İçerik | Not |
|-------|--------|-----|
| **HEADER** | Logo, Menü, Telefon, Teklif Al butonu | Sabit üst alan; menü ana sayfa / hizmetler / hakkımızda / referanslar / galeri / haberler / iletişim vb. |
| **SLIDER** | Slider görselleri, Başlık, Açıklama, Buton | Carousel; her slaytta görsel + başlık + kısa açıklama + CTA butonu (örn. Teklif Al) |
| **HİZMETLER** | Sepetli Vinç, Mobil Vinç, Platform, Kiralama | Kart veya ikonlu bloklar; her hizmet için başlık/kısa metin/link |
| **HAKKIMIZDA** | Kısa tanıtım, “Devamını oku” linki | Ana sayfada özet metin; “Devamını oku” → Hakkımızda sayfası |
| **REFERANSLAR** | Firma logoları | Logo listesi (carousel veya grid); CMS’ten yönetilebilir referans entity/ayar |
| **GALERİ** | Proje fotoğrafları | Grid; lightbox ile büyütme; mevcut GalleryItem API |
| **HABERLER / BLOG** | Firma haberleri | Son haberler grid/liste; mevcut News API |
| **İLETİŞİM** | Harita, Adres, Telefon, İletişim formu | Yan yana veya iki sütun: harita + adres/telefon | form |
| **FOOTER** | Sosyal medya, Hızlı linkler | İkonlar (Facebook, Instagram, LinkedIn vb.), menü linkleri; site ayarlarından URL’ler |

- **Teklif Al:** Header ve slider butonu → Randevu/teklif formu sayfasına veya iletişim formuna yönlendirilebilir.
- **Hizmetler:** Sabit dört başlık (Sepetli Vinç, Mobil Vinç, Platform, Kiralama) veya CMS’ten yönetilebilir hizmet listesi.
- **Referanslar:** Gerekirse Domain’e Referans/PartnerLogo entity; API ve panelde CRUD.

### Yönetim Paneli – Menü ve Rol Bazlı Ekranlar

- **Menü sistemi:** Sol sidebar veya üst nav; menü öğeleri **role göre** gösterilir. Her modül (Dashboard, Firmalar, Vinçler, …) için yetki matrisi “7. Yönetim Paneli Modülleri” ile aynı olacak şekilde bağlanır.
- **Rol bazlı ekranlar:** Admin tüm ekranlara erişir; Muhasebe, Operatör, Firma sadece yetkili oldukları menü öğelerini ve ekranları görür. Yetkisiz sayfaya gidildiğinde 403 veya login’e yönlendirme.
- **Tüm roller için ekranlar:** Dashboard (hepsi), Firmalar (Admin/Muhasebe/Firma), Vinçler (Admin/Muhasebe/Operatör/Firma), Operatörler, Şantiyeler, İş Planlama, Hakediş, Yevmiye, Mesai, Gelir-Gider, Yakıt, Bakım, Raporlar, Kullanıcı Yönetimi (Admin), Sistem Ayarları (Admin). Artı **CMS ekranları:** Slider, Haber, Galeri, Hakkımızda (ve gerekirse İletişim ayarları) — bu CMS ekranları Admin (ve istenirse belirli roller) ile sınırlı olabilir.

### CMS Tasarımı (Vinç Temalı)

- **Kapsam:** Yönetim panelinde içerik yönetimi (Slider, Haber, Galeri, Hakkımızda) için ekranlar; vinç / inşaat / ağır ekipman temasına uygun arayüz.
- **Hedef:** Listeleme (tablo veya kart), ekleme/düzenleme formları, sıralama, yayında/yayında değil, görsel yükleme (galeri/slider). Tasarım sade, okunabilir; renk ve ikonlar vinç sistemine uyumlu (kurumsal mavi, gri tonları).
- **Uygulama:** ADIM_ADIM_PROMPTLAR.md içindeki **"Adım 14 – Ana Sayfa CMS, Menü Sistemi ve Rol Ekranları"** promptu ile uygulanır.

### Web – Modern Tasarım

- **Kapsam:** Tüm public ve panel sayfaları; tutarlı, modern, responsive arayüz.
- **Hedef:** Profesyonel görünüm, okunabilir tipografi, tutarlı renk/boşluk, mobil uyum, erişilebilirlik (WCAG temel).
- **Uygulama:** ADIM_ADIM_PROMPTLAR.md içindeki **"Adım 12.1 – Web: Modern Tasarım (Tüm Sayfalar)"** promptu ile uygulanır.

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
