/**
 * 7ANRPS48.com — Countdown Timer (Geri Sayım Sayacı)
 * İndirimli/kampanyalı ürünlerde kullanılır.
 * 3 dil destekli, tema uyumlu.
 */
(function () {
  'use strict';

  var LANG = (document.documentElement.lang || 'ar').substring(0, 2);

  var TEXTS = {
    ar: { days: 'أيام', hours: 'ساعات', mins: 'دقائق', secs: 'ثواني', ended: 'انتهى الخصم!', label: 'ينتهي الخصم خلال:' },
    en: { days: 'Days', hours: 'Hrs', mins: 'Min', secs: 'Sec', ended: 'Discount ended!', label: 'Discount ends in:' }
  };

  var t = TEXTS[LANG] || TEXTS.ar;

  function initCountdown(element) {
    if (element._cdInterval) {
      clearInterval(element._cdInterval);
      element._cdInterval = null;
    }

    var endDateStr = element.getAttribute('data-end') || element.getAttribute('data-ends');
    if (!endDateStr) return;

    var endDate = new Date(endDateStr.endsWith('Z') ? endDateStr : endDateStr + 'Z');
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

    function onExpired() {
      if (element._cdInterval) {
        clearInterval(element._cdInterval);
        element._cdInterval = null;
      }
      if (counterEl) counterEl.style.display = 'none';
      if (endedEl) {
        endedEl.classList.remove('hidden');
        endedEl.style.display = 'inline-flex';
        endedEl.textContent = t.ended;
      }

      // Revert product detail page price in real-time
      var originalPrice = parseFloat(element.getAttribute('data-original-price'));
      if (!isNaN(originalPrice) && originalPrice > 0 && typeof window.formatMoney === 'function') {
        var priceDisplay = document.getElementById('priceDisplay');
        if (priceDisplay) {
          priceDisplay.innerHTML = window.formatMoney(originalPrice);
        }
        var oldPriceDisplay = document.getElementById('oldPriceDisplay');
        if (oldPriceDisplay) oldPriceDisplay.classList.add('hidden');
        var discountBadge = document.getElementById('discountBadge');
        if (discountBadge) discountBadge.classList.add('hidden');
        var imageDiscountBadge = document.querySelector('.product-badge--discount');
        if (imageDiscountBadge) imageDiscountBadge.remove();
      }

      element.dispatchEvent(new CustomEvent('countdown:ended', { bubbles: true }));
    }

    function tick() {
      var now = new Date().getTime();
      var diff = endDate.getTime() - now;

      if (diff <= 0) {
        onExpired();
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
    element._cdInterval = setInterval(tick, 1000);
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
