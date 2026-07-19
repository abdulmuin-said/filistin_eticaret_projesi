# 7ANRPS48 — Sunucu Deployment Rehberi

## Gereksinimler
- Docker + Docker Compose yüklü
- Git yüklü
- Sunucuda en az 2 GB RAM, 10 GB disk

---

## 1. İlk Kurulum (Sunucuya İlk Kez Deploy)

### 1.1 Projeyi sunucuya al
```bash
git clone <repo-url> /opt/7anrps48
cd /opt/7anrps48
```

### 1.2 Ortam değişkenlerini ayarla
```bash
cp .env.example .env
nano .env
```
`.env` içinde doldurulması zorunlu değerler:
```
POSTGRES_PASSWORD=guclu_bir_sifre_yaz
ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=filistindb;Username=kanvasuser;Password=guclu_bir_sifre_yaz
SMTP_HOST=smtp.example.com
SMTP_PORT=587
SMTP_USER=info@7anrps48.com
SMTP_PASS=smtp_sifresi
DATA_PROTECTION_KEYS_PATH=/app/secure-storage/dataprotection-keys
```

### 1.3 Uygulamayı başlat
```bash
docker compose build --no-cache
docker compose up -d
```

### 1.4 Sağlık kontrolü (migration bitene kadar bekle)
```bash
# Logları izle
docker logs -f filistinproje-web --tail 100

# Hazır olduğunda 200 döner
curl -s http://localhost:8080/health/ready
```

---

## 2. Güncelleme (Yeni Versiyon Deploy)

### 2.1 Yedek al (ÖNEMLİ — her güncellemeden önce)
```bash
cd /opt/7anrps48
docker exec filistinproje-db pg_dump -U kanvasuser filistindb \
  --no-owner --no-acl -F p \
  > backup_$(date +%Y%m%d_%H%M%S).sql
```

### 2.2 Yeni kodu çek
```bash
git pull origin main
```

### 2.3 Image'ı rebuild et ve deploy et
```bash
docker compose build --no-cache web
docker compose up -d web
```

### 2.4 Logları izle
```bash
docker logs -f filistinproje-web --tail 200
```

### 2.5 Sağlık doğrula
```bash
# Liveness (her zaman 200 olmalı)
curl -s http://localhost:8080/health/live

# Readiness (migration bittikten sonra 200 olmalı)
curl -s http://localhost:8080/health/ready
```

---

## 3. Veritabanı Geri Yükleme (Rollback)

### 3.1 Uygulamayı durdur
```bash
docker compose stop web
```

### 3.2 DB'yi sil ve geri yükle
```bash
# DİKKAT: TÜM VERİ SİLİNİR — sadece yedeğin olduğunda çalıştır
# DROP ve CREATE ayrı komutlar olmalı (transaction block hatası alırsın)
docker exec filistinproje-db psql -U kanvasuser -d postgres -c "DROP DATABASE IF EXISTS filistindb;"
docker exec filistinproje-db psql -U kanvasuser -d postgres -c "CREATE DATABASE filistindb OWNER kanvasuser;"

cat backup_YYYYMMDD_HHMMSS.sql | \
  docker exec -i filistinproje-db psql -U kanvasuser -d filistindb
```

### 3.3 Uygulamayı tekrar başlat
```bash
docker compose up -d web
docker logs -f filistinproje-web --tail 100
```

---

## 4. Faydalı Komutlar

```bash
# Tüm servislerin durumu
docker compose ps

# Web container logları
docker logs filistinproje-web --tail 50

# DB'ye bağlan
docker exec -it filistinproje-db psql -U kanvasuser -d filistindb

# Container'ı yeniden başlat
docker compose restart web

# Tüm sistemi durdur
docker compose down

# Tüm sistemi durdur + volume sil (VERİ KAYBI!)
docker compose down -v
```

---

## 5. Nginx Reverse Proxy (Opsiyonel)

Eğer 80/443 portunda yayın yapacaksanız:

```nginx
server {
    listen 80;
    server_name 7anrps48.com www.7anrps48.com;
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl;
    server_name 7anrps48.com www.7anrps48.com;

    ssl_certificate /etc/letsencrypt/live/7anrps48.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/7anrps48.com/privkey.pem;

    client_max_body_size 55M;

    location / {
        proxy_pass http://127.0.0.1:8080;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

SSL sertifikası için:
```bash
apt install certbot python3-certbot-nginx
certbot --nginx -d 7anrps48.com -d www.7anrps48.com
```

---

## 6. Bu Dump'ı Sunucuya Yükle

Lokal makineden sunucuya dump gönder:
```bash
scp backup_20260720_020234.sql user@sunucu-ip:/opt/7anrps48/
```

Sunucuda geri yükle:
```bash
cat backup_20260720_020234.sql | \
  docker exec -i filistinproje-db psql -U kanvasuser -d filistindb
```

---

## Sağlık URL'leri
| URL | Açıklama |
|-----|----------|
| `http://sunucu:8080/health/live` | Process canlı mı? (her zaman 200) |
| `http://sunucu:8080/health/ready` | Migration tamam, servis hazır mı? |
| `http://sunucu:8080/admin/hangfire` | Arka plan görevleri (sadece local) |
