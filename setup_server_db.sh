#!/usr/bin/env bash
set -e

echo "=========================================================="
echo " 7ANRPS48 PostgreSQL Sunucu Kurulum & Senkronizasyon Scripti"
echo "=========================================================="

echo "[1/6] PostgreSQL 18 dış bağlantı (listen_addresses = '*') ayarlanıyor..."
sudo sed -i "s/#listen_addresses = 'localhost'/listen_addresses = '*'/g" /etc/postgresql/18/main/postgresql.conf
sudo sed -i "s/listen_addresses = 'localhost'/listen_addresses = '*'/g" /etc/postgresql/18/main/postgresql.conf

echo "[2/6] pg_hba.conf erişim kuralları ekleniyor..."
if ! grep -q "100.64.0.0/10" /etc/postgresql/18/main/pg_hba.conf; then
  sudo tee -a /etc/postgresql/18/main/pg_hba.conf << 'EOF'

# 7ANRPS48 - Tailscale ve Docker Agi Erisim Izinleri
host    all             all             100.64.0.0/10           scram-sha-256
host    all             all             100.64.0.0/10           md5
host    all             all             172.16.0.0/12           scram-sha-256
host    all             all             172.16.0.0/12           md5
EOF
fi

echo "[3/6] UFW Güvenlik Duvarı izinleri tanımlanıyor (Port 5434)..."
sudo ufw allow from 100.64.0.0/10 to any port 5434 proto tcp comment "PostgreSQL Tailscale"
sudo ufw allow from 172.16.0.0/12 to any port 5434 proto tcp comment "PostgreSQL Docker"
sudo ufw reload

echo "[4/6] PostgreSQL servisi yeniden başlatılıyor..."
sudo systemctl restart postgresql

echo "[5/6] kanvasuser kullanıcısı ve filistindb veritabanı oluşturuluyor..."
sudo -u postgres psql -p 5434 -tc "SELECT 1 FROM pg_roles WHERE rolname='kanvasuser'" | grep -q 1 || \
sudo -u postgres psql -p 5434 -c "CREATE USER kanvasuser WITH PASSWORD 'changeme_in_production' SUPERUSER CREATEDB;"

sudo -u postgres psql -p 5434 -tc "SELECT 1 FROM pg_database WHERE datname='filistindb'" | grep -q 1 || \
sudo -u postgres psql -p 5434 -c "CREATE DATABASE filistindb OWNER kanvasuser;"

echo "[6/6] Dump dosyası içe aktarılıyor (Restore)..."
if [ -f "/home/abdulmuin/filistindb_from_local.dump" ]; then
    echo "Dump bulundu: /home/abdulmuin/filistindb_from_local.dump aktarılıyor..."
    sudo -u postgres pg_restore -p 5434 -d filistindb --no-owner --role=kanvasuser -v /home/abdulmuin/filistindb_from_local.dump || true
    sudo -u postgres psql -p 5434 -d filistindb -c "ALTER SCHEMA public OWNER TO kanvasuser; GRANT ALL ON SCHEMA public TO kanvasuser;" || true
    echo "Dump başarıyla içe aktarıldı!"
else
    echo "BİLGİ: /home/abdulmuin/filistindb_from_local.dump dosyası henüz bulunamadı."
    echo "Dosyayı yerel bilgisayardan scp ile aktardıktan sonra bu scripti tekrar çalıştırabilirsiniz."
fi

echo "=========================================================="
echo " Kurulum tamamlandı! PostgreSQL 5434 portunda hizmete hazır."
echo "=========================================================="
