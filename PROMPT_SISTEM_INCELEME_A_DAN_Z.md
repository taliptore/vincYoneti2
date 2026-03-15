# Prompt: Sistemi A’dan Z’ye İnceleme ve Hata Giderme

Bu promptu Cursor veya bir AI asistanına vererek **TORE VINC Vinç Yönetim Sistemi**nin tüm kod tabanının incelenmesini ve hataların giderilmesini isteyebilirsin.

---

## Yapıştırılacak prompt

```
TORE VINC Vinç Yönetim Sistemi projesini A'dan Z'ye incele, tüm sistemi kontrol et ve hataları gider.

KAPSAM
1. Backend (API)
   - src/CraneManagementSystem.API: Controller'lar, DataSeeder, Program.cs
   - src/CraneManagementSystem.Application: DTO'lar, servisler, interface'ler
   - src/CraneManagementSystem.Infrastructure: Repository'ler, DI
   - src/CraneManagementSystem.Persistence: DbContext, migration'lar
   - src/CraneManagementSystem.Domain: Entity'ler, enum'lar
   - Kontrol: Eksik [Authorize], yanlış route/HTTP metot, null reference riski, exception handling, CORS

2. Admin Vue (admin-vue)
   - src/views: Tüm liste/form/rapor sayfaları
   - src/router/index.js: Tüm route'ların doğru component'e bağlı olması
   - src/api/client.js: Base URL, interceptor (401 yönlendirme)
   - src/components/ui: DataTable, FormCard, PageHeader, LoadingSpinner, EmptyState
   - src/stores: auth, menu
   - Kontrol: API endpoint path'leri (örn. /api/cranes, /api/workplans) backend ile aynı mı; eksik import; v-model/emit; boş liste/loading/hata durumları; form validasyonu

3. Tutarlılık
   - Vue sayfalarındaki API çağrıları (GET/POST/PUT/DELETE) backend controller route'ları ile eşleşmeli (ASP.NET [controller] = Controller adı sans "Controller").
   - PagedResult: items, totalCount, page, pageSize alanları backend ile aynı mı (PascalCase vs camelCase - JSON serializer ayarı).
   - Foreign key: Form'larda dropdown için kullanılan API'ler (örn. /api/constructionsites, /api/companies) mevcut mu?

4. Lint ve derleme
   - admin-vue: ESLint / Vue hatası var mı?
   - API: dotnet build hatasız mı?

5. Kritik senaryolar
   - Login → JWT → menü yükleme (GET /api/menu/user) → Dashboard (GET /api/dashboard/summary)
   - Vinç listesi → düzenle → kaydet; Firma listesi → yeni → kaydet
   - İş planı, gelir-gider, bakım, CMS (slider/haber/galeri), ayarlar, rapor sayfaları en az bir kez mantıken doğru mu?
   - 401/403/404/500 durumlarında kullanıcıya anlamlı mesaj veya yönlendirme var mı?

ÇIKTI
- Tespit edilen her hata için: dosya, satır/konum, kısa açıklama, önerilen düzeltme.
- Düzeltmeleri doğrudan uygula (kod değişikliği yap).
- Özet: Kaç hata bulundu, kaçı giderildi, varsa giderilemeyen (örn. backend eksik endpoint) not et.
```

---

## Kullanım

1. Bu dosyadaki "Yapıştırılacak prompt" bölümündeki metni kopyala.
2. Cursor sohbetine yapıştır ve gönder.
3. Asistan tüm projeyi tarayıp hataları listeleyecek ve düzeltecek; özet rapor verecek.

## İsteğe bağlı ek kontroller

- **Güvenlik:** Şifre/API key ortam değişkeninde mi, log’lara yazılmıyor mu?
- **Performans:** Liste sayfalarında gereksiz büyük pageSize (örn. 1000) kullanımı var mı?
- **Erişilebilirlik:** Form label’ları, buton metinleri Türkçe ve tutarlı mı?
