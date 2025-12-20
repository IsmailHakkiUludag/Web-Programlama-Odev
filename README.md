# Spor Salonu Yönetim Sistemi (Web Programlama Projesi)

Bu proje, **ASP.NET Core 8.0 MVC** mimarisi kullanılarak geliştirilmiş kapsamlı bir **Spor Salonu Randevu ve Yönetim Sistemi**dir. Proje; şube (salon), antrenör ve hizmet yönetimini sağlarken, üyelerin online randevu almasına olanak tanır.

## 👨‍🎓 Öğrencilerin Bilgileri

* **Adı Soyadı:** [İsmail Hakkı Uludağ]
* **Öğrenci Numarası:** [G231210049]
* **Ders:** Web Programlama
* **Dönem:** 2025-2026 Güz

* **Adı Soyadı:** [Süleyman Gencay Coşkun]
* **Öğrenci Numarası:** [G231210073]
* **Ders:** Web Programlama
* **Dönem:** 2025-2026 Güz

---

## 🚀 Projenin Özellikleri

Proje **Admin** ve **Üye (Kullanıcı)** olmak üzere iki temel rol üzerine kurulmuştur.

### 🔐 1. Kimlik Doğrulama ve Yetkilendirme (Identity)
* Kullanıcı Kayıt (Register) ve Giriş (Login) işlemleri.
* Rol tabanlı yetkilendirme (Admin ve Uye rolleri).
* Yetkisiz erişimlerin engellenmesi (Authorize attribute).

### 🛠️ 2. Yönetici (Admin) Paneli
* **Salon Yönetimi:** Yeni spor salonu şubesi ekleme, düzenleme, silme ve resim yükleme.
* **Antrenör Yönetimi:** Antrenör ekleme, uzmanlık alanı ve çalışma saatlerini belirleme.
* **Hizmet Yönetimi:** Verilen hizmetleri (Fitness, Pilates vb.), sürelerini ve ücretlerini tanımlama.
* **Randevu Yönetimi:** Üyelerden gelen randevuları görüntüleme, **Onaylama** veya **Reddetme**.
* **Kullanıcı ve Rol Yönetimi:** Sistemdeki kullanıcıları listeleme, silme ve kullanıcılara yeni roller atama.
* **Raporlama:**
    * **Günlük Kazanç Raporu:** Tarih bazlı ciro takibi.
    * **Antrenör Kazanç Raporu:** Hangi antrenörün ne kadar kazandırdığının analizi.

### 📅 3. Üye (Kullanıcı) İşlemleri
* Hizmetleri ve Antrenörleri inceleme.
* Müsaitlik durumuna göre **Randevu Alma** (Tarih, Saat, Hizmet ve Antrenör seçimi).
* Kendi geçmiş ve gelecek randevularını görüntüleme.
* Bekleyen randevuları iptal etme.

### 🌐 4. WEB API (RESTful Service)
* Proje, dış kaynakların veri çekebilmesi için bir REST API içerir.
* **Endpoint:** `/api/HizmetApi`
* Hizmetleri ve Antrenörleri JSON formatında listeler.

---

## 💻 Kullanılan Teknolojiler

* **Platform:** .NET 8.0 (Core)
* **Framework:** ASP.NET Core MVC
* **Veritabanı:** MS SQL Server (LocalDB)
* **ORM:** Entity Framework Core (Code First)
* **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript (jQuery)
* **Kütüphaneler:**
    * `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
    * `Microsoft.EntityFrameworkCore.SqlServer`
    * `Microsoft.EntityFrameworkCore.Tools`

---

## ⚙️ Kurulum ve Çalıştırma

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

1.  **Projeyi İndirin:** Dosyaları bilgisayarınıza indirin veya klonlayın.
2.  **Visual Studio ile Açın:** `.sln` uzantılı dosyayı Visual Studio 2022 ile açın.
3.  **Veritabanını Oluşturun:**
    * Visual Studio'da `View > Other Windows > Package Manager Console` menüsünü açın.
    * Aşağıdaki komutu yazıp Enter'a basın (Migration dosyaları zaten mevcuttur):
        ```powershell
        Update-Database
        ```
    * *Bu işlem `appsettings.json` dosyasındaki LocalDB bağlantısını kullanarak veritabanını otomatik oluşturacaktır.*
4.  **Projeyi Başlatın:** `IIS Express` veya `http` profili ile projeyi çalıştırın (F5).

---

## 🔑 Varsayılan Yönetici (Admin) Bilgileri

Proje ilk kez çalıştırıldığında `IdentitySeedData.cs` dosyası otomatik olarak bir Admin kullanıcısı oluşturur.

* **E-Posta:** `g231210049@sakarya.edu.tr`
* **Şifre:** `sau`

*(Not: Admin paneline erişmek için bu bilgilerle giriş yapmanız gerekmektedir.)*

---

## 📂 Proje Dosya Yapısı

* **Controllers:** Sayfa yönlendirmeleri ve iş mantığı (Admin, Account, Randevu, Home vb.).
* **Models:** Veritabanı tabloları (Entity) ve Veritabanı Bağlamı (Context).
* **ViewModels:** Görünüm ile Controller arasında veri taşıyan modeller.
* **Views:** Kullanıcı arayüzü dosyaları (.cshtml).
* **wwwroot:** CSS, JS ve yüklenen resimlerin (img) bulunduğu klasör

---
