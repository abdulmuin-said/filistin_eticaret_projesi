/**
 * 7ANRPS48.com — Countdown Timer (Geri Sayım Sayacı)
 * İndirimli/kampanyalı ürünlerde kullanılır.
 * 3 dil destekli, tema uyumlu.
 */
(function () {
  'use strict';

  var LANG = (document.documentElement.lang || 'ar').substring(0, 2);

  var TEXTS = {
    ar: { days: 'أيام', hours: 'ساعات', mins: 'دقائق', secs: 'ثواني', ended: 'انتهت!', label: 'ينتهي خلال' },
    en: { days: 'Days', hours: 'Hrs', mins: 'Min', secs: 'Sec', ended: 'Ended!', label: 'Ends in' },
    tr: { days: 'Gün', hours: 'Saat', mins: 'Dak', secs: 'San', ended: 'Bitti!', label: 'Bitişe kalan' },
  };

  var t = TEXTS[LANG] || TEXTS.ar;

  function initCountdown(element) {
    var endDateStr = element.getAttribute('data-end') || element.getAttribute('data-ends');
    if (!endDateStr) return;

    var endDate = new Date(endDateStr + 'Z');
    if (isNaN(endDate.getTime())) {
      endDate = new Date(endDateStr);
      if (isNaN(endDate.getTime())) return;
    }

    var daysEl = element.querySelector('.cd-days');
    var hoursEl = element.querySelector('.cd-hours');
    var minsEl = element.querySelector('.cd-mins');
    var secsEl = element.querySelector('.cd-secs');
    var endedEl = element.querySelector('.cd-ended');
    var counterEl = element.querySelector('.cd-counter');

    function tick() {
      var now = new Date().getTime();
      var diff = endDate.getTime() - now;

      if (diff <= 0) {
        if (counterEl) counterEl.style.display = 'none';
        if (endedEl) {
          endedEl.style.display = 'flex';
          endedEl.textContent = t.ended;
        }
        return;
      }

      var days = Math.floor(diff / (1000 * 60 * 60 * 24));
      var hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      var minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
      var seconds = Math.floor((diff % (1000 * 60)) / 1000);

      if (daysEl) daysEl.textContent = days;
      if (hoursEl) hoursEl.textContent = String(hours).padStart(2, '0');
      if (minsEl) minsEl.textContent = String(minutes).padStart(2, '0');
      if (secsEl) secsEl.textContent = String(seconds).padStart(2, '0');
    }

    tick();
    setInterval(tick, 1000);
  }

  // DOM'daki tüm countdown'ları başlat
  function scanCountdowns() {
    var elements = document.querySelectorAll('[data-countdown]');
    for (var i = 0; i < elements.length; i++) {
      initCountdown(elements[i]);
    }
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', scanCountdowns);
  } else {
    scanCountdowns();
  }

  // Harici kullanım için
  window.CountdownTimer = {
    init: initCountdown,
    scan: scanCountdowns,
  };
})();
