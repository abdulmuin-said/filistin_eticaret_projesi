const { chromium } = require('playwright');
const baseUrl = 'http://localhost:5002';

(async () => {
  const browser = await chromium.launch({ headless: true });

  // Mobile check
  const ctx = await browser.newContext({ viewport: { width: 390, height: 844 }, deviceScaleFactor: 2, isMobile: true });
  const page = await ctx.newPage();
  await page.goto(baseUrl + '/', { waitUntil: 'networkidle' });

  const mobileResult = await page.evaluate(() => {
    const header = document.querySelector('header');
    const langToggle = document.querySelector('#mobileLangToggle');
    const globeBtn = langToggle?.querySelector('button');
    const langCode = document.querySelector('#mobileLangCode');
    const leftGroup = document.querySelector('.absolute.left-3');
    const icons = [...leftGroup?.querySelectorAll('button, .relative') || []].map(el => {
      const ariaLabel = el.getAttribute('aria-label') || el.id || '';
      const html = el.innerHTML.slice(0, 60);
      return { ariaLabel, html };
    });

    return {
      leftGroupIconCount: icons.length,
      icons,
      langToggleExists: !!langToggle,
      globeBtnExists: !!globeBtn,
      langCodeText: langCode?.innerText,
      headerContainsLang: header?.innerHTML?.includes('toggleLangMenu') || false
    };
  });

  // Test popover opens
  await page.click('#mobileLangToggle button');
  await page.waitForTimeout(200);
  const popoverVisible = await page.evaluate(() => {
    const popover = document.getElementById('mobileLangPopover');
    return !popover?.classList.contains('hidden') && popover?.offsetParent !== null;
  });

  await page.click('#mobileLangToggle button');
  await page.waitForTimeout(200);
  const popoverHidden = await page.evaluate(() => {
    const popover = document.getElementById('mobileLangPopover');
    return popover?.classList.contains('hidden');
  });

  // Desktop check (md viewport: 900px)
  const dctx = await browser.newContext({ viewport: { width: 900, height: 800 } });
  const dpage = await dctx.newPage();
  await dpage.goto(baseUrl + '/', { waitUntil: 'networkidle' });

  const desktopResult = await dpage.evaluate(() => {
    const rightGroup = document.querySelector('.absolute.right-4');
    const hasDesktopPill = !!rightGroup?.querySelector('.rounded-full.border.border-\\[\\#d8c9aa\\]');
    const hasDesktopGlobe = !!document.querySelector('#desktopLangToggle');
    const hasDesktopLangPopover = !!document.querySelector('#desktopLangPopover');
    return {
      viewportWidth: window.innerWidth,
      desktopPillVisible: hasDesktopPill,
      desktopGlobeExists: hasDesktopGlobe,
      desktopLangPopoverExists: hasDesktopLangPopover,
      pillDisplay: rightGroup?.querySelector('.rounded-full') ? getComputedStyle(rightGroup.querySelector('.rounded-full')).display : 'not-found'
    };
  });

  // Desktop 1366px check
  const dctx2 = await browser.newContext({ viewport: { width: 1366, height: 900 } });
  const dpage2 = await dctx2.newPage();
  await dpage2.goto(baseUrl + '/', { waitUntil: 'networkidle' });

  const desktopWideResult = await dpage2.evaluate(() => {
    const rightGroup = document.querySelector('.absolute.right-4');
    const pill = rightGroup?.querySelector('[class*="rounded-full"][class*="border"]');
    return {
      viewportWidth: window.innerWidth,
      pillVisible: pill ? getComputedStyle(pill).display !== 'none' : 'not-found - pill might not exist'
    };
  });

  console.log(JSON.stringify({ mobileResult, popoverVisible, popoverHidden, desktopResult, desktopWideResult }, null, 2));
  await browser.close();
})().catch(e => { console.error(e); process.exit(1); });
