import os
import re

def process_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    # Generic string replacements for missed labels
    replacements = {
        r'>Admin Panel<': '>Admin Panel<', # Do nothing, just to ensure it's hit
    }
    
    # We replace common Turkish fragments remaining in cshtml files in Admin
    common_fragments = [
        (r'>Onay Bekliyor<', '>@Localizer["PendingApproval"]<'),
        (r'>Hazırlanıyor<', '>@Localizer["Preparing"]<'),
        (r'>Kargoya Verildi<', '>@Localizer["Shipped"]<'),
        (r'>Teslim Edildi<', '>@Localizer["Delivered"]<'),
        (r'>İptal Edildi<', '>@Localizer["Cancelled"]<'),
        (r'>İade Edildi<', '>@Localizer["Refunded"]<'),
        (r'>Tümü<', '>@Localizer["All"]<'),
        (r'>Ara<', '>@Localizer["Search"]<'),
        (r'>Temizle<', '>@Localizer["Clear"]<'),
        (r'>Yeni Ekle<', '>@Localizer["AddNew"]<'),
        (r'>İşlemler<', '>@Localizer["Actions"]<'),
        (r'>Evet<', '>@Localizer["Yes"]<'),
        (r'>Hayır<', '>@Localizer["No"]<'),
        (r'>Kapat<', '>@Localizer["Close"]<'),
    ]

    modified = False
    for pattern, replacement in common_fragments:
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
