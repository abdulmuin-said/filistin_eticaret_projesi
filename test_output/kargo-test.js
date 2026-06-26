const { chromium } = require('playwright');
(async () => {
  const browser = await chromium.launch({ headless: true });
  const ctx = await browser.newContext({ locale: 'ar' });
  const page = await ctx.newPage({ viewport: { width: 1280, height: 800 } });

  // Add to cart via POST
  const formData = new URLSearchParams();
  formData.append('adet', '1');
  formData.append('urunId', '13');

  await page.goto('http://localhost:5002/Urun/Detay/modern-soyut-kanvas-tablo-13', { waitUntil: 'networkidle', timeout: 15000 });
  await page.waitForTimeout(500);

  // Find any form that targets Sepet/Ekle
  const forms = await page.evaluate(() => {
    return Array.from(document.querySelectorAll('form')).map(f => ({
      action: f.action,
      method: f.method,
      id: f.id
    }));
  });
  console.log('Forms:', JSON.stringify(forms, null, 2));

  // Try add to cart via AJAX if available
  const addResult = await page.evaluate(async () => {
    const addForm = document.querySelector('form[action*="Sepet/Ekle"], form[action*="Sepet"], form[action*="cart"]');
    if (addForm) {
      const btn = addForm.querySelector('button[type="submit"]');
      if (btn) {
        btn.click();
        await new Promise(r => setTimeout(r, 1500));
        return 'clicked submit';
      }
    }

    // Try a direct link/button with text
    const allBtns = Array.from(document.querySelectorAll('button, a'));
    for (const el of allBtns) {
      const txt = (el.textContent || '').toLowerCase();
      if (txt.includes('sepete') || txt.includes('ekle') || txt.includes('add') || txt.includes('cart') || txt.includes('إضافة') || txt.includes('سلة')) {
        el.click();
        await new Promise(r => setTimeout(r, 1500));
        return 'clicked: ' + txt.substring(0, 50);
      }
    }
    return 'no button found';
  });
  console.log('Add result:', addResult);

  // Navigate to checkout
  await page.goto('http://localhost:5002/Siparis/Odeme', { waitUntil: 'networkidle', timeout: 15000 });
  await page.waitForTimeout(1000);

  const currentUrl = page.url();
  console.log('Odeme URL:', currentUrl);

  const bodyText = await page.textContent('body');

  const checks = [
    { key: 'Kargo alani (Arapça رسوم الشحن)', val: bodyText.includes('رسوم') || bodyText.includes('الشحن') },
    { key: 'Sayfa yuklendi (hata yok)', val: !bodyText.includes('Error') && !bodyText.includes('Hata') },
    { key: 'Form alani', val: bodyText.includes('Sehir') || bodyText.includes('Şehir') || bodyText.includes('مدينة') },
  ];

  let allOk = true;
  checks.forEach(c => {
    console.log(c.val ? '✅' : '❌', c.key);
    if (!c.val) allOk = false;
  });

  await page.screenshot({ path: 'test_output/odeme-kargo-final.png', fullPage: true });
  console.log(allOk ? '\n✅ ALL CHECKS PASSED!' : '\n❌ SOME FAILED');
  await browser.close();
})();
