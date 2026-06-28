# FilistinProje - Güncelleme Adımları

Herhangi bir kod değişikliğinden sonra sunucuya yansıtmak için:

## 1. LOKALDE: Değişiklikleri yap ve GitHub'a gönder

```bash
# Proje klasöründe (lokal bilgisayar)
cd E:\Projeler\filistin_eticaret_projesi

# Değişiklikleri ekle
git add .
git commit -m "ne degisti"

# GitHub'a pushla
git push origin main
```

## 2. SUNUCUDA: Güncellemeyi çek ve uygula

SSH ile sunucuya bağlan:
```bash
ssh abdulmuin@canvasia-server
```

Sonra sırayla:
```bash
# Proje klasörüne git
cd ~/filistin_projesi/filistin_eticaret_projesi

# En son kodu çek
git pull origin main

# Docker imajını yeniden build et
docker compose build --no-cache

# Container'ları yeniden başlat
docker compose up -d
```

## Eğer sadece küçük bir değişiklikse (hızlı yol)

```bash
cd ~/filistin_projesi/filistin_eticaret_projesi
git pull origin main
docker compose build --no-cache
docker compose up -d
```

Bu kadar! Her zaman bu 3 adım yeterli:
1. Lokalde `git push`
2. Sunucuda `git pull`
3. Sunucuda `docker compose build --no-cache && docker compose up -d`
