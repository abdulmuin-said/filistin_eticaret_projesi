import os
import re

# Look into product image upload functionality
# Controller or Service where this is handled

def process_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    # Admin'den ürün fotoğrafı eklerken arkaplanı olmasın şeklinde foto yüklemeye 
    # izin veriyor ama ben öyle olsun istemiyorum
    # We might have a service like ImageService or in UrunController that uses ImageSharp to remove background
    
    modified = False
    
    # Check if there's any background removal code using ImageSharp or similar
    if "RemoveBackground" in content or "Arkaplan" in content or "Transparent" in content or "White" in content:
        # We need to manually inspect this
        print(f"Potential background manipulation found in {filepath}")
        
    # More generally, let's look at ImageSharp save logic
    if "Image.Load" in content or "Mutate" in content:
        print(f"Image manipulation found in {filepath}")

for root, _, files in os.walk('FilistinProje.Web'):
    for file in files:
        if file.endswith('.cs'):
            process_file(os.path.join(root, file))
            
for root, _, files in os.walk('FilistinProje.Service'):
    for file in files:
        if file.endswith('.cs'):
            process_file(os.path.join(root, file))
