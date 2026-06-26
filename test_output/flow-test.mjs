import { chromium } from 'playwright';

const BASE = 'http://localhost:5002';
const VIEWPORTS = [
  { name: 'mobile-375', width: 375, height: 812 },
  { name: 'desktop-1280', width: 1280, height: 800 },
];

async function test() {
  const browser = await chromium.launch({ headless: true });
  const results = {};

  for (const vp of VIEWPORTS) {
    console.log(`\n=== ${vp.name} ===`);
    results[vp.name] = {};
    const context = await browser.newContext({
      viewport: { width: vp.width, height: vp.height },
      locale: 'tr-TR',
    });
    const page = await context.newPage();

    // 1. Kayıt sayfası
    await page.goto(`${BASE}/Hesap/KayitOl`, { waitUntil: 'networkidle', timeout: 15000 });
    await page.waitForTimeout(500);

    const kayitOverflow = await page.evaluate(() => document.documentElement.scrollWidth > window.innerWidth + 2);
    results[vp.name].kayitNoOverflow = !kayitOverflow;
    console.log(`  Kayit overflow: ${kayitOverflow ? '❌' : '✅'}`);

    // Form alanları görünür mü?
    const formFields = await page.evaluate(() => {
      const inputs = document.querySelectorAll('#kayitForm input, #kayitForm select, #kayitForm textarea, #kayitForm button[type="submit"]');
      return inputs.length;
    });
    results[vp.name].kayitFormFields = formFields;
    console.log(`  Kayit form fields: ${formFields}`);

    await page.screenshot({
      path: `test_output/${vp.name}/kayit-form.png`,
      fullPage: true,
    });

    // 2. Ürün sayfasına git
    await page.goto(`${BASE}/Urun`, { waitUntil: 'networkidle', timeout: 15000 });
    await page.waitForTimeout(500);

    const urunOverflow = await page.evaluate(() => document.documentElement.scrollWidth > window.innerWidth + 2);
    results[vp.name].urunNoOverflow = !urunOverflow;
    console.log(`  Urun overflow: ${urunOverflow ? '❌' : '✅'}`);

    await page.screenshot({
      path: `test_output/${vp.name}/urun-liste.png`,
      fullPage: true,
    });

    // 3. Ürün detay
    await page.goto(`${BASE}/Urun/Detay/modern-soyut-kanvas-tablo-13`, { waitUntil: 'networkidle', timeout: 15000 });
    await page.waitForTimeout(500);

    const detayOverflow = await page.evaluate(() => document.documentElement.scrollWidth > window.innerWidth + 2);
    results[vp.name].detayNoOverflow = !detayOverflow;
    console.log(`  Detay overflow: ${detayOverflow ? '❌' : '✅'}`);

    await page.screenshot({
      path: `test_output/${vp.name}/urun-detay.png`,
      fullPage: true,
    });

    // 4. Sepete ekle
    const addBtn = await page.$('button:has-text("Sepete Ekle"), button:has-text("Add to Cart"), button:has-text("أضف إلى السلة")');
    if (addBtn) {
      await addBtn.click();
      await page.waitForTimeout(1500);
      console.log(`  Sepete ekle: ✅`);
    } else {
      // Try add via form
      const form = await page.$('form[action*="Sepet"]');
      if (form) {
        await form.evaluate(f => f.submit());
        await page.waitForTimeout(1500);
        console.log(`  Sepete ekle (form): ✅`);
      } else {
        console.log(`  Sepete ekle: ❌ no button found`);
      }
    }

    // 5. Sepet sayfası
    await page.goto(`${BASE}/Sepet`, { waitUntil: 'networkidle', timeout: 15000 });
    await page.waitForTimeout(500);

    const sepetOverflow = await page.evaluate(() => document.documentElement.scrollWidth > window.innerWidth + 2);
    results[vp.name].sepetNoOverflow = !sepetOverflow;
    console.log(`  Sepet overflow: ${sepetOverflow ? '❌' : '✅'}`);

    await page.screenshot({
      path: `test_output/${vp.name}/sepet.png`,
      fullPage: true,
    });

    // 6. Ödeme sayfası
    await page.goto(`${BASE}/Siparis/Odeme`, { waitUntil: 'networkidle', timeout: 15000 });
    await page.waitForTimeout(500);

    const odemeOverflow = await page.evaluate(() => document.documentElement.scrollWidth > window.innerWidth + 2);
    results[vp.name].odemeNoOverflow = !odemeOverflow;
    console.log(`  Odeme overflow: ${odemeOverflow ? '❌' : '✅'}`);

    await page.screenshot({
      path: `test_output/${vp.name}/odeme.png`,
      fullPage: true,
    });

    await context.close();
  }

  await browser.close();

  // Summary
  console.log('\n\n=== FINAL RESPONSIVE TEST RESULTS ===');
  for (const [vpName, vpResults] of Object.entries(results)) {
    console.log(`\n${vpName}:`);
    for (const [key, val] of Object.entries(vpResults)) {
      const icon = val === true ? '✅' : val === false ? '❌' : '✓';
      console.log(`  ${icon} ${key}: ${val}`);
    }
  }
}

test().catch(console.error);
