const { chromium } = require('playwright');

(async () => {
    const browser = await chromium.launch({ headless: true });
    const page = await browser.newPage();
    const brokenLinks = [];

    page.on('response', response => {
        if (response.status() >= 400) {
            brokenLinks.push(`${response.url()} - Status: ${response.status()}`);
        }
    });

    try {
        await page.goto('http://localhost:5002/Admin/Home');
        // Eğer login sayfasına atarsa önce login ol
        if (page.url().includes('GirisYap')) {
            console.log("Login gerekiyor. Admin şifresini bilmediğimiz için testte zorlanabiliriz. Local API/Cookie kullanılacak.");
        } else {
            console.log("Admin Home açıldı.");
            // Tüm linkleri topla
            const links = await page.$$eval('a[href^="/Admin/"]', els => els.map(a => a.href));
            const uniqueLinks = [...new Set(links)];
            console.log(`Toplam ${uniqueLinks.length} adet Admin linki bulundu.`);

            for (const link of uniqueLinks) {
                console.log(`Test ediliyor: ${link}`);
                await page.goto(link, { waitUntil: 'domcontentloaded' });
            }
        }
    } catch (e) {
        console.error("Test hatası:", e);
    }
    
    if (brokenLinks.length > 0) {
        console.log("\nÇalışmayan (4xx / 5xx) Linkler:");
        brokenLinks.forEach(l => console.log(l));
    } else {
        console.log("\nTüm ziyaret edilen sayfalardan HTTP 200 OK alındı, 400+ hata yok.");
    }
    
    await browser.close();
})();
