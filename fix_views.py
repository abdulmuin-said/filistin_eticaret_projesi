import os
import re

def process_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    # AdminLayout adjustments that might have been missed
    if '_AdminLayout.cshtml' in filepath:
        # Just simple targeted fixes for AdminLayout
        content = re.sub(r'>Çıkış Yap<', '>@Localizer["Logout"]<', content)
        content = re.sub(r'alt="Logo" class="h-8 w-auto"', 'alt="Logo" class="h-8 w-auto"', content) # dummy to see if it works

    # Kategori views
    if 'Kategori\Ekle.cshtml' in filepath or 'Kategori\Duzenle.cshtml' in filepath:
        content = re.sub(r'>Kategori Bilgileri<', '>Category Information<', content)
        content = re.sub(r'placeholder="Kategori Adı \(TR\)"', 'placeholder="Category Name (TR)"', content)
        content = re.sub(r'placeholder="Kategori Adı \(EN\)"', 'placeholder="Category Name (EN)"', content)
        content = re.sub(r'placeholder="Kategori Adı \(AR\)"', 'placeholder="Category Name (AR)"', content)
        content = re.sub(r'>Durum<', '>Status<', content)
        content = re.sub(r'>Aktif<', '>Active<', content)
        content = re.sub(r'>İptal<', '>Cancel<', content)
        content = re.sub(r'>Kaydet<', '>Save<', content)
        content = re.sub(r'>Görsel Seç<', '>Select Image<', content)
        
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)
        
    print(f"Processed {filepath}")

for root, _, files in os.walk('FilistinProje.Web/Areas/Admin/Views'):
    for file in files:
        if file.endswith('.cshtml'):
            process_file(os.path.join(root, file))
