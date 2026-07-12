# 7ANRPS48.com — Üretime Geçmeden Önce Admin Panelden Tamamlanması Gerekenler

**Son güncelleme:** 2026-07-09

Sistem otomatik seed ile aşağıdaki yapılandırmaları **sıfır ücret/placeholder değerlerle** ekledi. Üretime geçmeden önce admin panelden gerçek bilgileri girmelisiniz.

---

## 1. Kargo Fiyatları (`/Admin/Kargo`)

### Bölgeler (4 bölge seed edildi):

| Bölge | Mevcut Fiyat | Yapılacak |
|---|---|---|
| 48 İç Bölge (Kuzey) — Hayfa, Nasıra, Akka, Ümmü'l-Fahm | ₪0 | Gerçek fiyat gir |
| 48 İç Bölge (Merkez) — Yafa, Lydda, Ramla, Taybe | ₪0 | Gerçek fiyat gir |
| Batı Şeria (Kuzey/Merkez) — Cenin, Nablus, Ramallah, El-Halil, Beytüllahim, Salfit, Tubas, Tulkarim, Kalkilya, Eriha | ₪0 | Gerçek fiyat gir |
| Kudüs — El-Kudüs | ₪0 | Gerçek fiyat gir |

**Not:** Fiyatı ₪0 olan bölgede checkout **engellenir** (kullanıcıya "şehriniz için kargo yapılandırılmadı" mesajı gösterilir). Üretimde her bölgeye **gerçek teslimat ücreti** girin.

### Eksik bölge: **Gazze**

Şu anda Gazze şehirleri (Gazze, Han Yunus, Deyr el-Balah, Refah, Kuzey Gazze) seed edilmemiştir. Proje sahibi Gazze dahil edilecekse:
- Yeni bölge oluşturup şehirleri ekleyin.
- Gerçek kargo fiyatlarını girin.

### United Express (varsayılan kargo firması)

| Alan | Mevcut Değer | Yapılacak |
|---|---|---|
| Telefon | `+970 000 000 000` | Gerçek telefon |
| Takip URL | `https://tracking.unitedexpress.ps/?track=` | Gerçek takip adresi |
| Fiyat | ₪0 | Gerçek firma fiyatı (opsiyonel — bölge fiyatları önceliklidir) |
| Gönderici Adres | `Ramallah, Filistin` | Gerçek depo/ofis adresi |
| Gönderici Telefon | `+970 000 000 000` | Gerçek gönderici telefon |

### Kargo firması yönetimi

Müşteri checkout'ta **kargo firması seçmez**. Sadece şehir seçer → bölge fiyatı uygulanır. Admin sipariş sonrası `Admin/Siparis/Detay/{id}` sayfasında uygun firmayı ve takip numarasını girebilir.

---

## 2. Banka Hesapları (`/Admin/Bankalar`)

Seed edilmedi (veriler bilinmiyor). Admin panelden ekleyin:

- Banka adı
- Hesap sahibi (7ANRPS48 veya şahıs)
- IBAN (Filistin formatında, örn. PSXX XXXX XXXX XXXX XXXX XXXX)
- Şube kodu / Hesap no (varsa)
- **AktifMi** → ✅ işaretleyin

**Önemli:** Hiç aktif banka hesabı yoksa checkout'ta "Banka Havalesi" seçiliyken sipariş **engellenir** ve kullanıcıya yapılandırma eksik mesajı gösterilir.

---

## 3. Diğer Site Ayarları (`/Admin/Ayarlar`)

| Ayar | Kontrol |
|---|---|
| Ücretsiz kargo limiti (₪) | Mevcut 200₪ — doğru mu? |
| Kapıda ödeme aktif mi | İstediğiniz bölgeler için ayarlayın |
| Kapıda ödeme hizmet bedeli (₪) | Gerçek bedeli girin |
| Kapıda ödeme limiti (₪) | 2000₪ üstü kapıda ödeme yasak — doğru mu? |
| Toptancı min sipariş tutarı | Gerekirse ayarlayın |

---

## 4. Çalışma Kontrol Listesi (üretim öncesi)

- [ ] Her bölge için **₪0'dan büyük** gerçek kargo fiyatı girildi
- [ ] En az 1 **aktif** banka hesabı (IBAN) eklendi, AktifMi ✅, IBAN doğru
- [ ] United Express takip URL'si gerçek adresle güncellendi
- [ ] Gazze dahil edilecekse ek bölge + şehirler + fiyatlar tamamlandı
- [ ] Kapıda ödeme ayarları kontrol edildi (aktiflik, bedel, limit)
- [ ] Test: Bir şehir seçip checkout → doğru kargo ücreti geliyor mu?
- [ ] Test: Banka havalesi seçili → IBAN görünüyor mu?
- [ ] Test: ₪0 fiyatlı bölgeden checkout denenirse → engelleme mesajı gösteriliyor mu?