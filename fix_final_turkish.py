import os
import re

def process_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    replacements = {
        r'>Ana Sayfa Seksiyon Yönetimi<': '>Home Page Section Management<',
        r'>Seksiyon Düzenle<': '>Edit Section<',
        r'>Tüm Seksiyonlar<': '>All Sections<',
        r'>Yeni Seksiyon<': '>New Section<',
        r'>Başlık<': '>Title<',
        r'>Sıra<': '>Order<',
        r'>Durum<': '>Status<',
        r'>Tipi<': '>Type<',
        r'>Görünüm<': '>Layout<',
        r'>Aktif<': '>Active<',
        r'>Pasif<': '>Inactive<',
        r'>Şimdi Kaydet<': '>Save Now<',
        r'>Vazgeç<': '>Cancel<',
        r'>Bileşen Tipi Seçiniz<': '>Select Component Type<',
        r'>Ürün Seçimi \(En Fazla 10 Ürün\)<': '>Product Selection (Max 10 Products)<',
        r'>Kategori<': '>Category<',
        r'>Marka<': '>Brand<',
        r'>Görünüm Şekli<': '>Layout Type<',
        r'>Sayfa Yönetimi<': '>Page Management<',
        r'>Sayfa Düzenle<': '>Edit Page<',
        r'>Yeni Sayfa<': '>New Page<',
        r'>Tüm Sayfalar<': '>All Pages<',
        r'>Özet<': '>Summary<',
        r'>İçerik<': '>Content<',
        r'>SEO Başlık<': '>SEO Title<',
        r'>Arapça İçerik<': '>Arabic Content<',
        r'>İngilizce İçerik<': '>English Content<',
        r'>Türkçe İçerik<': '>Turkish Content<',
        r'>Sayfayı Görüntüle<': '>View Page<',
        r'placeholder="Virgülle ayırarak yazın"': 'placeholder="Separate with commas"',
        r'>Temel Bilgiler<': '>Basic Information<',
        r'>Ürün Seçimi<': '>Product Selection<',
        r'>Görünüm Ayarları<': '>Layout Settings<'
    }

    modified = False
    for pattern, replacement in replacements.items():
        if re.search(pattern, content):
            content = re.sub(pattern, replacement, content)
            modified = True
            
    if modified:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"Processed {filepath}")

for root, _, files in os.walk('FilistinProje.Web/Areas/Admin/Views'):
    for file in files:
        if file.endswith('.cshtml'):
            process_file(os.path.join(root, file))
