const { chromium } = require('playwright');

const baseUrl = 'http://localhost:5002';
const routes = ['/', '/Urun', '/Sepet', '/Hesap/GirisYap', '/Hesap/KayitOl', '/Siparis/Odeme', '/Favori', '/Kurumsal/Iletisim'];

function rectSummary(rect) {
  if (!rect) return null;
  return {
    x: Math.round(rect.x),
    y: Math.round(rect.y),
    width: Math.round(rect.width),
    height: Math.round(rect.height),
    centerY: Math.round(rect.y + rect.height / 2)
  };
}

(async () => {
  const browser = await chromium.launch({ headless: true });
  const mobile = await browser.newContext({ viewport: { width: 390, height: 844 }, deviceScaleFactor: 2, isMobile: true });
  const desktop = await browser.newContext({ viewport: { width: 1366, height: 900 } });

  const page = await mobile.newPage();
  await page.goto(baseUrl + '/', { waitUntil: 'networkidle' });

  const homeMobile = await page.evaluate(() => {
    const header = document.querySelector('header')?.getBoundingClientRect();
    const logo = document.querySelector('#siteLogo')?.getBoundingClientRect();
    const icons = document.querySelector('#mobileIcons')?.getBoundingClientRect();
    const heroDots = document.querySelector('#heroDots')?.getBoundingClientRect();
    const heroTrackImages = [...document.querySelectorAll('#heroTrack img')].map(img => img.getAttribute('src'));
    const activeDots = [...document.querySelectorAll('.hero-dot')].map(dot => {
      const rect = dot.getBoundingClientRect();
      return { width: rect.width, height: rect.height, classes: dot.className };
    });
    return {
      header: header && { x: header.x, y: header.y, width: header.width, height: header.height },
      logo: logo && { x: logo.x, y: logo.y, width: logo.width, height: logo.height },
      icons: icons && { x: icons.x, y: icons.y, width: icons.width, height: icons.height },
      iconVsLogoCenterDelta: icons && logo ? Math.round((icons.y + icons.height / 2) - (logo.y + logo.height / 2)) : null,
      heroDots: heroDots && { x: heroDots.x, y: heroDots.y, width: heroDots.width, height: heroDots.height },
      activeDots,
      heroTrackImages,
      hasSlider1InHero: heroTrackImages.some(src => src && src.includes('/slider1.png')),
      scrollWidth: document.documentElement.scrollWidth,
      clientWidth: document.documentElement.clientWidth
    };
  });

  await page.screenshot({ path: 'test_output/mobile-home-after-fix.png', fullPage: true });

  await page.click('#mobileMenuBtn');
  await page.waitForTimeout(300);
  const mobileMenu = await page.evaluate(() => {
    const drawer = document.querySelector('#mobileNav')?.getBoundingClientRect();
    const firstDetails = document.querySelector('[data-mobile-category]');
    if (firstDetails) firstDetails.setAttribute('open', '');
    const subItems = document.querySelectorAll('[data-mobile-subitem]').length;
    return {
      drawerVisible: !!drawer && drawer.width > 0 && drawer.height > 0 && !document.querySelector('#mobileNav')?.classList.contains('hidden'),
      drawerWidth: drawer ? Math.round(drawer.width) : null,
      subItems
    };
  });

  const routeResults = [];
  for (const route of routes) {
    const p = await mobile.newPage();
    const response = await p.goto(baseUrl + route, { waitUntil: 'networkidle' });
    const metrics = await p.evaluate(() => ({
      title: document.title,
      statusText: document.body.innerText.slice(0, 120),
      scrollWidth: document.documentElement.scrollWidth,
      clientWidth: document.documentElement.clientWidth,
      hasHorizontalOverflow: document.documentElement.scrollWidth > document.documentElement.clientWidth + 2,
      mobileIconsVisible: !!document.querySelector('#mobileIcons') && getComputedStyle(document.querySelector('#mobileIcons')).display !== 'none',
      mobileMenuButtonVisible: !!document.querySelector('#mobileMenuBtn') && getComputedStyle(document.querySelector('#mobileMenuBtn')).display !== 'none'
    }));
    routeResults.push({ route, status: response ? response.status() : null, ...metrics });
    await p.close();
  }

  const dpage = await desktop.newPage();
  await dpage.goto(baseUrl + '/', { waitUntil: 'networkidle' });
  const desktopDropdown = await dpage.evaluate(async () => {
    const items = [...document.querySelectorAll('[data-category-menu-item]')];
    return { itemCount: items.length, labels: items.slice(0, 5).map(i => i.innerText.trim().replace(/\s+/g, ' ')) };
  });
  let hoverResult = null;
  for (let i = 0; i < desktopDropdown.itemCount; i++) {
    const item = dpage.locator('[data-category-menu-item]').nth(i);
    await item.hover();
    await dpage.waitForTimeout(350);
    hoverResult = await dpage.evaluate((index) => {
      const item = document.querySelectorAll('[data-category-menu-item]')[index];
      const dropdown = item?.querySelector('[data-category-dropdown]');
      const rect = dropdown?.getBoundingClientRect();
      const style = dropdown ? getComputedStyle(dropdown) : null;
      return {
        index,
        text: item?.innerText.trim().replace(/\s+/g, ' '),
        visible: !!dropdown && style.visibility !== 'hidden' && style.opacity !== '0' && rect.width > 0 && rect.height > 0,
        opacity: style?.opacity,
        visibility: style?.visibility,
        rect: rect && { x: Math.round(rect.x), y: Math.round(rect.y), width: Math.round(rect.width), height: Math.round(rect.height) },
        linkCount: dropdown ? dropdown.querySelectorAll('a[href^="/Urun?k="]').length : 0
      };
    }, i);
    if (hoverResult.visible) break;
  }
  await dpage.screenshot({ path: 'test_output/desktop-category-dropdown-after-fix.png', fullPage: false });

  console.log(JSON.stringify({ homeMobile, mobileMenu, routeResults, desktopDropdown, hoverResult }, null, 2));
  await browser.close();
})().catch(error => {
  console.error(error);
  process.exit(1);
});
