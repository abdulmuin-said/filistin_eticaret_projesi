import os
import re

def process_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    # Generic string replacements for missed labels
    replacements = {
        r'<label class="ca-label">Arapça kategori adı</label>': '<label class="ca-label">@Localizer["CategoryNameAR"]</label>',
        r'<label class="ca-label">İngilizce kategori adı</label>': '<label class="ca-label">@Localizer["CategoryNameEN"]</label>'
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
