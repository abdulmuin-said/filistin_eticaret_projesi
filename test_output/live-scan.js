const { chromium } = require('playwright');
(async () => {
  const browser = await chromium.launch({ headless: true });
  const page = await browser.newPage({ viewport: { width: 1280, height: 800 } });

  console.log('=== SITE TARAMASI ===\n');

  const pages = [
    '/', '/Urun', '/Hesap/KayitOl', '/Hesap/GirisYap',
    '/Kurumsal/Hakkimizda', '/Kurumsal/Iletisim', '/Kurumsal/SSS',
    '/Favori', '/Sepet'
  ];

  let errors = [];

  for (const p of pages) {
    try {
      const resp = await page.goto('https://filistin.kastamonuesnaf.com.tr' + p, {
        waitUntil: 'networkidle', timeout: 20000
      });
      const status = resp.status();
      const title = await page.title();

      const bodyText = await page.textContent('body');
      const issues = [];

      if (bodyText.includes('Exception') || bodyText.includes('Hata') || bodyText.includes('Error')) {
        issues.push('Sayfada hata mesaji var');
      }

      const badLinks = await page.evaluate(() => {
        const links = Array.from(document.querySelectorAll('[href*="file://"], [src*="file://"]'));
        return links.length;
      });
      if (badLinks > 0) issues.push(badLinks + ' adet file:// referansi');

      const ogImg = await page.evaluate(() => {
        const meta = document.querySelector('meta[property="og:image"]');
        return meta ? meta.getAttribute('content') : '';
      });
      if (ogImg && ogImg.startsWith('file:')) issues.push('OG:image file:// referansi');

      const canon = await page.evaluate(() => {
        const link = document.querySelector('link[rel="canonical"]');
        return link ? link.getAttribute('href') : '';
      });
      if (canon && canon.startsWith('file:')) issues.push('Canonical file:// referansi');

      console.log((status === 200 ? 'OK' : 'FAIL') + ' ' + p + ' (' + status + ') ' + title.substring(0, 50));
      if (issues.length > 0) {
        issues.forEach(i => console.log('   ! ' + i));
        errors.push({ page: p, issues });
      }
    } catch (err) {
      console.log('FAIL ' + p + ' - ' + err.message.substring(0, 80));
      errors.push({ page: p, issues: [err.message] });
    }
  }

  console.log('\n=== SONUC ===');
  if (errors.length === 0) {
    console.log('Tum sayfalar sorunsuz!');
  } else {
    console.log(errors.length + ' sayfada sorun var:');
    errors.forEach(e => {
      console.log('  ' + e.page + ':');
      e.issues.forEach(i => console.log('    - ' + i));
    });
  }

  await browser.close();
})();
