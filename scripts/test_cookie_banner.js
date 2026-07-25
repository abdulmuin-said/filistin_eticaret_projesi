const { chromium } = require('playwright');
const path = require('path');

(async () => {
  let browser;
  try {
    browser = await chromium.launch({ channel: 'msedge', headless: true });
  } catch (e) {
    try {
      browser = await chromium.launch({ channel: 'chrome', headless: true });
    } catch (e2) {
      console.error("Could not launch chrome/edge", e2);
      process.exit(1);
    }
  }
  
  const context = await browser.newContext({
    viewport: { width: 1280, height: 800 }
  });
  const page = await context.newPage();
  
  await page.goto('http://localhost:5002', { waitUntil: 'networkidle' });
  await page.evaluate(() => localStorage.clear());
  await page.reload({ waitUntil: 'networkidle' });
  await page.waitForTimeout(1000);
  
  const screenshotPath = path.join(__dirname, '..', 'cookie_banner_screenshot.png');
  await page.screenshot({ path: screenshotPath, fullPage: false });
  console.log(`Screenshot saved to ${screenshotPath}`);

  await page.setViewportSize({ width: 390, height: 844 });
  const mobileScreenshotPath = path.join(__dirname, '..', 'cookie_banner_mobile.png');
  await page.screenshot({ path: mobileScreenshotPath, fullPage: false });
  console.log(`Mobile screenshot saved to ${mobileScreenshotPath}`);

  await browser.close();
})();
