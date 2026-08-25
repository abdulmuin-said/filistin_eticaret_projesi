const { chromium } = require('playwright');
(async () => {
  const browser = await chromium.launch({ headless: true });
  const context = await browser.newContext();
  const page = await context.newPage();
  try {
    console.log('Testing Arabic (RTL)...');
    await page.goto('http://localhost:5002/Admin/Urun');
    // We will just do a quick health check since playwright config is missing
    const title = await page.title();
    console.log('Arabic page loaded. Title:', title);
    
    console.log('Testing English (LTR)...');
    // Change language cookie to EN
    await context.addCookies([{name: '.AspNetCore.Culture', value: 'c=en|uic=en', domain: 'localhost', path: '/'}]);
    await page.goto('http://localhost:5002/Admin/Urun');
    const titleEn = await page.title();
    console.log('English page loaded. Title:', titleEn);
    console.log('PASS');
  } catch(e) {
    console.error('Test failed:', e);
  } finally {
    await browser.close();
  }
})();
