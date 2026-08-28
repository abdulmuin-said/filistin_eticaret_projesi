with open('FilistinProje.Web/Areas/Admin/Views/Shared/_AdminLayout.cshtml', 'r', encoding='utf-8') as f:
    content = f.read()

content = content.replace('@Localizer["Admin_SosyalMedya_Baslik"]', '@Localizer["Admin_SosyalMedya_Baslik"].Value')

with open('FilistinProje.Web/Areas/Admin/Views/Shared/_AdminLayout.cshtml', 'w', encoding='utf-8') as f:
    f.write(content)
