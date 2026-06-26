/**
 * 7ANRPS48.com — Çarkıfelek (Spin the Wheel)
 * Canvas tabanlı, 3 dil (AR/EN/TR), marka uyumlu kupon kodu,
 * otomatik sepete ekleme, giriş zorunluluğu.
 * Ödüller admin panelden (CarkOdul) yönetilir.
 */
window.SpinWheel = (function () {
  'use strict';

  var LANG = (document.documentElement.lang || 'ar').substring(0, 2);

  var TEXTS = {
    ar: {
      title: 'جرب حظك!', subtitle: 'دور العجلة واربح قسيمة خصم!',
      spinBtn: 'دور', spinning: 'جاري الدوران...', close: 'إغلاق',
      tryAgain: 'حاول مرة أخرى!', tryAgainSub: 'ربما في المرة القادمة.',
      discountTitle: 'تهانينا! خصم %S%!', discountSub: 'قسيمتك أدناه. استخدمها في سلة المشتريات.',
      freeshipTitle: 'تهانينا! شحن مجاني!', freeshipSub: 'قسيمتك أدناه. استخدمها في طلبك القادم.',
      useCoupon: 'استخدم القسيمة', loginToUse: 'سجل الدخول واستخدم',
      copied: 'تم النسخ!', startShopping: 'ابدأ التسوق', copyCode: 'انسخ الكود',
      oneChance: '*كل مستخدم لديه فرصة واحدة فقط.',
    },
    en: {
      title: 'Try Your Luck!', subtitle: 'Spin the wheel and win a discount coupon!',
      spinBtn: 'Spin', spinning: 'Spinning...', close: 'Close',
      tryAgain: 'Try Again!', tryAgainSub: 'Better luck next time.',
      discountTitle: 'Congratulations! %S% Off!', discountSub: 'Your coupon code is below. Use it in your cart.',
      freeshipTitle: 'Congratulations! Free Shipping!', freeshipSub: 'Your coupon code is below. Use it on your next order.',
      useCoupon: 'Use Coupon', loginToUse: 'Login to Use',
      copied: 'Copied!', startShopping: 'Start Shopping', copyCode: 'Copy Code',
      oneChance: '*Each user gets one spin only.',
    },
    tr: {
      title: 'Şansını Dene!', subtitle: 'Çarkı çevir, indirim kuponu kazan!',
      spinBtn: 'Çevir', spinning: 'Çevriliyor...', close: 'Kapat',
      tryAgain: 'Tekrar Dene!', tryAgainSub: 'Bir şans daha şansını getirebilir.',
      discountTitle: 'Tebrikler! %S% İndirim!', discountSub: 'Kupon kodun aşağıda. Sepetinde kullanabilirsin.',
      freeshipTitle: 'Tebrikler! Ücretsiz Kargo!', freeshipSub: 'Kupon kodun aşağıda. Bir sonraki siparişinde kullanabilirsin.',
      useCoupon: 'Kuponu Kullan', loginToUse: 'Giriş Yap ve Kullan',
      copied: 'Kopyalandı!', startShopping: 'Alışverişe Başla', copyCode: 'Kodu Kopyala',
      oneChance: '*Her kullanıcı bir kez çevirme hakkına sahiptir.',
    }
  };

  var t = TEXTS[LANG] || TEXTS.ar;

  // --- STATE ---
  var PRIZES = [];
  var SEGMENT_COUNT = 0;
  var ARC = 0;
  var isSpinning = false;
  var currentRotation = 0;
  var animationId = null;
  var wheelCanvas = null;
  var ctx = null;
  var onCompleteCallback = null;
  var overlay = null;
  var resultModal = null;
  var isLoggedIn = false;
  var lastCouponCode = null;
  var lastPrize = null;
  var prizesLoaded = false;

  function generateCouponCode() {
    var chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
    var code = '7ANRPS48-';
    for (var i = 0; i < 6; i++) { code += chars.charAt(Math.floor(Math.random() * chars.length)); }
    return code;
  }

  function drawWheel(rotation) {
    var canvas = wheelCanvas;
    if (!canvas) return;
    ctx = ctx || canvas.getContext('2d');
    var dpr = window.devicePixelRatio || 1;
    var size = canvas.width / dpr;
    var radius = size / 2 - 8;
    var center = size / 2;

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    ctx.beginPath();
    ctx.arc(center * dpr, center * dpr, (radius + 4) * dpr, 0, 2 * Math.PI);
    ctx.fillStyle = '#b58735';
    ctx.fill();

    ctx.beginPath();
    ctx.arc(center * dpr, center * dpr, (radius + 1) * dpr, 0, 2 * Math.PI);
    ctx.fillStyle = '#2a2a1a';
    ctx.fill();

    ctx.save();
    ctx.translate(center * dpr, center * dpr);
    ctx.rotate(rotation);
    ctx.translate(-center * dpr, -center * dpr);

    for (var i = 0; i < SEGMENT_COUNT; i++) {
      if (!PRIZES[i]) continue;
      var label = PRIZES[i].label;
      var startAngle = i * ARC - Math.PI / 2;
      var endAngle = startAngle + ARC;

      ctx.beginPath();
      ctx.moveTo(center * dpr, center * dpr);
      ctx.arc(center * dpr, center * dpr, radius * dpr, startAngle, endAngle);
      ctx.closePath();
      ctx.fillStyle = PRIZES[i].color;
      ctx.fill();
      ctx.strokeStyle = 'rgba(255,255,255,0.15)';
      ctx.lineWidth = 1 * dpr;
      ctx.stroke();

      ctx.save();
      ctx.translate(center * dpr, center * dpr);
      ctx.rotate(startAngle + ARC / 2);
      ctx.textAlign = 'right';
      ctx.fillStyle = '#ffffff';
      ctx.shadowColor = 'rgba(0,0,0,0.6)';
      ctx.shadowBlur = 3 * dpr;

      var maxWidth = radius * dpr * 0.6;
      var fontSize = 11 * dpr;
      ctx.font = 'bold ' + fontSize + 'px Manrope, sans-serif';
      while (ctx.measureText(label).width > maxWidth && fontSize > 7 * dpr) {
        fontSize -= 0.5 * dpr;
        ctx.font = 'bold ' + fontSize + 'px Manrope, sans-serif';
      }
      ctx.fillText(label, radius * dpr * 0.9, 4 * dpr);
      ctx.restore();
    }
    ctx.restore();

    var gradient = ctx.createRadialGradient(center * dpr, center * dpr, 0, center * dpr, center * dpr, 20 * dpr);
    gradient.addColorStop(0, '#d4a84b');
    gradient.addColorStop(0.6, '#b58735');
    gradient.addColorStop(1, '#8a6a2a');
    ctx.beginPath();
    ctx.arc(center * dpr, center * dpr, 20 * dpr, 0, 2 * Math.PI);
    ctx.fillStyle = gradient;
    ctx.fill();
    ctx.strokeStyle = '#6f5d2a';
    ctx.lineWidth = 2 * dpr;
    ctx.stroke();

    ctx.beginPath();
    ctx.moveTo(center * dpr, 2 * dpr);
    ctx.lineTo((center - 11) * dpr, 24 * dpr);
    ctx.lineTo((center + 11) * dpr, 24 * dpr);
    ctx.closePath();
    ctx.fillStyle = '#c0392b';
    ctx.fill();
    ctx.strokeStyle = '#a93226';
    ctx.lineWidth = 1.5 * dpr;
    ctx.stroke();
  }

  function spinWheel(targetRotation, duration) {
    var startRotation = currentRotation;
    var totalRotation = targetRotation - startRotation;
    var startTime = performance.now();
    function animate(time) {
      var elapsed = time - startTime;
      var progress = Math.min(elapsed / duration, 1);
      var eased = 1 - Math.pow(1 - progress, 4);
      currentRotation = startRotation + totalRotation * eased;
      drawWheel(currentRotation);
      if (progress < 1) {
        animationId = requestAnimationFrame(animate);
      } else {
        currentRotation = targetRotation;
        isSpinning = false;
        if (onCompleteCallback) onCompleteCallback();
      }
    }
    isSpinning = true;
    animationId = requestAnimationFrame(animate);
  }

  function getPrizeIndex(finalRotation) {
    var norm = (((finalRotation % (2 * Math.PI)) + 2 * Math.PI) % (2 * Math.PI));
    var idx = Math.floor(norm / ARC);
    return Math.min(idx, SEGMENT_COUNT - 1);
  }

  function showResult(prize) {
    if (!resultModal) return;
    lastPrize = prize;

    var iconEl = resultModal.querySelector('.sw-result-icon');
    var titleEl = resultModal.querySelector('.sw-result-title');
    var subtitleEl = resultModal.querySelector('.sw-result-subtitle');
    var codeEl = resultModal.querySelector('.sw-coupon-code');
    var copyBtn = resultModal.querySelector('.sw-copy-btn');
    var actionBtn = resultModal.querySelector('.sw-claim-btn');
    var actionBtn2 = resultModal.querySelector('.sw-claim-btn-alt');

    if (prize.type === 'none') {
      iconEl.innerHTML = '<svg class="w-16 h-16 mx-auto text-[#6f6a5e]" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><circle cx="12" cy="12" r="10"/><path d="M8 15l8-8M8 7l8 8" stroke-linecap="round"/></svg>';
      titleEl.textContent = t.tryAgain;
      subtitleEl.textContent = prize.msg || t.tryAgainSub;
      codeEl.classList.add('hidden');
      if (copyBtn) copyBtn.classList.add('hidden');
      if (actionBtn2) actionBtn2.classList.add('hidden');
      actionBtn.textContent = t.close;
      actionBtn.className = 'sw-claim-btn w-full bg-[#6f6a5e] hover:bg-[#5a554a] text-white font-semibold text-sm tracking-widest uppercase py-3 rounded-xl shadow transition-all duration-300';
      actionBtn.onclick = closeWheel;
      document.querySelector('.sw-wheel-container')?.classList.add('hidden');
      resultModal.classList.remove('hidden');
      return;
    }

    lastCouponCode = generateCouponCode();

    if (prize.type === 'discount') {
      iconEl.innerHTML = '<svg class="w-16 h-16 mx-auto text-[#b58735]" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M9.813 15.904L15.904 9.813M21 12a9 9 0 11-18 0 9 9 0 0118 0z" stroke-linecap="round" stroke-linejoin="round"/></svg>';
      titleEl.textContent = (prize.msg || t.discountTitle).replace('%S%', prize.value);
      subtitleEl.textContent = t.discountSub;
    } else {
      iconEl.innerHTML = '<svg class="w-16 h-16 mx-auto text-[#b58735]" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><path d="M8.25 18.75a1.5 1.5 0 01-3 0m3 0a1.5 1.5 0 00-3 0m3 0h6m-9 0H3.375a1.125 1.125 0 01-1.125-1.125V14.25m17.25 4.5a1.5 1.5 0 01-3 0m3 0a1.5 1.5 0 00-3 0m3 0h1.125c.621 0 1.129-.504 1.09-1.124a17.902 17.902 0 00-3.213-9.193 2.056 2.056 0 00-1.58-.86H14.25M16.5 18.75h-2.25m0-11.177v-.958c0-.568-.422-1.048-.987-1.106a48.554 48.554 0 00-10.026 0 1.106 1.106 0 00-.987 1.106v7.635m12-6.677v6.677m0 4.5v-4.5m0 0h-12" stroke-linecap="round" stroke-linejoin="round"/></svg>';
      titleEl.textContent = prize.msg || t.freeshipTitle;
      subtitleEl.textContent = t.freeshipSub;
    }

    codeEl.textContent = lastCouponCode;
    codeEl.classList.remove('hidden');

    if (isLoggedIn) {
      if (copyBtn) {
        copyBtn.classList.remove('hidden');
        copyBtn.textContent = t.copyCode;
        copyBtn.onclick = function () { copyToClipboard(lastCouponCode, copyBtn, t.copied); };
      }
      if (actionBtn2) actionBtn2.classList.add('hidden');
      actionBtn.textContent = t.useCoupon;
      actionBtn.className = 'sw-claim-btn w-full bg-[#b58735] hover:bg-[#9a6d24] text-white font-semibold text-sm tracking-widest uppercase py-3 rounded-xl shadow-lg hover:shadow-xl transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed';
      actionBtn.onclick = function () { claimCoupon(lastCouponCode, prize); };
    } else {
      if (copyBtn) copyBtn.classList.add('hidden');
      actionBtn.textContent = t.loginToUse;
      actionBtn.className = 'sw-claim-btn w-full bg-[#b58735] hover:bg-[#9a6d24] text-white font-semibold text-sm tracking-widest uppercase py-3 rounded-xl shadow-lg hover:shadow-xl transition-all duration-300';
      actionBtn.onclick = function () {
        window.location.href = '/Hesap/GirisYap?returnUrl=' + encodeURIComponent('/Sepet/KuponUygula?kuponKodu=' + encodeURIComponent(lastCouponCode));
      };
      if (actionBtn2) {
        actionBtn2.classList.remove('hidden');
        actionBtn2.textContent = t.copyCode;
        actionBtn2.className = 'sw-claim-btn-alt w-full bg-[#313511] hover:bg-[#1c2001] text-[#b58735] font-semibold text-sm tracking-widest uppercase py-2.5 rounded-xl border border-[#b58735]/30 transition-all duration-300';
        actionBtn2.onclick = function () { copyToClipboard(lastCouponCode, actionBtn2, t.copied); };
      }
    }

    document.querySelector('.sw-wheel-container')?.classList.add('hidden');
    resultModal.classList.remove('hidden');
  }

  function copyToClipboard(code, btn, successText) {
    function done() {
      if (!btn) return;
      var orig = btn.textContent;
      btn.textContent = successText || t.copied; btn.disabled = true;
      setTimeout(function () { btn.textContent = orig; btn.disabled = false; }, 2000);
    }
    if (navigator.clipboard && navigator.clipboard.writeText) {
      navigator.clipboard.writeText(code).then(done)['catch'](function () { fallbackCopy(code, done); });
    } else { fallbackCopy(code, done); }
  }

  function fallbackCopy(code, cb) {
    var ta = document.createElement('textarea');
    ta.value = code; ta.style.position = 'fixed'; ta.style.opacity = '0';
    document.body.appendChild(ta); ta.select();
    try { document.execCommand('copy'); } catch (e) { /* ignore */ }
    document.body.removeChild(ta); if (cb) cb();
  }

  function claimCoupon(code, prize) {
    if (!code) return;
    var btn = document.querySelector('.sw-claim-btn');
    if (btn) { btn.disabled = true; btn.textContent = t.copied + '...'; }
    fetch('/api/wheel/claim', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': getAntiForgeryToken() },
      body: JSON.stringify({ code: code, discountType: prize.type, discountValue: prize.value })
    }).then(function (r) { return r.json(); }).then(function (data) {
      if (data.redirect) window.location.href = data.redirect;
      else if (data.error) { if (btn) { btn.disabled = false; btn.textContent = t.useCoupon; } alert(data.error); }
    })['catch'](function () { window.location.href = '/Sepet'; });
  }

  function getAntiForgeryToken() {
    var el = document.querySelector('input[name="__RequestVerificationToken"]');
    return el ? el.value : '';
  }

  function openWheel() {
    if (!overlay || !prizesLoaded) return;
    overlay.classList.remove('hidden');
    document.body.style.overflow = 'hidden';
    if (resultModal) resultModal.classList.add('hidden');
    var wc = document.querySelector('.sw-wheel-container');
    if (wc) wc.classList.remove('hidden');
    currentRotation = 0;
    drawWheel(currentRotation);
    setTimeout(function () {
      if (!isSpinning) { var btn = document.querySelector('.sw-spin-btn'); if (btn) btn.click(); }
    }, 800);
  }

  function closeWheel() {
    if (!overlay) return;
    overlay.classList.add('hidden');
    document.body.style.overflow = '';
    try { localStorage.setItem('sw_shown', 'true'); } catch (e) { /* ignore */ }
  }

  function triggerSpin() {
    if (isSpinning) return;
    var btn = document.querySelector('.sw-spin-btn');
    if (btn) { btn.disabled = true; btn.textContent = t.spinning; }
    var fullRotations = (5 + Math.floor(Math.random() * 3)) * 2 * Math.PI;
    var targetRotation = currentRotation + fullRotations + Math.random() * 2 * Math.PI;
    onCompleteCallback = function () {
      var idx = getPrizeIndex(currentRotation);
      var prize = PRIZES[idx];
      showResult(prize);
      if (btn) { btn.disabled = false; btn.textContent = t.spinBtn; }
    };
    spinWheel(targetRotation, 4500);
  }

  function loadPrizes(callback) {
    fetch('/api/wheel/prizes')
      .then(function (r) { return r.json(); })
      .then(function (data) {
        if (!data || !data.length) { PRIZES = []; SEGMENT_COUNT = 0; ARC = 0; prizesLoaded = true; if (callback) callback(); return; }

        var langLabels = { ar: 'labelAr', en: 'labelEn', tr: 'labelTr' };
        var langMsgs = { ar: 'mesajAr', en: 'mesajEn', tr: 'mesajTr' };
        var labelKey = langLabels[LANG] || 'labelTr';
        var msgKey = langMsgs[LANG] || 'mesajTr';

        PRIZES = data.map(function (p) {
          return {
            label: p[labelKey] || p.labelTr,
            value: p.deger,
            type: p.tip,
            color: p.renk,
            msg: p[msgKey] || null
          };
        });

        SEGMENT_COUNT = PRIZES.length;
        ARC = SEGMENT_COUNT > 0 ? (2 * Math.PI) / SEGMENT_COUNT : 0;
        prizesLoaded = true;
        if (callback) callback();
      })
      ['catch'](function () {
        // Fallback to defaults if API fails
        var labels = { ar: { d: ['خصم 5%','خصم 10%','خصم 15%','خصم 20%','خصم 25%','شحن مجاني','حاول مرة أخرى','خصم 10%'], f: ['discount','discount','discount','discount','discount','freeship','none','discount'], v: [5,10,15,20,25,0,0,10], c: ['#313511','#b58735','#4a4a2e','#c49a3c','#313511','#b58735','#6f6a5e','#d4a84b'] } };
        var fallback = labels['ar'];
        PRIZES = (fallback.d || []).map(function (l, i) { return { label: l, value: (fallback.v||[])[i]||0, type: (fallback.f||[])[i]||'discount', color: (fallback.c||[])[i]||'#313511' }; });
        SEGMENT_COUNT = PRIZES.length;
        ARC = SEGMENT_COUNT > 0 ? (2 * Math.PI) / SEGMENT_COUNT : 0;
        prizesLoaded = true;
        if (callback) callback();
      });
  }

  function init() {
    overlay = document.getElementById('spinWheelOverlay');
    if (!overlay) return;
    isLoggedIn = overlay.getAttribute('data-logged-in') === 'true';
    resultModal = overlay.querySelector('.sw-result-modal');
    wheelCanvas = overlay.querySelector('#wheelCanvas');
    if (!wheelCanvas) return;

    loadPrizes(function () {
      var dpr = window.devicePixelRatio || 1;
      var displaySize = Math.min(360, window.innerWidth - 56);
      wheelCanvas.width = displaySize * dpr;
      wheelCanvas.height = displaySize * dpr;
      wheelCanvas.style.width = displaySize + 'px';
      wheelCanvas.style.height = displaySize + 'px';

      overlay.querySelector('.sw-title').textContent = t.title;
      overlay.querySelector('.sw-subtitle').textContent = t.subtitle;
      overlay.querySelector('.sw-spin-btn').textContent = t.spinBtn;
      overlay.querySelector('.sw-onechance').textContent = t.oneChance;

      if (SEGMENT_COUNT > 0) drawWheel(0);

      var spinBtn = overlay.querySelector('.sw-spin-btn');
      if (spinBtn) spinBtn.addEventListener('click', triggerSpin);
      var closeBtn = overlay.querySelector('.sw-close-btn');
      if (closeBtn) closeBtn.addEventListener('click', closeWheel);
      overlay.addEventListener('click', function (e) { if (e.target === overlay) closeWheel(); });
    });
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else { init(); }

  return { open: openWheel, close: closeWheel, spin: triggerSpin };
})();
