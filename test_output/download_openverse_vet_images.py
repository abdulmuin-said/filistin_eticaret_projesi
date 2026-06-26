import json
import os
import time
import urllib.parse
import urllib.request

OUT_DIR = r"E:\Projeler\filistin_eticaret_projesi\FilistinProje.Web\wwwroot\img\products"
META_PATH = r"E:\Projeler\filistin_eticaret_projesi\test_output\openverse_vet_image_sources.json"

ITEMS = [
    ("vet-real-pet-vitamin.jpg", "pet vitamin bottle"),
    ("vet-real-pet-shampoo.jpg", "dog shampoo bottle"),
    ("vet-real-probiotic.jpg", "supplement powder bottle"),
    ("vet-real-wound-spray.jpg", "medical spray bottle"),
    ("vet-real-ear-cleaner.jpg", "ear drops bottle"),
    ("vet-real-mineral-block.jpg", "cattle salt lick mineral block"),
    ("vet-real-poultry-premix.jpg", "chicken feed poultry"),
    ("vet-real-calf-start.jpg", "calf feeding farm"),
]

HEADERS = {"User-Agent": "FilistinProjeDemo/1.0"}


def get_json(url):
    req = urllib.request.Request(url, headers=HEADERS)
    with urllib.request.urlopen(req, timeout=40) as r:
        return json.loads(r.read().decode("utf-8"))


def download(url, dest):
    req = urllib.request.Request(url, headers=HEADERS)
    with urllib.request.urlopen(req, timeout=60) as r:
        data = r.read()
        ctype = r.headers.get("Content-Type", "")
    if not data or not ctype.startswith("image/"):
        raise RuntimeError(f"not image content-type={ctype}")
    with open(dest, "wb") as f:
        f.write(data)


def pick_image(query):
    params = urllib.parse.urlencode({
        "q": query,
        "page_size": 20,
        "license_type": "commercial,modification",
        "extension": "jpg,png",
    })
    url = "https://api.openverse.engineering/v1/images/?" + params
    data = get_json(url)
    for item in data.get("results", []):
        if not item.get("url"):
            continue
        if item.get("mature"):
            continue
        return item
    return None


def main():
    os.makedirs(OUT_DIR, exist_ok=True)
    picked = []
    for filename, query in ITEMS:
        item = pick_image(query)
        if not item:
            print("NO_RESULT", filename, query)
            continue
        # prefer thumbnail if it is a real image url, otherwise full URL
        urls = [item.get("thumbnail"), item.get("url")]
        ok = False
        for u in urls:
            if not u:
                continue
            try:
                dest = os.path.join(OUT_DIR, filename)
                download(u, dest)
                ok = True
                break
            except Exception as exc:
                print("download failed", filename, exc)
                time.sleep(1)
        if not ok:
            continue
        picked.append({
            "filename": filename,
            "local_path": "/img/products/" + filename,
            "query": query,
            "title": item.get("title"),
            "creator": item.get("creator"),
            "license": item.get("license"),
            "license_version": item.get("license_version"),
            "foreign_landing_url": item.get("foreign_landing_url"),
            "source": item.get("source"),
        })
        print("DOWNLOADED", filename, item.get("title"), item.get("license"), item.get("source"))
        time.sleep(1)
    with open(META_PATH, "w", encoding="utf-8") as f:
        json.dump(picked, f, ensure_ascii=False, indent=2)

if __name__ == "__main__":
    main()
