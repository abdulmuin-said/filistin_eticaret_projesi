import { test, expect } from '@playwright/test';

test('variant editor operations', async ({ page }) => {
  await page.goto('http://localhost:5002/Hesap/GirisYap');
  await page.fill('input[name="eposta"]', 'admin@7anrps48.com');
  await page.fill('input[name="sifre"]', 'Admin123!');
  await page.click('button:has-text("Giriş")'); // Update based on translation if needed, or simply press enter
  
  // Alternative login by pressing Enter
  await page.keyboard.press('Enter');
  await page.waitForTimeout(1000);

  await page.goto('http://localhost:5002/Admin/Urun/Ekle');

  // Let's add variations using evaluating javascript to bypass localization string mismatch
  await page.evaluate(() => {
     if(window.caVariantEditor) {
        window.caVariantEditor.add();
        window.caVariantEditor.add();
        window.caVariantEditor.add();
     } else {
        console.error('caVariantEditor not found');
     }
  });

  const variantCards = await page.$$('[data-variant-card]');
  console.log(`Added variants: ${variantCards.length}`);
  
  if (variantCards.length > 0) {
      // try to delete the first one
      const deleteButtons = await page.$$('[data-variant-remove]');
      if (deleteButtons.length > 0) {
          await deleteButtons[0].click();
          console.log(`Remaining variants after deletion: ${(await page.$$('[data-variant-card]')).length}`);
      }
      
      // Let's get the final HTML names to see if indexing is right
      const names = await page.evaluate(() => {
          return Array.from(document.querySelectorAll('[data-variant-card] input[data-variant-field="Id"]')).map(el => el.getAttribute('name'));
      });
      console.log('Final Input Names for Id:', names);
  }
});
