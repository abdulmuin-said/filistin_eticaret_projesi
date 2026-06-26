const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch({ headless: true });
  const viewports = [
    { name: 'mobile', width: 375, height: 812 },
    { name: 'tablet', width: 768, height: 1024 },
    { name: 'desktop', width: 1280, height: 800 }
  ];
  const paths = [
    '/',
    '/Urun',
    '/Sepet',
    '/Hesap/GirisYap',
    '/Hesap/KayitOl',
    '/Kurumsal/Hakkimizda',
    '/Kurumsal/Iletisim',
    '/Kurumsal/SSS',
    '/Kurumsal/Gizlilik',
    '/Kurumsal/KullaniciSozlesmesi',
    '/Kurumsal/MesafeliSatis',
    '/Kurumsal/IadeKosullari',
    '/admin'
  ];

  const results = [];

  for (const vp of viewports) {
    const page = await browser.newPage({ viewport: { width: vp.width, height: vp.height }, locale: 'ar' });

    for (const path of paths) {
      try {
        const resp = await page.goto('http://localhost:5002' + path, { waitUntil: 'networkidle', timeout: 20000 });
        const status = resp ? resp.status() : 0;
        const title = await page.title();
        const metrics = await page.evaluate(() => ({
          scrollWidth: document.documentElement.scrollWidth,
          clientWidth: document.documentElement.clientWidth,
          lang: document.documentElement.lang,
          dir: document.documentElement.dir,
          text: document.body.innerText,
          bodyLen: document.body.innerText.length
        }));

        const issues = [];
        if (status >= 500 || status === 0) issues.push('HTTP ' + status);
        if (metrics.text.includes('Exception') || metrics.text.includes('Stack trace') || metrics.text.includes('NullReferenceException')) {
          issues.push('Exception metni var');
        }
        if (metrics.scrollWidth > metrics.clientWidth + 2) {
          issues.push('Horizontal overflow ' + metrics.scrollWidth + '>' + metrics.clientWidth);
        }
        if (metrics.bodyLen < 50) issues.push('Sayfa çok boş');

        results.push({ viewport: vp.name, path, status, title: title.substring(0, 60), issues, lang: metrics.lang, dir: metrics.dir });
      } catch (e) {
        results.push({ viewport: vp.name, path, status: 0, title: '', issues: ['Timeout/Error: ' + e.message.substring(0, 100)] });
      }
    }

    await page.goto('http://localhost:5002/', { waitUntil: 'networkidle', timeout: 20000 });
    await page.screenshot({ path: 'test_output/' + vp.name + '-verification.png', fullPage: true });
    await page.close();
  }

  for (const r of results) {
    const ok = r.issues.length === 0 && r.status < 500 && r.status !== 0;
    console.log((ok ? 'OK' : 'FAIL') + ' [' + r.viewport + '] ' + r.path + ' (' + r.status + ') ' + r.title + ' lang=' + (r.lang || '') + ' dir=' + (r.dir || ''));
    for (const issue of r.issues) console.log('  - ' + issue);
  }

  const failures = results.filter(r => r.issues.length || r.status >= 500 || r.status === 0);
  console.log('\nFAILURES=' + failures.length);
  await browser.close();
})();
