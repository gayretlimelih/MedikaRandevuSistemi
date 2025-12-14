# 🏥 Medika Randevu Sistemi

**Medika Randevu Sistemi**, hastaların doktorlardan çevrim içi randevu almasını sağlayan modern bir web uygulamasıdır.  
ASP.NET MVC mimarisi ile geliştirilmiştir ve SQL Server veritabanı kullanmaktadır.

Bu proje ders kapsamında geliştirilmiş olup kullanıcı yönetimi, doktor listesi görüntüleme ve randevu oluşturma gibi temel özellikleri içerir.

---

## 🚀 Kullanılan Teknolojiler

- **ASP.NET MVC**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Identity**
- **HTML, CSS, JavaScript**
- **Bootstrap**
- **C#**

---

## 🔧 Özellikler

- 👤 **Üyelik Sistemi (Kayıt & Giriş)**
- 🧑‍⚕️ **Doktor Listesi**
- 📅 **Randevu Sistemi**
- 🔐 **Kimlik Doğrulama (Identity)**
- 🗂️ **Migration ile Veritabanı Oluşturma**
- 🎨 **Modern UI (HTML/CSS/Bootstrap)**

---

## 🗄️ Veritabanı Yapısı

Proje EF Core Migration yapısı ile çalışır.  
Başlıca tablolar:

- **Hastalar**
- **Doktorlar**
- **Randevular**
- **AspNetUsers (Identity)**

Veritabanı, `ApplicationDbContext` üzerinden yönetilir.

---

## 🛠️ Kurulum Talimatları

Projeyi bilgisayarınızda çalıştırmak için:

1. Bu repoyu klonlayın:
   ```bash
   git clone https://github.com/gayretlimelih/MedikaRandevuSistemi.git
