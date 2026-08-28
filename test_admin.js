const { chromium } = require('playwright');
(async () => {
  const browser = await chromium.launch({ headless: true });
  const context = await browser.newContext();
  const page = await context.newPage();
  
  console.log("Navigating to login page...");
  const response = await page.goto('http://localhost:5002/Hesap/GirisYap');
  console.log(`Status: ${response.status()}`);
  
  await browser.close();
})();
