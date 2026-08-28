with open('FilistinProje.Web/Areas/Admin/Views/Shared/_AdminLayout.cshtml', 'r', encoding='utf-8') as f:
    content = f.read()

import re
content = re.sub(r'Microsoft\.Extensions\.Localization\.StringLocalizer`1\[FilistinProje\.Web\.Resources\.SharedResource\]\?\["Admin_SosyalMedya_Baslik"\?\]', r'@Localizer["Admin_SosyalMedya_Baslik"].Value', content)

with open('FilistinProje.Web/Areas/Admin/Views/Shared/_AdminLayout.cshtml', 'w', encoding='utf-8') as f:
    f.write(content)
