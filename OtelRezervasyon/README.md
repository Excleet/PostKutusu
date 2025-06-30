# Otel Rezervasyon Sistemi
## Valeria Antique Hotel Sultanahmet 1892

Bu proje, Valeria Antique Hotel için geliştirilmiş bir web tabanlı rezervasyon sistemidir.

## 🏗️ Proje Mimarisi

Proje 4 katmanlı mimari ile geliştirilmiştir:

### Katmanlar:
- **Otel.Web** - Presentation Layer (ASP.NET Core MVC)
- **Otel.BLL** - Business Logic Layer 
- **Otel.DAL** - Data Access Layer (Entity Framework Core)
- **Otel.Entity** - Entity ve DTO sınıfları

## 🛠️ Teknolojiler

- **Framework:** ASP.NET Core 8.0 MVC
- **ORM:** Entity Framework Core 8.0
- **Database:** SQL Server LocalDB
- **Authentication:** Cookie Authentication
- **Frontend:** HTML5, CSS3, JavaScript (Vanilla)
- **Icons:** Font Awesome 6.5.0

## 📋 Özellikler

### Müşteri Özellikleri:
- ✅ Kayıt olmadan rezervasyon yapabilme
- ✅ Oda tipi seçimi ve müsaitlik kontrolü
- ✅ Tarih validasyonu
- ✅ Responsive tasarım
- ✅ Otel bilgileri ve galeri görüntüleme

### Admin Özellikleri:
- ✅ Güvenli admin girişi
- ✅ Dashboard ile genel istatistikler
- ✅ Rezervasyon listesi ve filtreleme
- ✅ Rezervasyon onaylama/silme
- ✅ Rezervasyon detay görüntüleme

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler:
- .NET 8.0 SDK
- SQL Server LocalDB (Visual Studio ile gelir)
- Visual Studio Code veya Visual Studio 2022

### Adımlar:

1. **Projeyi İndirin:**
   ```bash
   git clone [repo-url]
   cd OtelRezervasyon
   ```

2. **Otomatik Build (Önerilen):**
   ```bash
   build.bat
   ```

3. **Manuel Build:**
   ```bash
   dotnet restore
   dotnet build
   cd Otel.Web
   dotnet ef migrations add InitialCreate --project ..\Otel.DAL\Otel.DAL.csproj
   dotnet ef database update --project ..\Otel.DAL\Otel.DAL.csproj
   ```

4. **Projeyi Çalıştırın:**
   ```bash
   dotnet run --project Otel.Web
   ```

5. **Tarayıcıda Açın:**
   - Ana Site: https://localhost:5001
   - Admin Panel: https://localhost:5001/Admin/Login

## 🔐 Varsayılan Admin Bilgileri

- **Kullanıcı Adı:** admin
- **Şifre:** 123456

> ⚠️ **Güvenlik Notu:** Prod ortamında mutlaka şifreyi değiştirin!

## 🏨 Oda Tipleri ve Fiyatları

| Oda Tipi | Alan | Kapasite | Fiyat/Gece |
|-----------|------|----------|------------|
| Economic Double Room | 13m² | 2 kişi | €40 |
| Deluxe Double Room | 17m² | 2 kişi | €55 |
| Standard Triple Room | 22m² | 3 kişi | €60 |
| Deluxe Triple Room | 24m² | 3 kişi | €80 |
| Deluxe Sultans Triple Room | 24m² | 3 kişi | €120 |

## 📁 Proje Yapısı

```
OtelRezervasyon/
├── Otel.Entity/
│   ├── Entities/
│   │   ├── Rezervasyon.cs
│   │   └── Admin.cs
│   └── DTOs/
│       ├── RezervasyonDto.cs
│       └── AdminLoginDto.cs
├── Otel.DAL/
│   ├── Data/
│   │   └── OtelDbContext.cs
│   └── Repositories/
│       ├── Abstract/
│       └── Concrete/
├── Otel.BLL/
│   └── Services/
│       ├── Abstract/
│       └── Concrete/
├── Otel.Web/
│   ├── Controllers/
│   ├── Views/
│   └── wwwroot/
└── OtelRezervasyon.sln
```

## 🌐 Sayfa Yapısı

### Public Sayfalar:
- `/` - Ana Sayfa
- `/Home/About` - Hakkımızda
- `/Home/Rooms` - Odalar
- `/Home/Services` - Hizmetler
- `/Home/Gallery` - Galeri
- `/Home/Contact` - İletişim
- `/Home/Rezervasyon` - Rezervasyon Formu

### Admin Sayfalar:
- `/Admin/Login` - Admin Giriş
- `/Admin/Dashboard` - Admin Ana Sayfa
- `/Admin/Rezervasyonlar` - Rezervasyon Listesi
- `/Admin/RezervasyonDetay/{id}` - Rezervasyon Detayı

## 🗄️ Veritabanı

### Tablolar:

**Rezervasyonlar:**
- Id (int, PK)
- AdSoyad (string)
- Email (string)
- Telefon (string)
- GirisTarihi (DateTime)
- CikisTarihi (DateTime)
- OdaTipi (string)
- MisafirSayisi (int)
- Mesaj (string, nullable)
- OlusturmaTarihi (DateTime)
- OnayDurumu (bool)

**Adminler:**
- Id (int, PK)
- KullaniciAdi (string)
- Sifre (string)
- AdSoyad (string, nullable)
- Email (string, nullable)
- OlusturmaTarihi (DateTime)
- SonGirisTarihi (DateTime, nullable)
- Aktif (bool)

## 🎨 Frontend Özellikler

- **Responsive Design:** Mobil ve masaüstü uyumlu
- **Image Slider:** Ana sayfada otomatik dönen resim galerisi
- **Form Validation:** Tarih kontrolü ve alan validasyonu
- **AJAX:** Oda müsaitlik kontrolü
- **Smooth Scrolling:** Sayfa içi navigasyon
- **Modern UI:** CSS Grid ve Flexbox kullanımı

## 🔧 Geliştirme Notları

### CSS Organizasyonu:
- `/wwwroot/css/styles.css` - Ana stil dosyası
- Vintage/antique tema renk paleti
- CSS custom properties kullanımı

### JavaScript:
- `/wwwroot/js/script.js` - Ana JavaScript dosyası
- Vanilla JavaScript (Framework kullanılmamış)
- Modüler function yapısı

### Güvenlik:
- Cookie Authentication
- CSRF koruması
- SQL Injection koruması (EF Core ile)
- XSS koruması (Razor ile otomatik)

## 📞 İletişim Bilgileri

**Valeria Antique Hotel Sultanahmet 1892**
- **Adres:** Mutrip Sokak No 15, Sultanahmet, Fatih, İstanbul 34000
- **Telefon:** +90 537 268 25 49
- **Rezervasyon:** +90 538 208 14 58
- **Email:** valeria.antique@gmail.com
- **Instagram:** @valeria_antique_1892

## 📝 Lisans

Bu proje Valeria Antique Hotel için özel olarak geliştirilmiştir.

---
**Geliştirme Tarihi:** 2025  
**Framework:** ASP.NET Core MVC 8.0  
**Mimari:** 4 Katmanlı Mimari
