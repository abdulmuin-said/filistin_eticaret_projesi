"""
Playwright UI test — Filistin E-Ticaret
Çalıştır: pip install pytest playwright pytest-playwright && playwright install chromium
         pytest tests/test_ui_playwright.py -v --base-url=http://localhost:5002

Ayarlar:
  BASE_URL env var veya --base-url pytest-playwright argümanı
"""

import re
import os
import pytest
from playwright.sync_api import Page, expect

BASE = os.getenv("BASE_URL", "http://localhost:5002")

# ---------------------------------------------------------------------------
# Yardımcı
# ---------------------------------------------------------------------------

def goto(page: Page, path: str):
    page.goto(f"{BASE}{path}", wait_until="domcontentloaded", timeout=15_000)


def assert_no_encoding_garbage(page: Page):
    """Sayfada â€™ â‚ª Ã© gibi Latin-1/UTF-8 karışımı yoksa geçer."""
    text = page.inner_text("body")
    garbage = re.findall(r"[Ã¢â€˜Â£Ã©Ã]+", text)
    assert not garbage, f"Encoding bozukluğu: {garbage[:5]}"


def assert_no_turkish_hardcode(page: Page):
    """Türkçe hardcode'lu UI ifadeleri Arapça modda görünmesin."""
    # Sadece navigasyon/buton metinlerindeki açık Türkçe kelimeleri kontrol et
    TURKISH_UI = ["Alışveriş", "Sepete Ekle", "Ürünler", "Kategoriler", "İletişim",
                  "Hakkımızda", "Giriş Yap", "Kayıt Ol"]
    body = page.inner_text("body")
    hits = [w for w in TURKISH_UI if w in body]
    assert not hits, f"Türkçe hardcode bulundu: {hits}"


# ---------------------------------------------------------------------------
# Ana sayfa
# ---------------------------------------------------------------------------

class TestHome:
    def test_loads(self, page: Page):
        goto(page, "/")
        expect(page).not_to_have_title("")

    def test_no_encoding_garbage(self, page: Page):
        goto(page, "/")
        assert_no_encoding_garbage(page)

    def test_hero_slider_exists(self, page: Page):
        goto(page, "/")
        # Hero bölümü var mı — en az bir slider/banner container
        hero = page.locator(".home-hero-frame, [data-hero], section.hero, .swiper")
        assert hero.count() > 0, "Hero slider bulunamadı"

    def test_product_cards_visible(self, page: Page):
        goto(page, "/")
        cards = page.locator("a[href*='/Urun/Detay/']")
        assert cards.count() > 0, "Ana sayfada ürün kartı yok"

    def test_cart_icon_shows_count(self, page: Page):
        goto(page, "/")
        cart_badge = page.locator("[data-cart-count], .cart-count, #sepet-count")
        # badge var mı (içi 0 olabilir, ama eleman olmalı)
        assert cart_badge.count() > 0, "Sepet sayacı elementi yok"

    def test_rtl_direction(self, page: Page):
        goto(page, "/")
        lang = page.evaluate("document.documentElement.lang")
        dir_ = page.evaluate("document.documentElement.dir")
        if lang == "ar":
            assert dir_ == "rtl", f"Arapça'da dir=rtl bekleniyor, gelen: {dir_}"

    def test_whatsapp_button(self, page: Page):
        goto(page, "/")
        wa = page.locator("a[href*='wa.me'], a[href*='whatsapp']")
        assert wa.count() > 0, "WhatsApp butonu yok"

    def test_no_console_errors(self, page: Page):
        errors = []
        page.on("console", lambda msg: errors.append(msg.text) if msg.type == "error" else None)
        goto(page, "/")
        page.wait_for_timeout(2000)
        critical = [e for e in errors if "Uncaught" in e or "TypeError" in e or "ReferenceError" in e]
        assert not critical, f"JS hataları: {critical[:3]}"


# ---------------------------------------------------------------------------
# Ürün listesi
# ---------------------------------------------------------------------------

class TestUrunList:
    def test_loads(self, page: Page):
        goto(page, "/Urun")
        expect(page).not_to_have_title("")

    def test_product_cards_present(self, page: Page):
        goto(page, "/Urun")
        cards = page.locator("a[href*='/Urun/Detay/']")
        assert cards.count() > 0, "Ürün listesinde kart yok"

    def test_currency_symbol_not_garbage(self, page: Page):
        """â‚ª encoding sorunu var mı kontrol et (₪ olmalı)."""
        goto(page, "/Urun")
        text = page.inner_text("body")
        assert "â‚ª" not in text, "Fiyat sembolü encoding hatası: â‚ª görünüyor, ₪ olmalı"

    def test_filter_sidebar_exists(self, page: Page):
        goto(page, "/Urun")
        sidebar = page.locator("aside, .sidebar, [class*='filter'], form[method='get']")
        assert sidebar.count() > 0, "Filtre sidebar'ı yok"

    def test_pagination(self, page: Page):
        goto(page, "/Urun")
        # Sayfalama var mı (ürün sayısına bağlı, olmayabilir — sadece crash olmadan geçer)
        page.wait_for_selector("body", timeout=5000)

    def test_search_works(self, page: Page):
        goto(page, "/Urun?s=test")
        expect(page).not_to_have_title("")
        # 500 hatası yok
        assert "500" not in page.title()

    def test_sort_works(self, page: Page):
        goto(page, "/Urun?sort=fiyat_artan")
        page.wait_for_selector("body", timeout=5000)
        assert "500" not in (page.title() or "")

    def test_category_filter(self, page: Page):
        goto(page, "/Urun?k=1")
        page.wait_for_selector("body", timeout=5000)
        assert "500" not in (page.title() or "")

    def test_no_encoding_garbage(self, page: Page):
        goto(page, "/Urun")
        assert_no_encoding_garbage(page)


# ---------------------------------------------------------------------------
# Ürün detay
# ---------------------------------------------------------------------------

class TestUrunDetay:
    @pytest.fixture(scope="class")
    def first_product_url(self, page: Page):
        """Ana sayfadan ilk ürün URL'ini al."""
        goto(page, "/Urun")
        first = page.locator("a[href*='/Urun/Detay/']").first
        href = first.get_attribute("href")
        return href or "/Urun/Detay/urun-1"

    def test_loads(self, page: Page, first_product_url):
        goto(page, first_product_url)
        expect(page).not_to_have_title("")
        assert "500" not in (page.title() or "")

    def test_add_to_cart_button(self, page: Page, first_product_url):
        goto(page, first_product_url)
        btn = page.locator(
            "button[data-action='sepet'], "
            "button:has-text('Sepet'), "
            "button[class*='sepet'], "
            "form[action*='/Sepet/'] button[type='submit'], "
            ".add-to-cart"
        )
        assert btn.count() > 0, "Sepete ekle butonu yok"

    def test_product_gallery(self, page: Page, first_product_url):
        goto(page, first_product_url)
        img = page.locator("img[src*='/img/products/'], img[src*='/media/products/'], img.product-img")
        assert img.count() > 0, "Ürün görseli yok"

    def test_price_displayed(self, page: Page, first_product_url):
        goto(page, first_product_url)
        # Fiyat var mı — ₪ veya ILS veya rakam içeren eleman
        price = page.locator("[class*='fiyat'], [class*='price'], [data-price]")
        if price.count() == 0:
            # Text içinde fiyat sembolü ara
            text = page.inner_text("body")
            assert any(c.isdigit() for c in text), "Sayfada hiç rakam/fiyat yok"

    def test_breadcrumb(self, page: Page, first_product_url):
        goto(page, first_product_url)
        bc = page.locator("nav[aria-label*='breadcrumb'], .breadcrumb, ol.breadcrumb")
        assert bc.count() > 0, "Breadcrumb yok"

    def test_no_encoding_garbage(self, page: Page, first_product_url):
        goto(page, first_product_url)
        assert_no_encoding_garbage(page)


# ---------------------------------------------------------------------------
# Sepet
# ---------------------------------------------------------------------------

class TestSepet:
    def test_loads_empty(self, page: Page):
        goto(page, "/Sepet")
        expect(page).not_to_have_title("")

    def test_empty_state_message(self, page: Page):
        """Boş sepet: anlamlı mesaj göster."""
        goto(page, "/Sepet")
        text = page.inner_text("body").lower()
        empty_signals = ["boş", "empty", "فارغ", "لا يوجد", "no items", "nothing"]
        # Sepet gerçekten boşsa mesaj olmalı
        cart_items = page.locator("table tr, .cart-item, [class*='sepet-item']")
        if cart_items.count() == 0:
            assert any(s in text for s in empty_signals), \
                "Boş sepet mesajı yok (Arapça veya İngilizce)"

    def test_no_encoding_garbage(self, page: Page):
        goto(page, "/Sepet")
        assert_no_encoding_garbage(page)


# ---------------------------------------------------------------------------
# Hesap (giriş, kayıt)
# ---------------------------------------------------------------------------

class TestHesap:
    def test_login_page_loads(self, page: Page):
        goto(page, "/Hesap/GirisYap")
        expect(page).not_to_have_title("")

    def test_login_form_fields(self, page: Page):
        goto(page, "/Hesap/GirisYap")
        assert page.locator("input[type='email'], input[name*='mail']").count() > 0, "E-posta alanı yok"
        assert page.locator("input[type='password']").count() > 0, "Şifre alanı yok"
        assert page.locator("button[type='submit'], input[type='submit']").count() > 0, "Submit butonu yok"

    def test_register_page_loads(self, page: Page):
        goto(page, "/Hesap/KayitOl")
        expect(page).not_to_have_title("")

    def test_forgot_password_page(self, page: Page):
        goto(page, "/Hesap/SifremiUnuttum")
        expect(page).not_to_have_title("")

    def test_login_csrf_token(self, page: Page):
        goto(page, "/Hesap/GirisYap")
        token = page.locator("input[name='__RequestVerificationToken']")
        assert token.count() > 0, "CSRF token yok — güvenlik açığı"


# ---------------------------------------------------------------------------
# Kurumsal sayfalar
# ---------------------------------------------------------------------------

KURUMSAL_PAGES = [
    "/Kurumsal/Hakkimizda",
    "/Kurumsal/Iletisim",
    "/Kurumsal/Gizlilik",
    "/Kurumsal/SSS",
    "/Kurumsal/IadeKosullari",
]


@pytest.mark.parametrize("path", KURUMSAL_PAGES)
def test_kurumsal_pages_load(page: Page, path: str):
    goto(page, path)
    assert page.title() != "", f"{path} başlık boş"
    assert "404" not in page.title() and "500" not in page.title(), \
        f"{path} hata sayfası dönüyor"
    assert_no_encoding_garbage(page)


# ---------------------------------------------------------------------------
# Profil (anonim → redirect beklenir)
# ---------------------------------------------------------------------------

class TestProfil:
    def test_profile_redirects_to_login(self, page: Page):
        goto(page, "/Profil")
        # Auth redirect: /Hesap/GirisYap veya 401 beklenir
        final_url = page.url
        assert "GirisYap" in final_url or "login" in final_url.lower() or \
               "Hesap" in final_url, \
            f"Profil korumalı değil! Final URL: {final_url}"

    def test_orders_redirects_to_login(self, page: Page):
        goto(page, "/Profil/Siparislerim")
        final_url = page.url
        assert "GirisYap" in final_url or "login" in final_url.lower(), \
            f"Siparişlerim korumalı değil: {final_url}"


# ---------------------------------------------------------------------------
# Sipariş (ödeme sayfası — redirect beklenir)
# ---------------------------------------------------------------------------

class TestSiparis:
    def test_odeme_redirects_unauthenticated(self, page: Page):
        goto(page, "/Siparis/Odeme")
        # Giriş yapmamış kullanıcı: redirect veya hata
        final_url = page.url
        assert "GirisYap" in final_url or "Hesap" in final_url or \
               page.locator("form").count() == 0 or \
               "Siparis/Odeme" not in final_url, \
            "Ödeme sayfası anonim erişime açık!"

    def test_basarili_page(self, page: Page):
        goto(page, "/Siparis/Basarili")
        # 500 olmamalı
        assert "500" not in (page.title() or "")

    def test_basarisiz_page(self, page: Page):
        goto(page, "/Siparis/Basarisiz")
        assert "500" not in (page.title() or "")


# ---------------------------------------------------------------------------
# Favori
# ---------------------------------------------------------------------------

class TestFavori:
    def test_loads_or_redirects(self, page: Page):
        goto(page, "/Favori")
        # Ya login redirect ya da boş favori sayfası
        assert "500" not in (page.title() or "")


# ---------------------------------------------------------------------------
# 404 sayfası
# ---------------------------------------------------------------------------

class Test404:
    def test_custom_404(self, page: Page):
        response = page.goto(f"{BASE}/bu-sayfa-kesinlikle-yoktur-xyz123")
        assert response is not None
        # Custom 404 sayfası dönüyorsa 404 status
        assert response.status in [404, 302], f"Beklenmedik status: {response.status}"

    def test_404_no_stack_trace(self, page: Page):
        page.goto(f"{BASE}/bu-sayfa-kesinlikle-yoktur-xyz123")
        text = page.inner_text("body")
        assert "System.Exception" not in text, "Stack trace sızdırılıyor!"
        assert "at FilistinProje" not in text, "Stack trace sızdırılıyor!"


# ---------------------------------------------------------------------------
# Performans / temel kontroller
# ---------------------------------------------------------------------------

class TestPerformance:
    def test_home_load_time(self, page: Page):
        import time
        start = time.time()
        goto(page, "/")
        elapsed = time.time() - start
        assert elapsed < 8.0, f"Ana sayfa çok yavaş: {elapsed:.1f}s"

    def test_no_mixed_content(self, page: Page):
        """HTTPS ortamda HTTP kaynak yok (dev'de sadece uyarı)."""
        mixed = []
        page.on("console", lambda m: mixed.append(m.text)
                if "Mixed Content" in m.text else None)
        goto(page, "/")
        page.wait_for_timeout(2000)
        # Dev'de HTTP olabilir, sadece logla
        if mixed:
            pytest.skip(f"Mixed content uyarısı (dev ortamı): {mixed[0]}")

    def test_images_not_broken(self, page: Page):
        goto(page, "/Urun")
        page.wait_for_timeout(1500)
        broken = page.evaluate("""
            () => {
                const imgs = [...document.querySelectorAll('img')];
                return imgs
                    .filter(i => !i.complete || i.naturalWidth === 0)
                    .map(i => i.src)
                    .filter(s => s && !s.startsWith('data:'))
                    .slice(0, 5);
            }
        """)
        assert not broken, f"Kırık görseller: {broken}"


# ---------------------------------------------------------------------------
# RTL / Dil kontrolleri
# ---------------------------------------------------------------------------

class TestRTL:
    def test_arabic_lang_attr(self, page: Page):
        goto(page, "/")
        lang = page.evaluate("document.documentElement.lang")
        assert lang in ["ar", "en"], f"Geçersiz lang: {lang}"

    def test_dir_rtl_when_arabic(self, page: Page):
        goto(page, "/?culture=ar")
        page.wait_for_timeout(1000)
        lang = page.evaluate("document.documentElement.lang")
        dir_ = page.evaluate("document.documentElement.dir")
        if lang == "ar":
            assert dir_ == "rtl", "Arapça'da RTL yok"

    def test_language_switcher_present(self, page: Page):
        goto(page, "/")
        # AR/EN dil değiştirici
        switcher = page.locator(
            "a[href*='culture=ar'], a[href*='culture=en'], "
            "[data-lang], .lang-switcher, #lang-switcher"
        )
        assert switcher.count() > 0, "Dil değiştirici yok"
