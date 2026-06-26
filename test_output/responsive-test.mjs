import { chromium } from 'playwright';
import { writeFileSync, mkdirSync, existsSync } from 'fs';

const BASE = 'http://localhost:5002';
const VIEWPORTS = [
  { name: 'mobile-375', width: 375, height: 812 },
  { name: 'tablet-768', width: 768, height: 1024 },
  { name: 'desktop-1280', width: 1280, height: 800 },
];

const PAGES = [
  { path: '/', label: 'anasayfa' },
  { path: '/Urun', label: 'urun-liste' },
  { path: '/Urun/Detay/modern-soyut-kanvas-tablo-13', label: 'urun-detay' },
  { path: '/Sepet', label: 'sepet' },
  { path: '/Siparis/Odeme', label: 'odeme' },
  { path: '/Hesap/KayitOl', label: 'kayit' },
  { path: '/Hesap/GirisYap', label: 'giris' },
  { path: '/Favori', label: 'favori' },
  { path: '/Kurumsal/Iletisim', label: 'iletisim' },
  { path: '/Kurumsal/Hakkimizda', label: 'hakkimizda' },
];

// Findings collector
const findings = [];

async function capture(browser, viewport, pageDef) {
  const context = await browser.newContext({
    viewport: { width: viewport.width, height: viewport.height },
    locale: 'tr-TR',
  });
  const page = await context.newPage();
  const dir = `test_output/${viewport.name}`;
  if (!existsSync(dir)) mkdirSync(dir, { recursive: true });

  try {
    await page.goto(`${BASE}${pageDef.path}`, { waitUntil: 'networkidle', timeout: 15000 });
    await page.waitForTimeout(1000);

    // Full-page screenshot
    await page.screenshot({
      path: `${dir}/${pageDef.label}-full.png`,
      fullPage: true
    });

    // Check for common responsive issues
    const issues = [];

    // 1. Horizontal overflow
    const overflow = await page.evaluate(() => {
      const docWidth = document.documentElement.scrollWidth;
      const viewWidth = window.innerWidth;
      const overflowX = document.documentElement.scrollWidth > document.documentElement.clientWidth;
      return { docWidth, viewWidth, overflowX };
    });
    if (overflow.overflowX) {
      issues.push(`Horizontal overflow: doc=${overflow.docWidth}px > view=${overflow.viewWidth}px`);
    }

    // 2. Touch targets too small (< 44px) on mobile
    if (viewport.width <= 768) {
      const smallTargets = await page.evaluate(() => {
        const elements = document.querySelectorAll('a, button, input, select, [role="button"]');
        let small = 0;
        elements.forEach(el => {
          const rect = el.getBoundingClientRect();
          if (rect.width > 0 && rect.height > 0) {
            const w = rect.width;
            const h = rect.height;
            // Only flag if it's a clickable element that's visible
            const style = window.getComputedStyle(el);
            if (style.display !== 'none' && style.visibility !== 'hidden') {
              if (w < 36 || h < 36) small++;
            }
          }
        });
        return small;
      });
      if (smallTargets > 5) {
        issues.push(`${smallTargets} clickable elements are smaller than 36px (touch target risk)`);
      }
    }

    // 3. Mobile nav hidden state
    if (viewport.width < 768) {
      const mobileNav = await page.evaluate(() => {
        const nav = document.getElementById('mobileNav');
        return nav ? nav.classList.contains('hidden') : 'not-found';
      });
      // Mobile nav should be hidden by default on mobile (drawer)
      if (mobileNav === 'not-found') {
        issues.push('Mobile navigation drawer not found');
      }
    }

    // 4. Check text readability
    const textIssues = await page.evaluate(() => {
      const all = document.querySelectorAll('p, span, a, li, h1, h2, h3, h4, h5, h6');
      let tiny = 0;
      all.forEach(el => {
        const fs = parseFloat(getComputedStyle(el).fontSize);
        if (fs > 0 && fs < 10) tiny++;
      });
      return tiny;
    });
    if (textIssues > 10) {
      issues.push(`${textIssues} elements have font-size < 10px`);
    }

    // 5. Viewport meta tag
    const viewportMeta = await page.evaluate(() => {
      const meta = document.querySelector('meta[name="viewport"]');
      return meta ? meta.getAttribute('content') : null;
    });
    if (!viewportMeta) {
      issues.push('Missing viewport meta tag');
    } else if (!viewportMeta.includes('width=device-width')) {
      issues.push(`Viewport meta may not be mobile-optimized: "${viewportMeta}"`);
    }

    // 6. Position fixed/absolute elements that might overlap
    const overlaps = await page.evaluate(() => {
      const fixed = document.querySelectorAll('header, footer, .fixed, .sticky, [class*="fixed"], [class*="sticky"]');
      return fixed.length;
    });
    if (overlaps > 5) {
      issues.push(`${overlaps} fixed/sticky elements — potential overlap risk on small screens`);
    }

    if (issues.length > 0) {
      findings.push({ viewport: viewport.name, page: pageDef.label, issues });
    } else {
      findings.push({ viewport: viewport.name, page: pageDef.label, issues: ['✅ No responsive issues detected'] });
    }

    console.log(`✅ ${viewport.name} - ${pageDef.label}: ${issues.length > 0 ? issues.length + ' issue(s)' : 'OK'}`);

  } catch (err) {
    console.error(`❌ ${viewport.name} - ${pageDef.label}: ${err.message}`);
    findings.push({ viewport: viewport.name, page: pageDef.label, issues: [`ERROR: ${err.message}`] });
    // Screenshot even on error
    try {
      await page.screenshot({ path: `${dir}/${pageDef.label}-error.png`, fullPage: true });
    } catch {}
  } finally {
    await context.close();
  }
}

async function main() {
  const browser = await chromium.launch({ headless: true });

  for (const vp of VIEWPORTS) {
    console.log(`\n=== Testing ${vp.name} (${vp.width}x${vp.height}) ===`);
    for (const pg of PAGES) {
      await capture(browser, vp, pg);
    }
  }

  await browser.close();

  // Generate report
  console.log(`\n\n=====================================`);
  console.log(`RESPONSIVE TEST FINDINGS`);
  console.log(`=====================================`);

  const grouped = {};
  for (const f of findings) {
    if (!grouped[f.page]) grouped[f.page] = {};
    grouped[f.page][f.viewport] = f.issues;
  }

  for (const [pageName, viewports] of Object.entries(grouped)) {
    console.log(`\n📄 ${pageName.toUpperCase()}`);
    for (const [vpName, issues] of Object.entries(viewports)) {
      console.log(`  ${vpName}:`);
      issues.forEach(i => console.log(`    ${i}`));
    }
  }

  // Write findings as JSON
  writeFileSync('test_output/responsive-findings.json', JSON.stringify(findings, null, 2));
  console.log('\n\nFull report saved to test_output/responsive-findings.json');
}

main().catch(console.error);
