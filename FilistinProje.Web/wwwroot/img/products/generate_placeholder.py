"""Generate a brand-aligned product placeholder image.

Brand palette (from storefront.css / _Layout):
    cream background : #f8f6f2
    soft border line : #d9d2c2 / border-brand
    gold accent      : #b58735 (brand-gold)
    muted text       : #8a7e63 (brand-mutedtext / brand-border)

The placeholder is a centered, abstract "package" outline (not a real
photo) so it can never be confused with an actual catalog picture.

Outputs:
    wwwroot/img/products/placeholder.webp
    wwwroot/img/products/placeholder.png   (fallback if .webp fails)
"""

from __future__ import annotations

from PIL import Image, ImageDraw, ImageFont
from pathlib import Path

OUT_DIR = Path(__file__).resolve().parent
OUT_DIR.mkdir(parents=True, exist_ok=True)

W, H = 800, 800
CREAM = (248, 246, 242)
LIGHT_BORDER = (217, 210, 194)
GOLD = (181, 135, 53)
MUTED = (138, 126, 99)


def _font(size: int) -> ImageFont.ImageFont:
    candidates = [
        "C:/Windows/Fonts/arial.ttf",
        "C:/Windows/Fonts/segoeui.ttf",
        "C:/Windows/Fonts/tahoma.ttf",
    ]
    for c in candidates:
        if Path(c).exists():
            return ImageFont.truetype(c, size=size)
    return ImageFont.load_default()


def render() -> Image.Image:
    img = Image.new("RGB", (W, H), CREAM)
    draw = ImageDraw.Draw(img)

    # Outer box outline (border-brand)
    box_left, box_top, box_right, box_bottom = 200, 200, 600, 580
    draw.rounded_rectangle(
        (box_left, box_top, box_right, box_bottom),
        radius=24,
        outline=LIGHT_BORDER,
        width=4,
    )

    # Inner abstract "package" placeholder box (gold)
    draw.rounded_rectangle(
        (300, 290, 500, 480),
        radius=18,
        outline=GOLD,
        width=5,
    )

    # Faint diagonal accent inside the inner box
    draw.line((325, 305, 475, 465), fill=(217, 210, 194), width=3)
    draw.line((475, 305, 325, 465), fill=(217, 210, 194), width=3)

    # Tagline (English default fallback). Brand-ali
    brand = "7ANRPS48"
    caption = "Product image coming soon"
    brand_font = _font(38)
    caption_font = _font(24)

    bx = draw.textbbox((0, 0), brand, font=brand_font)
    bw = bx[2] - bx[0]
    draw.text(((W - bw) / 2, 612), brand, fill=GOLD, font=brand_font)

    cx = draw.textbbox((0, 0), caption, font=caption_font)
    cw = cx[2] - cx[0]
    draw.text(((W - cw) / 2, 660), caption, fill=MUTED, font=caption_font)

    return img


def main() -> None:
    base = render()
    webp_path = OUT_DIR / "placeholder.webp"
    png_path = OUT_DIR / "placeholder.png"

    # WebP, quality 75, lossless=False -> small file
    base.save(webp_path, format="WEBP", quality=75, method=6, optimize=True)
    base.save(png_path, format="PNG", optimize=True)

    w = webp_path.stat().st_size
    p = png_path.stat().st_size
    print(f"placeholder.webp: {w} bytes")
    print(f"placeholder.png:  {p} bytes")


if __name__ == "__main__":
    main()
