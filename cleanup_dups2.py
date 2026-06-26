import re
from pathlib import Path

base = Path(r"E:\Projeler\filistin_eticaret_projesi\KanvasProje.Web\Resources")
dup_keys = {"Admin_ShortDescription", "Admin_ShortDescriptionPlaceholder", "Admin_RecordCount", "Admin_Type"}

for fname in ["SharedResource.tr.resx", "SharedResource.en.resx", "SharedResource.ar.resx"]:
    p = base / fname
    content = p.read_text(encoding="utf-8")
    for key in dup_keys:
        pattern = re.compile(r"\s*<data name=\"" + re.escape(key) + r"\" xml:space=\"preserve\">\s*<value>[^<]*</value>\s*</data>", re.DOTALL)
        new_content, n = pattern.subn("", content)
        if n > 0:
            print(f"{fname}: removed {n} duplicate(s) of {key}")
            content = new_content
    p.write_text(content, encoding="utf-8")
