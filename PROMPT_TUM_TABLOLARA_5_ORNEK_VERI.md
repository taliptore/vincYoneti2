# Prompt: Tüm Tablolara 5'er Örnek Veri

Aşağıdaki metni kopyalayıp Cursor veya bir AI asistanına yapıştırarak kullanabilirsin.

---

## Yapıştırılacak prompt

```
TORE VINC Vinç Yönetim Sistemi projesinde site şu an çok boş; veri yok. Tüm ilgili tablolara tam olarak 5'er örnek veri eklenecek.

HEDEF
- Veritabanındaki aşağıdaki tabloların her birine tam 5 kayıt ekle (mevcut seed verisi az veya yoksa).
- Proje: src/CraneManagementSystem.API, seed sınıfı: Data/DataSeeder.cs, DbContext: Persistence/ApplicationDbContext.cs.
- Yöntem: DataSeeder içinde yeni bir metot ekle (örn. EnsureFullSampleDataAsync). Bu metot Program.cs içinde EnsureSampleDataAsync'ten sonra çağrılsın.
- Koşul: Tabloda zaten 5 veya daha fazla kayıt varsa o tabloyu atla; yoksa tam 5 kayıt ekle (toplam 5 olacak şekilde, eksik sayıyı tamamla).
- Foreign key ilişkilerine dikkat et: Önce Company, ConstructionSite, User (operatör), Crane gibi ana tablolar dolu olmalı; sonra WorkPlan, ProgressPayment, IncomeExpense, MaintenanceRecord, OperatorAssignment vb. ilişkili tablolar.

EKLENECEK TABLOLAR VE 5'ER KAYIT

1. Companies – 5 firma (Name, TaxNumber, Address, Phone, Email). Gerçekçi Türk firma adları.
2. ConstructionSites – 5 şantiye (Name, Address, StartDate, EndDate opsiyonel).
3. Cranes – 5 vinç (Code, Name, Type, Location, Status, ConstructionSiteId, AssignedOperatorId opsiyonel). Farklı tipler: Mobil, Kule, Paletli vb.
4. WorkPlans – 5 iş planı (Title, CraneId, ConstructionSiteId, PlannedStart, PlannedEnd, Status, CompanyId). Mevcut crane/site/company Id'lerini kullan.
5. ProgressPayments – 5 hakediş (WorkPlanId, CompanyId, Amount, Period, Status). Farklı dönem ve tutarlar.
6. IncomeExpenses – 5 gelir + 5 gider = toplam 10 kayıt (Type, Category, Amount, Date, Description, ReferenceType, CompanyId). Veya en az 5 kayıt (gelir ve gider karışık).
7. MaintenanceRecords – 5 bakım kaydı (CraneId, MaintenanceDate, Description, Type, NextDueDate). Farklı vinç ve tipler (Periyodik, Arıza, Revizyon).
8. Sliders – 5 slider (Title, ShortText, ImageUrl, SortOrder, IsActive). Ana sayfa için.
9. News – 5 haber (Title, Summary, Body, ImageUrl, IsPublished, PublishedAt). En az 3'ü yayında.
10. GalleryItems – 5 galeri öğesi (Title, ImageUrl, SortOrder, IsActive).
11. SystemSettings – en az 5 farklı Key (örn. SiteAdi, LogoUrl, SiteAciklama, SmtpHost, FromAddress). Value'lar anlamlı dolsun.
12. OperatorAssignments – 5 atama (UserId=operatör, CraneId, ConstructionSiteId, StartDate, EndDate). Mevcut User (Operatör) ve Crane/ConstructionSite Id'leri ile.
13. DailyWageRecords – 5 yevmiye kaydı (UserId, ConstructionSiteId, WorkPlanId opsiyonel, Date, Amount, Description, Status).
14. OvertimeRecords – 5 mesai kaydı (UserId, Date, Hours, Rate, Amount, IsApproved).
15. FuelRecords – 5 yakıt kaydı (CraneId, Quantity, Unit, Date, OperatorId).
16. Announcements – 5 duyuru (Title, Content, IsPinned, StartDate, EndDate).
17. ContactMessages – 5 iletişim mesajı (Name, Email, Phone, Subject, Message, IsRead).
18. Appointments – 5 randevu talebi (CustomerName, Email, Phone, PreferredDate, Notes, Status, CompanyId opsiyonel).

KURALLAR
- Tarihler Türkiye/UTC için mantıklı olsun (CreatedAt = DateTime.UtcNow veya geçmiş tarihler).
- Domain entity'lerini ve enum'ları kullan (IncomeExpenseType.Gelir/Gider, ReferenceType.Manuel vb.).
- Id'ler Guid.NewGuid() ile üretilsin.
- BaseEntity kullanan tablolarda CreatedAt mutlaka set edilsin.
- Önce Companies, ConstructionSites, Users (operatör) ve Cranes dolu değilse 5'e tamamla; sonra diğer tablolar.
- EnsureFullSampleDataAsync sadece geliştirme/test için çalışsın; production'da çalıştırılmasın (ör. ortam kontrolü veya appsettings'te bir anahtar ile kapatılabilir). Veya her tablo için "eğer kayıt sayısı < 5 ise 5'e tamamla" mantığı kullan.
```

---

## Kullanım

1. Bu dosyadaki "Yapıştırılacak prompt" bölümündeki tüm metni (üç tırnak dahil veya dahil değil) kopyala.
2. Cursor veya kullandığın AI sohbetine yapıştır ve gönder.
3. Asistan DataSeeder'a `EnsureFullSampleDataAsync` ekleyecek ve Program.cs'te çağıracak; gerekirse önce veritabanını sıfırlayıp migration ile tekrar oluşturduktan sonra API'yi çalıştırarak tüm tabloların 5'er kayıtla dolduğunu kontrol edebilirsin.

## Not

- Users tablosu için zaten Seed:Admin vb. kullanıcılar var; bu prompt ile ek olarak 5 operatör veya ek kullanıcı istemiyorsan, sadece OperatorAssignments ve DailyWage/Overtime için mevcut kullanıcı Id'leri kullanılır.
- Menus, Roles, MenuRoles, Permissions, RolePermission yapısal seed olduğu için "5'er kayıt" kapsamına alınmadı; istenirse ayrıca belirt.
