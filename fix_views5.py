import os
import re

def process_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    # Search for common Turkish terms in attributes and plain text
    # that we might have missed
    
    replacements = {
        r'Silmek istediğinize emin misiniz\?': 'Are you sure you want to delete?',
        r'Değişiklikleri kaydetmek istiyor musunuz\?': 'Do you want to save changes?',
        r'İşlem başarılı': 'Operation successful',
        r'Bir hata oluştu': 'An error occurred',
        r'Kayıt bulunamadı': 'No records found'
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
