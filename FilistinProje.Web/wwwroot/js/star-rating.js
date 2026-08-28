(function () {
    'use strict';

    function getOptions(group) {
        return Array.from(group.querySelectorAll('[role="radio"][data-rating-value]'))
            .sort((a, b) => Number(a.dataset.ratingValue) - Number(b.dataset.ratingValue));
    }

    function render(group, rating) {
        getOptions(group).forEach(option => {
            const value = Number(option.dataset.ratingValue);
            option.dataset.filled = value <= rating ? 'true' : 'false';
        });
    }

    function select(group, rating, focus) {
        const options = getOptions(group);
        const selected = options.find(option => Number(option.dataset.ratingValue) === rating);
        const input = group.querySelector('[data-rating-input]');
        if (!selected || !input) return;

        input.value = String(rating);
        options.forEach(option => {
            const isSelected = option === selected;
            option.setAttribute('aria-checked', isSelected ? 'true' : 'false');
            option.tabIndex = isSelected ? 0 : -1;
        });
        render(group, rating);
        if (focus) selected.focus();
    }

    function reset(group) {
        if (!group) return;
        const fallback = Number(group.dataset.defaultRating) || 1;
        select(group, Math.min(5, Math.max(1, fallback)), false);
    }

    function init(group) {
        if (group.dataset.ratingInitialized === 'true') return;
        group.dataset.ratingInitialized = 'true';

        const options = getOptions(group);
        options.forEach(option => {
            const rating = Number(option.dataset.ratingValue);
            option.addEventListener('click', () => select(group, rating, false));
            option.addEventListener('pointerenter', () => render(group, rating));
            option.addEventListener('keydown', event => {
                let nextRating = rating;
                if (event.key === 'ArrowRight' || event.key === 'ArrowUp') nextRating = Math.min(5, rating + 1);
                else if (event.key === 'ArrowLeft' || event.key === 'ArrowDown') nextRating = Math.max(1, rating - 1);
                else if (event.key === 'Home') nextRating = 1;
                else if (event.key === 'End') nextRating = 5;
                else if (event.key !== ' ' && event.key !== 'Enter') return;

                event.preventDefault();
                select(group, nextRating, true);
            });
        });

        group.addEventListener('pointerleave', () => {
            const input = group.querySelector('[data-rating-input]');
            render(group, Number(input?.value) || 1);
        });

        reset(group);
    }

    function initAll(root) {
        (root || document).querySelectorAll('[data-star-rating]').forEach(init);
    }

    window.starRatings = { initAll, reset, select };
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => initAll(document));
    } else {
        initAll(document);
    }
})();
