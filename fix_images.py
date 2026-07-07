import re

with open("FilistinProje.Web/Views/Home/Index.cshtml", "r", encoding="utf-8") as f:
    content = f.read()

# Replace the empty image div with a branded logo placeholder
empty_img_pattern = r'<div class="flex h-full w-full items-center justify-center text-gray-300">\s*<i class="fas fa-image fa-2x"></i>\s*</div>'
branded_placeholder = """<div class="flex h-full w-full items-center justify-center bg-[#fcf9f3]">
                                                    <img src="/74anrps48logo2.svg" alt="Placeholder" class="w-1/2 opacity-20 grayscale" />
                                                </div>"""

new_content = re.sub(empty_img_pattern, branded_placeholder, content)

with open("FilistinProje.Web/Views/Home/Index.cshtml", "w", encoding="utf-8") as f:
    f.write(new_content)

print("Fixed placeholders.")
