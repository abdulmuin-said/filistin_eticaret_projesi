/**
 * Firebase Web Push Bildirimleri — İstemci tarafı başlatma ve yönetim.
 *
 * İzin istemi kullanıcı etkileşiminden sonra (ör: "Bildirimlere İzin Ver" butonu)
 * veya sayfa yüklendiğinde otomatik olarak tetiklenir.
 *
 * Graceful degradation: Firebase yapılandırılmamışsa sessizce çalışmaz.
 */
(function () {
    'use strict';

    // ============================================================
    // KONFİGÜRASYON
    // ============================================================
    var FIREBASE_ENABLED = false; // Firebase SDK yüklüyse true olur

    // Firebase SDK'sı yüklendikten sonra çalışacak callback
    window.__firebaseReady = function (firebaseConfig) {
        if (!firebaseConfig || !firebaseConfig.apiKey) {
            console.log('[FirebasePush] Yapılandırma eksik, push bildirimi devre dışı.');
            return;
        }

        try {
            // Firebase'i başlat
            firebase.initializeApp(firebaseConfig);
            var messaging = firebase.messaging();
            FIREBASE_ENABLED = true;

            // Background mesajları (servis worker)
            messaging.onBackgroundMessage(function (payload) {
                console.log('[FirebasePush] Arka planda mesaj:', payload);
                var title = payload.notification?.title || '7ANRPS48';
                var body = payload.notification?.body || '';
                var icon = payload.notification?.icon || '/74anrps48logo2.svg';
                var clickUrl = payload.data?.click_url || '/';

                self.registration.showNotification(title, {
                    body: body,
                    icon: icon,
                    badge: '/74anrps48logo2.svg',
                    data: { click_url: clickUrl },
                    requireInteraction: true
                });
            });

            // Foreground mesajları
            messaging.onMessage(function (payload) {
                console.log('[FirebasePush] Ön planda mesaj:', payload);
                var title = payload.notification?.title || '7ANRPS48';
                var body = payload.notification?.body || '';

                // Toast bildirimi göster (custom)
                showPushToast(title, body);
            });

            // Mevcut token'ı kontrol et
            checkExistingToken(messaging);

            console.log('[FirebasePush] Firebase başarıyla başlatıldı.');
        } catch (err) {
            console.warn('[FirebasePush] Başlatma hatası:', err);
            FIREBASE_ENABLED = false;
        }
    };

    // ============================================================
    // İZİN İSTEME
    // ============================================================
    window.requestPushPermission = function () {
        if (!FIREBASE_ENABLED) {
            console.log('[FirebasePush] Firebase hazır değil.');
            return Promise.resolve(false);
        }

        return Notification.requestPermission()
            .then(function (permission) {
                if (permission === 'granted') {
                    return getFcmToken();
                } else {
                    console.log('[FirebasePush] Bildirim izni reddedildi.');
                    return false;
                }
            })
            .catch(function (err) {
                console.warn('[FirebasePush] İzin hatası:', err);
                return false;
            });
    };

    // ============================================================
    // FCM TOKEN AL / GÖNDER
    // ============================================================
    function getFcmToken() {
        try {
            var messaging = firebase.messaging();

            return messaging.getToken({ vapidKey: window.__firebaseVapidKey || '' })
                .then(function (token) {
                    if (token) {
                        // Token'ı sunucuya kaydet
                        return saveTokenToServer(token);
                    } else {
                        console.log('[FirebasePush] Token alınamadı.');
                        return false;
                    }
                })
                .catch(function (err) {
                    console.warn('[FirebasePush] Token alma hatası:', err);
                    return false;
                });
        } catch (err) {
            console.warn('[FirebasePush] getFcmToken hatası:', err);
            return Promise.resolve(false);
        }
    }

    function checkExistingToken(messaging) {
        messaging.getToken({ vapidKey: window.__firebaseVapidKey || '' })
            .then(function (token) {
                if (token) {
                    saveTokenToServer(token);
                }
            })
            .catch(function () {
                // Sessiz — token yoksa sorun değil
            });
    }

    function saveTokenToServer(token) {
        var payload = {
            token: token,
            tarayici: getBrowserInfo(),
            platform: getPlatformInfo()
        };

        return fetch('/PushBildirim/AboneOl', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                if (data.success) {
                    console.log('[FirebasePush] Token kaydedildi.');
                    return true;
                }
                return false;
            })
            .catch(function () {
                return false;
            });
    }

    // ============================================================
    // TOAST BİLDİRİMİ (Ön Plan)
    // ============================================================
    function showPushToast(title, body) {
        // Mevcut toast varsa kaldır
        var existing = document.getElementById('firebasePushToast');
        if (existing) existing.remove();

        var toast = document.createElement('div');
        toast.id = 'firebasePushToast';
        toast.style.cssText = [
            'position: fixed',
            'bottom: 24px',
            'right: 24px',
            'z-index: 9999',
            'background: #313511',
            'color: #f4f0ea',
            'padding: 16px 20px',
            'border-radius: 8px',
            'box-shadow: 0 8px 32px rgba(0,0,0,0.25)',
            'max-width: 360px',
            'font-family: inherit',
            'font-size: 14px',
            'line-height: 1.5',
            'cursor: pointer',
            'transition: all 0.3s ease',
            'animation: pushToastIn 0.4s ease'
        ].join(';');

        // İçerik — güvenli DOM API'leri ile
        var flexWrapper = document.createElement('div');
        flexWrapper.style.cssText = 'display:flex;justify-content:space-between;align-items:flex-start;gap:12px';

        var textWrapper = document.createElement('div');

        var strongEl = document.createElement('strong');
        strongEl.style.cssText = 'display:block;margin-bottom:4px;font-size:13px;text-transform:uppercase;letter-spacing:0.05em';
        strongEl.textContent = title;
        textWrapper.appendChild(strongEl);

        var spanEl = document.createElement('span');
        spanEl.style.cssText = 'font-size:13px;opacity:0.9';
        spanEl.textContent = body;
        textWrapper.appendChild(spanEl);

        flexWrapper.appendChild(textWrapper);

        var closeBtn = document.createElement('button');
        closeBtn.textContent = '×';
        closeBtn.style.cssText = 'background:none;border:none;color:#f4f0ea;cursor:pointer;font-size:18px;opacity:0.7;padding:0;line-height:1';
        closeBtn.onclick = function () { toast.remove(); };
        flexWrapper.appendChild(closeBtn);

        toast.appendChild(flexWrapper);
        document.body.appendChild(toast);

        // 8 saniye sonra otomatik kapan
        setTimeout(function () {
            if (toast.parentNode) {
                toast.style.opacity = '0';
                toast.style.transform = 'translateY(20px)';
                setTimeout(function () { toast.remove(); }, 400);
            }
        }, 8000);
    }

    // Toast animasyonu
    var styleSheet = document.createElement('style');
    styleSheet.textContent = [
        '@keyframes pushToastIn {',
        '  from { opacity: 0; transform: translateY(20px); }',
        '  to { opacity: 1; transform: translateY(0); }',
        '}'
    ].join('\n');
    document.head.appendChild(styleSheet);

    // ============================================================
    // YARDIMCI FONKSİYONLAR
    // ============================================================
    function getBrowserInfo() {
        var ua = navigator.userAgent || '';
        if (ua.includes('Chrome')) return 'Chrome ' + ua.match(/Chrome\/(\d+)/)?.[1] || '';
        if (ua.includes('Firefox')) return 'Firefox ' + ua.match(/Firefox\/(\d+)/)?.[1] || '';
        if (ua.includes('Safari')) return 'Safari ' + ua.match(/Version\/(\d+)/)?.[1] || '';
        if (ua.includes('Edg')) return 'Edge ' + ua.match(/Edg\/(\d+)/)?.[1] || '';
        return ua.substring(0, 60);
    }

    function getPlatformInfo() {
        var ua = navigator.userAgent || '';
        if (ua.includes('Windows')) return 'Windows';
        if (ua.includes('Mac')) return 'macOS';
        if (ua.includes('Android')) return 'Android';
        if (ua.includes('iPhone') || ua.includes('iPad')) return 'iOS';
        if (ua.includes('Linux')) return 'Linux';
        return 'Unknown';
    }

    function escapeHtml(text) {
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(text || ''));
        return div.innerHTML;
    }

})();
