import os
import re

def process_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    # Generic string replacements across Admin views
    replacements = {
        r'>Çıkış Yap<': '>@Localizer["Logout"]<',
        r'placeholder="Kategori Adı \(TR\)"': 'placeholder="@Localizer["CategoryNameTR"]"',
        r'placeholder="Kategori Adı \(EN\)"': 'placeholder="@Localizer["CategoryNameEN"]"',
        r'placeholder="Kategori Adı \(AR\)"': 'placeholder="@Localizer["CategoryNameAR"]"',
        r'>Durum<': '>@Localizer["Status"]<',
        r'>Aktif<': '>@Localizer["Active"]<',
        r'>İptal<': '>@Localizer["Cancel"]<',
        r'>Kaydet<': '>@Localizer["Save"]<',
        r'>Görsel Seç<': '>@Localizer["SelectImage"]<',
        r'>Yeni Kategori<': '>@Localizer["NewCategory"]<',
        r'>Yeni Ürün<': '>@Localizer["NewProduct"]<',
        r'>Düzenle<': '>@Localizer["Edit"]<',
        r'>Sil<': '>@Localizer["Delete"]<',
        r'>Kategoriler<': '>@Localizer["Categories"]<',
        r'>Ürünler<': '>@Localizer["Products"]<',
        r'>Siparişler<': '>@Localizer["Orders"]<'
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
