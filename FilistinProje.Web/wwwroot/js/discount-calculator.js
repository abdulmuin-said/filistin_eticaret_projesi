(function (window) {
    'use strict';

    function calculatePercentage(oldPrice, effectivePrice) {
        if (!Number.isFinite(oldPrice) || !Number.isFinite(effectivePrice) ||
            oldPrice <= 0 || effectivePrice < 0 || effectivePrice >= oldPrice) {
            return null;
        }

        var percentage = ((oldPrice - effectivePrice) / oldPrice) * 100;

        // Pozitif oranlarda C# MidpointRounding.AwayFromZero ile aynıdır.
        return Math.floor(percentage + 0.5);
    }

    window.caDiscountCalculator = Object.freeze({
        calculatePercentage: calculatePercentage
    });
})(window);
