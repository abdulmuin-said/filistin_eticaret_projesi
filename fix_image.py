import re

with open('FilistinProje.Service/Services/LocalMediaService.cs', 'r', encoding='utf-8') as f:
    content = f.read()

# Make it save as original format rather than forcing lossy WebP which removes transparency if not configured right
# We'll use the original extension and save with auto encoder
new_content = re.sub(
    r'var finalFileName = \$\"\{slug\}-\{suffix\}-\{token\}\.webp\";\s*var fullPath = Path\.Combine\(folder, finalFileName\);\s*using var stream = new MemoryStream\(imageBytes\);\s*using var image = await Image\.LoadAsync\(stream\);\s*image\.Mutate\(x =>\s*\{\s*x\.AutoOrient\(\);\s*x\.Resize\(new ResizeOptions\s*\{\s*Mode = ResizeMode\.Max,\s*Size = new Size\(maxWidth, maxHeight\)\s*\}\);\s*\}\);\s*await image\.SaveAsWebpAsync\(fullPath, new WebpEncoder\s*\{\s*Quality = 82,\s*FileFormat = WebpFileFormatType\.Lossy\s*\}\);',
    r'''var finalFileName = $"{slug}-{suffix}-{token}{extension.ToLowerInvariant()}";
                var fullPath = Path.Combine(folder, finalFileName);

                using var stream = new MemoryStream(imageBytes);
                using var image = await Image.LoadAsync(stream);
                
                image.Mutate(x =>
                {
                    x.AutoOrient();
                    x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(maxWidth, maxHeight)
                    });
                });

                await image.SaveAsync(fullPath);''',
    content
)

with open('FilistinProje.Service/Services/LocalMediaService.cs', 'w', encoding='utf-8') as f:
    f.write(new_content)
    print("Modified LocalMediaService.cs")
