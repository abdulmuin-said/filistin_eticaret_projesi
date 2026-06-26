/**
 * Firebase Cloud Messaging Service Worker
 *
 * Arka planda push bildirimlerini alır ve gösterir.
 * Firebase SDK'sı importScripts ile yüklenir.
 *
 * Graceful degradation: SDK yüklenemezse sessizce çalışmaz.
 */

// Firebase SDK'sını yükle
try {
    importScripts('https://www.gstatic.com/firebasejs/10.14.1/firebase-app-compat.js');
    importScripts('https://www.gstatic.com/firebasejs/10.14.1/firebase-messaging-compat.js');
} catch (e) {
    console.warn('[FirebaseSW] SDK yüklenemedi:', e);
}

// Service worker kurulumu
self.addEventListener('install', function (event) {
    console.log('[FirebaseSW] Yüklendi.');
    self.skipWaiting();
});

self.addEventListener('activate', function (event) {
    console.log('[FirebaseSW] Aktif.');
});

// Firebase yapılandırmasını client'tan al
self.addEventListener('message', function (event) {
    if (event.data && event.data.type === 'FIREBASE_CONFIG') {
        var config = event.data.config;
        if (!config || !config.apiKey) return;

        try {
            firebase.initializeApp(config);
            var messaging = firebase.messaging();

            messaging.onBackgroundMessage(function (payload) {
                var title = payload.notification?.title || '7ANRPS48';
                var body = payload.notification?.body || '';
                var icon = payload.notification?.icon || '/74anrps48logo2.svg';
                var clickUrl = payload.data?.click_url || '/';

                self.registration.showNotification(title, {
                    body: body,
                    icon: icon,
                    badge: '/74anrps48logo2.svg',
                    data: { click_url: clickUrl, firebase_payload: payload },
                    requireInteraction: true,
                    vibrate: [200, 100, 200]
                });
            });

            console.log('[FirebaseSW] Firebase messaging başlatıldı.');
        } catch (err) {
            console.warn('[FirebaseSW] Başlatma hatası:', err);
        }
    }
});

// Bildirime tıklandığında
self.addEventListener('notificationclick', function (event) {
    event.notification.close();

    var clickUrl = event.notification.data?.click_url || '/';

    event.waitUntil(
        clients.matchAll({ type: 'window', includeUncontrolled: true })
            .then(function (clientList) {
                // Açık pencere varsa ona yönlendir
                for (var i = 0; i < clientList.length; i++) {
                    var client = clientList[i];
                    if (client.url.includes(self.location.host) && 'focus' in client) {
                        client.postMessage({ type: 'NOTIFICATION_CLICKED', url: clickUrl });
                        return client.focus().then(function () {
                            return client.navigate(clickUrl);
                        });
                    }
                }
                // Yoksa yeni pencere aç
                if (clients.openWindow) {
                    return clients.openWindow(clickUrl);
                }
            })
    );
});
