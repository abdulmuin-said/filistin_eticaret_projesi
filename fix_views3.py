import os
import re

def process_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    # Search for common Turkish terms in attributes and plain text
    # that we might have missed
    
    replacements = {
        r'placeholder="Kategori Adı \(TR\)"': 'placeholder="@Localizer["CategoryNameTR"]"',
        r'placeholder="Kategori Adı \(EN\)"': 'placeholder="@Localizer["CategoryNameEN"]"',
        r'placeholder="Kategori Adı \(AR\)"': 'placeholder="@Localizer["CategoryNameAR"]"',
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

