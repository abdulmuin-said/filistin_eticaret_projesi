const { chromium } = require('playwright');
(async () => {
  const browser = await chromium.launch({ headless: true });

  // Test 1: Ana sayfa
  const page1 = await browser.newPage({ viewport: { width: 1280, height: 800 } });
  await page1.goto('http://localhost:5002/', { waitUntil: 'networkidle', timeout: 15000 });
  const pageTitle = await page1.title();
  const body1 = await page1.textContent('body');

  console.log('=== TEST RAPORU ===');
  console.log('Ana Sayfa Basligi:', pageTitle.substring(0, 60));
  console.log('Sayfa Yuklendi:', pageTitle.length > 10 ? 'EVET' : 'HAYIR');

  // Ürün varlığını kontrol et (Arapça "منتجات" = ürünler)
  console.log('Urun var mi:', body1.includes('منتجات') || body1.includes('₪') ? 'EVET' : 'HAYIR');
  console.log('Navigasyon var mi:', body1.includes('اتصل') || body1.includes('قائمة') ? 'EVET' : 'HAYIR');

  // Para birimi (Şekel)
  if (body1.includes('₪')) {
    const matches = body1.match(/₪/g);
    console.log('Para birimi (₪) adedi:', matches ? matches.length : 0);
  }

  // Test 2: Ürün listesi
  await page1.goto('http://localhost:5002/Urun', { waitUntil: 'networkidle', timeout: 15000 });
  const urunBody = await page1.textContent('body');
  console.log('Urun sayfasi yuklendi:', urunBody.length > 100 ? 'EVET' : 'HAYIR');

  // Ürün fiyatı var mı?
  if (urunBody.includes('₪')) {
    const fiyatAdet = (urunBody.match(/₪/g) || []).length;
    console.log('Urun listesinde ₪ adedi:', fiyatAdet);
  }

  // Test 3: Sepet
  await page1.goto('http://localhost:5002/Sepet', { waitUntil: 'networkidle', timeout: 15000 });
  const sepetUrl = page1.url();
  console.log('Sepet sayfasi:', sepetUrl.includes('Sepet') ? 'CALISIYOR' : 'HATA');

  // Test 4: İletişim
  await page1.goto('http://localhost:5002/Kurumsal/Iletisim', { waitUntil: 'networkidle', timeout: 15000 });
  const iletisimStatus = await page1.title();
  console.log('Iletisim sayfasi:', iletisimStatus.length > 5 ? 'CALISIYOR' : 'HATA');

  console.log('\n=== TEST BITTI ===');

  // Screenshot
  await page1.screenshot({ path: 'test_output/local-test.png', fullPage: true });
  console.log('Ekran goruntusu: test_output/local-test.png');

  await browser.close();
})();
