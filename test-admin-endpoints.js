const http = require('http');

const endpoints = [
    '/Admin/Urun', '/Admin/Kategori', '/Admin/UrunOzellik', 
    '/Admin/Slayt', '/Admin/AnaSayfa', '/Admin/HomeSections', 
    '/Admin/Kupon', '/Admin/Yorum', '/Admin/Bulten', '/Admin/Iletisim', 
    '/Admin/Home', '/Admin/Rapor', '/Admin/Ziyaretci', '/Admin/Search', 
    '/Admin/Bankalar', '/Admin/SosyalMedya', '/Admin/Ayarlar', 
    '/Admin/Kullanici', '/Admin/Personel', '/Admin/Toptanci', 
    '/Admin/Siparis', '/Admin/Kargo', '/Admin/Iade'
];

async function checkEndpoint(path) {
    return new Promise((resolve) => {
        const req = http.request({
            hostname: 'localhost',
            port: 5002,
            path: path,
            method: 'GET'
        }, (res) => {
            resolve({ path, status: res.statusCode, location: res.headers.location });
        });
        
        req.on('error', (e) => {
            resolve({ path, status: 'ERROR', message: e.message });
        });
        
        req.end();
    });
}

(async () => {
    console.log("Checking Admin Endpoints...");
    for (const endpoint of endpoints) {
        const result = await checkEndpoint(endpoint);
        console.log(`${result.status} - ${result.path} ${result.location ? '-> ' + result.location : ''}`);
    }
})();
