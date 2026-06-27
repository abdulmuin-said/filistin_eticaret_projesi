const { chromium } = require('playwright');
const baseUrl = 'http://localhost:5002';

(async () => {
  const browser = await chromium.launch({ headless: true });

  // === DESKTOP DROPDOWN TEST ===
  const desktop = await browser.newContext({ viewport: { width: 1366, height: 900 } });
  const page = await desktop.newPage();
  await page.goto(baseUrl + '/', { waitUntil: 'networkidle' });

  const ddResult = await page.evaluate(() => {
    const items = document.querySelectorAll('[data-category-menu-item]');
    const results = [];
    items.forEach((item, idx) => {
      const link = item.querySelector('a');
      const label = link?.innerText?.trim() || 'unknown';
      const dd = item.querySelector('[data-category-dropdown]');
      if (!dd) {
        results.push({ idx, label, hasDropdown: false });
        return;
      }
      const before = window.getComputedStyle(dd).opacity;
      const beforeVis = window.getComputedStyle(dd).visibility;
      item.dispatchEvent(new MouseEvent('mouseenter', { bubbles: true }));
      const after = window.getComputedStyle(dd).opacity;
      const afterVis = window.getComputedStyle(dd).visibility;
      const links = dd.querySelectorAll('a[href^="/Urun?k="]');
      results.push({
        idx, label, hasDropdown: true,
        beforeOpacity: before, beforeVisibility: beforeVis,
        afterOpacity: after, afterVisibility: afterVis,
        linkCount: links.length,
        links: Array.from(links).map(l => l.innerText.trim())
      });
    });
    return results;
  });

  console.log('=== DESKTOP DROPDOWN TEST ===');
  ddResult.forEach(r => {
    if (!r.hasDropdown) {
      console.log(`[${r.idx}] ${r.label}: ❌ no dropdown element`);
      return;
    }
    const pass = r.afterOpacity === '1' && r.beforeOpacity === '0';
    console.log(`[${r.idx}] ${r.label}: ${pass ? '✅' : '❌'} before=${r.beforeOpacity} after=${r.afterOpacity} links=${r.linkCount}`);
    if (r.linkCount > 0) console.log(`     links: ${r.links.join(', ')}`);
  });

  // === TICKER TEST ===
  await page.goto(baseUrl + '/', { waitUntil: 'networkidle' });
  const tickerResult = await page.evaluate(() => {
    const track = document.querySelector('.trust-ticker-track');
    if (!track) return { exists: false };
    const style = window.getComputedStyle(track);
    const parent = document.querySelector('.trust-ticker');
    const parentStyle = parent ? window.getComputedStyle(parent) : null;
    const items = track.querySelectorAll('span');
    return {
      exists: true,
      animation: style.animation,
      animationDuration: style.animationDuration,
      animationName: style.animationName,
      itemCount: items.length,
      parentOverflow: parentStyle?.overflow,
      firstText: items[0]?.innerText?.trim()?.slice(0, 50)
    };
  });

  console.log('\n=== TICKER TEST ===');
  if (tickerResult.exists) {
    console.log(`✅ Animation: ${tickerResult.animationName} ${tickerResult.animationDuration}`);
    console.log(`✅ Items: ${tickerResult.itemCount}`);
    console.log(`✅ First: ${tickerResult.firstText}`);
    console.log(`✅ Parent overflow: ${tickerResult.parentOverflow}`);
  } else {
    console.log('❌ Ticker not found');
  }

  // === MOBILE HAMBURGER TEST ===
  const mobile = await browser.newContext({ viewport: { width: 390, height: 844 }, isMobile: true });
  const mPage = await mobile.newPage();
  await mPage.goto(baseUrl + '/', { waitUntil: 'networkidle' });

  const mobileResult = await mPage.evaluate(() => {
    const menuBtn = document.getElementById('mobileMenuBtn');
    const langToggle = document.getElementById('mobileLangToggle');
    const searchBtn = document.getElementById('mobileSearchBtn');
    if (!menuBtn || !langToggle || !searchBtn) {
      return {
        menuBtn: !!menuBtn,
        langToggle: !!langToggle,
        searchBtn: !!searchBtn,
        error: 'missing elements'
      };
    }
    const parent = menuBtn.parentElement;
    const children = [...parent.children];
    const order = children.map(c => c.id || c.className.slice(0, 30));
    return {
      order,
      menuBtnExists: true,
      langToggleExists: true,
      searchBtnExists: true,
      parentDir: parent.getAttribute('dir') || 'none'
    };
  });

  console.log('\n=== MOBILE HEADER ORDER TEST ===');
  if (mobileResult.error) {
    console.log(`❌ ${mobileResult.error}`);
  } else {
    console.log(`✅ Dir: ${mobileResult.parentDir}`);
    console.log(`✅ Order: ${mobileResult.order.join(' → ')}`);
    const isHamburgerFirst = mobileResult.order[0].includes('mobileMenuBtn');
    console.log(`✅ Hamburger first: ${isHamburgerFirst ? 'YES' : 'NO'}`);
  }

  await browser.close();
})().catch(e => { console.error(e); process.exit(1); });
