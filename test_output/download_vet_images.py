import json
import os
import time
import urllib.parse
import urllib.request

OUT_DIR = r"E:\Projeler\filistin_eticaret_projesi\FilistinProje.Web\wwwroot\img\products"
META_PATH = r"E:\Projeler\filistin_eticaret_projesi\test_output\vet_image_sources.json"

SEARCHES = [
    ("vet-real-pet-vitamin.jpg", ["veterinary medicine bottle", "medicine bottle animal", "vitamin bottle"]),
    ("vet-real-pet-shampoo.jpg", ["dog shampoo", "pet shampoo", "shampoo bottle"]),
    ("vet-real-probiotic.jpg", ["probiotic powder", "dietary supplement powder", "powder supplement"]),
    ("vet-real-wound-spray.jpg", ["spray bottle medicine", "disinfectant spray bottle", "medical spray bottle"]),
    ("vet-real-ear-cleaner.jpg", ["ear cleaner bottle", "dropper bottle", "ear drops bottle"]),
    ("vet-real-mineral-block.jpg", ["mineral block cattle", "salt lick cattle", "cattle mineral lick"]),
    ("vet-real-poultry-premix.jpg", ["poultry feed", "chicken feed", "feed premix"]),
    ("vet-real-calf-start.jpg", ["calf feeding", "calf milk replacer", "calf bottle feeding"]),
]

API = "https://commons.wikimedia.org/w/api.php"
HEADERS = {"User-Agent": "FilistinProjeDemo/1.0 (local demo image research)"}


def api(params):
    q = urllib.parse.urlencode(params)
    req = urllib.request.Request(API + "?" + q, headers=HEADERS)
    with urllib.request.urlopen(req, timeout=30) as r:
        return json.loads(r.read().decode("utf-8"))


def search(term):
    data = api({
        "action": "query",
        "format": "json",
        "generator": "search",
        "gsrsearch": f"file:{term}",
        "gsrnamespace": "6",
        "gsrlimit": "12",
        "prop": "imageinfo",
        "iiprop": "url|mime|extmetadata",
        "iiurlwidth": "900",
    })
    pages = data.get("query", {}).get("pages", {})
    results = []
    for page in pages.values():
        infos = page.get("imageinfo") or []
        if not infos:
            continue
        info = infos[0]
        mime = info.get("mime", "")
        if mime not in ("image/jpeg", "image/png"):
            continue
        url = info.get("thumburl") or info.get("url")
        if not url:
            continue
        meta = info.get("extmetadata", {})
        results.append({
            "title": page.get("title"),
            "url": url,
            "source_url": info.get("descriptionurl"),
            "license": meta.get("LicenseShortName", {}).get("value", ""),
            "artist": meta.get("Artist", {}).get("value", ""),
            "credit": meta.get("Credit", {}).get("value", ""),
            "mime": mime,
        })
    return results


def download(url, dest):
    req = urllib.request.Request(url, headers=HEADERS)
    with urllib.request.urlopen(req, timeout=60) as r:
        data = r.read()
    with open(dest, "wb") as f:
        f.write(data)


def main():
    os.makedirs(OUT_DIR, exist_ok=True)
    picked = []
    used_titles = set()
    for filename, terms in SEARCHES:
        chosen = None
        for term in terms:
            try:
                results = search(term)
            except Exception as exc:
                print(f"search failed {term}: {exc}")
                continue
            for item in results:
                if item["title"] in used_titles:
                    continue
                chosen = item
                chosen["search_term"] = term
                break
            if chosen:
                break
            time.sleep(0.5)
        if not chosen:
            print(f"NO IMAGE for {filename}")
            continue
        ext = ".png" if chosen["mime"] == "image/png" else ".jpg"
        if not filename.lower().endswith(ext):
            filename = os.path.splitext(filename)[0] + ext
        dest = os.path.join(OUT_DIR, filename)
        download(chosen["url"], dest)
        chosen["local_path"] = "/img/products/" + filename
        picked.append(chosen)
        used_titles.add(chosen["title"])
        print(f"DOWNLOADED {filename}: {chosen['title']} [{chosen['license']}]")
    with open(META_PATH, "w", encoding="utf-8") as f:
        json.dump(picked, f, ensure_ascii=False, indent=2)

if __name__ == "__main__":
    main()
