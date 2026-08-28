const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch({ headless: true });
  const context = await browser.newContext();
  const page = await context.newPage();

  console.log('Navigating to login page...');
  const response = await page.goto('http://localhost:5002/Hesap/GirisYap?ReturnUrl=%2FAdmin');
  console.log('Status:', response.status());

  console.log('Logging in...');
  
  try {
      // The controller takes "eposta" and "sifre" parameters.
      await page.fill('input[name="eposta"]', 'admin@filistin.com');
      await page.fill('input[name="sifre"]', 'Admin123!');
      
      // Wait for navigation and click properly
      await Promise.all([
          page.waitForNavigation({ timeout: 15000 }),
          page.click('form[action*="GirisYap"] button[type="submit"]')
      ]);
      
      console.log('Current URL after login:', page.url());
      
      if (page.url().includes('GirisYap')) {
          console.log('Failed to login! Checking why...');
          // Check for validation errors
          const errors = await page.evaluate(() => {
              return Array.from(document.querySelectorAll('.text-danger, .validation-summary-errors, .alert-danger'))
                  .map(el => el.textContent.trim());
          });
          console.log('Validation errors:', errors);
      } else {
          console.log('Login successful! Testing some pages...');
          
          const pagesToTest = [
              '/Admin',
              '/Admin/Urun',
              '/Admin/Kategori',
              '/Admin/Siparis',
              '/Admin/Kullanici',
              '/Admin/Ayarlar'
          ];
          
          let successCount = 0;
          for (const url of pagesToTest) {
              const fullUrl = `http://localhost:5002${url}`;
              try {
                  const res = await page.goto(fullUrl);
                  console.log(`[${res.status()}] ${fullUrl}`);
                  
                  // Check for obvious error signatures
                  const content = await page.content();
                  if (content.includes('NullReferenceException') || 
                      content.includes('An unhandled exception occurred') ||
                      content.includes('InvalidOperationException')) {
                      console.log(`  -> ERROR FOUND on page!`);
                  } else if (res.status() === 200) {
                      successCount++;
                  }
              } catch (e) {
                  console.log(`  -> FAILED TO LOAD: ${e.message}`);
              }
          }
          console.log(`Tested ${pagesToTest.length} pages. ${successCount} loaded successfully with 200 OK.`);
      }
  } catch (err) {
      console.error('Error during test:', err.message);
  }

  await browser.close();
})();
