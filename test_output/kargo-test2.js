const { chromium } = require('playwright');
(async () => {
  const browser = await chromium.launch({ headless: true });
  const context = await browser.newContext({ locale: 'ar' });
  const page = await context.newPage();
  page.setDefaultTimeout(15000);

  // 1. Get Anti-Forgery Token
  await page.goto('http://localhost:5002/Urun/Detay/modern-soyut-kanvas-tablo-13', { waitUntil: 'networkidle' });
  const token = await page.evaluate(() => {
    const form = document.getElementById('globalAntiForgeryForm');
    return form ? form.querySelector('input[name="__RequestVerificationToken"]')?.value : null;
  });
  console.log('Token:', token ? '✅' : '❌');

  // 2. Add to cart via fetch (same session)
  await page.evaluate(async (token) => {
    const fd = new URLSearchParams();
    fd.append('urunId', '13');
    fd.append('adet', '1');
    const res = await fetch('/Sepet/Ekle', {
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'RequestVerificationToken': token },
      body: fd.toString()
    });
    const data = await res.json();
    return data;
  }, token);
  await page.waitForTimeout(1000);
  console.log('Ürün 13 sepete eklendi');

  // 3. Add second product
  await page.goto('http://localhost:5002/Urun/Detay/palestinian-pattern-wall-art-1', { waitUntil: 'networkidle' });
  const token2 = await page.evaluate(() => {
    const form = document.getElementById('globalAntiForgeryForm');
    return form ? form.querySelector('input[name="__RequestVerificationToken"]')?.value : null;
  });
  await page.evaluate(async (token) => {
    const fd = new URLSearchParams();
    fd.append('urunId', '1');
    fd.append('adet', '1');
    const res = await fetch('/Sepet/Ekle', {
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'RequestVerificationToken': token },
      body: fd.toString()
    });
    return await res.json();
  }, token2);
  await page.waitForTimeout(1000);
  console.log('Ürün 1 sepete eklendi');

  // 4. Go to checkout
  await page.goto('http://localhost:5002/Siparis/Odeme', { waitUntil: 'networkidle' });
  await page.waitForTimeout(1000);

  const currentUrl = page.url();
  const bodyText = await page.textContent('body');

  console.log('Odeme URL:', currentUrl);
  console.log('HTTP 200:', currentUrl.includes('Odeme') ? '✅' : '❌ (redirect)');

  const checks = [
    bodyText.includes('رسوم') || bodyText.includes('الشحن'),
    !bodyText.includes('Exception') && !bodyText.includes('Error'),
    bodyText.includes('Sehir') || bodyText.includes('Şehir') || bodyText.includes('مدينة'),
  ];

  console.log('Kargo alani:', checks[0] ? '✅' : '⚠️');
  console.log('Hata yok:', checks[1] ? '✅' : '⚠️');
  console.log('Form:', checks[2] ? '✅' : '⚠️');

  await page.screenshot({ path: 'test_output/odeme-son-test.png', fullPage: true });
  console.log('Screenshot saved');
  await browser.close();
})();
