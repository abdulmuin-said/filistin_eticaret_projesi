#!/bin/bash
npm i -D playwright
cat << 'JS' > test_admin.js
const { chromium } = require('playwright');
(async () => {
  const browser = await chromium.launch({ headless: true });
  const context = await browser.newContext();
  const page = await context.newPage();
  
  // Set auth cookie if needed or do login
  // Here we'll try to just check the login page first
  console.log("Navigating to login page...");
  const response = await page.goto('http://localhost:5002/Hesap/GirisYap');
  console.log(`Status: ${response.status()}`);
  
  // Enter credentials
  console.log("Logging in...");
  await page.fill('input[name="Email"]', 'admin@example.com');
  await page.fill('input[name="Password"]', 'Admin123!');
  await Promise.all([
    page.waitForNavigation(),
    page.click('button[type="submit"]')
  ]);
  console.log(`Current URL after login: ${page.url()}`);
  
  // Now hit admin endpoints
  const adminUrls = [
    '/admin/dashboard',
    '/admin/urunler',
    '/admin/kategoriler',
    '/admin/siparisler',
    '/admin/kuponlar',
    '/admin/iade',
    '/admin/kargo',
    '/admin/kullanicilar',
    '/admin/ayarlar',
    '/admin/bulten',
    '/admin/yorumlar'
  ];
  
  let failed = false;
  for (const url of adminUrls) {
    const fullUrl = `http://localhost:5002${url}`;
    try {
      const resp = await page.goto(fullUrl, { waitUntil: 'domcontentloaded' });
      if (resp && resp.status() !== 200) {
        console.error(`❌ FAILED: ${url} returned ${resp.status()}`);
        failed = true;
      } else {
        console.log(`✅ OK: ${url}`);
      }
    } catch (e) {
      console.error(`❌ CRASH: ${url} - ${e.message}`);
      failed = true;
    }
  }
  
  await browser.close();
  process.exit(failed ? 1 : 0);
})();
JS
node test_admin.js
