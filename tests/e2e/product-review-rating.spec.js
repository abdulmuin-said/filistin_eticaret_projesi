const path = require('path');
const { test, expect } = require('@playwright/test');

const ratingScript = path.resolve(__dirname, '../../FilistinProje.Web/wwwroot/js/star-rating.js');

function ratingMarkup(language, direction) {
  const options = Array.from({ length: 5 }, (_, index) => {
    const rating = index + 1;
    return `<button type="button" class="star-rating-option" role="radio"
      aria-label="${rating} / 5" aria-checked="${rating === 1}"
      tabindex="${rating === 1 ? 0 : -1}" data-rating-value="${rating}">★</button>`;
  }).join('');

  return `<!doctype html><html lang="${language}" dir="${direction}"><body>
    <form action="https://rating.test/products/YorumYap" method="post" data-review-form>
      <div role="radiogroup" aria-label="Rating" data-star-rating data-default-rating="1" dir="ltr">
        <input type="hidden" name="Puan" value="1" data-rating-input>
        ${options}
      </div>
      <button type="submit">Submit</button>
    </form>
  </body></html>`;
}

for (const locale of [
  { language: 'ar', direction: 'rtl' },
  { language: 'en', direction: 'ltr' },
]) {
  test(`${locale.language.toUpperCase()} yıldız puanı görsel, klavye ve post değeriyle eşleşir`, async ({ page }) => {
    await page.setContent(ratingMarkup(locale.language, locale.direction));
    await page.addScriptTag({ path: ratingScript });

    const group = page.getByRole('radiogroup', { name: 'Rating' });
    const options = group.getByRole('radio');
    await expect(group).toHaveAttribute('dir', 'ltr');
    await expect(options).toHaveCount(5);

    for (let rating = 1; rating <= 5; rating += 1) {
      await options.nth(rating - 1).click();
      await expect(group.locator('[data-filled="true"]')).toHaveCount(rating);
      await expect(options.nth(rating - 1)).toHaveAttribute('aria-checked', 'true');
      await expect(group.locator('[name="Puan"]')).toHaveValue(String(rating));
    }

    await options.nth(1).hover();
    await expect(group.locator('[data-filled="true"]')).toHaveCount(2);
    await page.mouse.move(0, 0);
    await expect(group.locator('[data-filled="true"]')).toHaveCount(5);

    await options.nth(2).click();
    await options.nth(2).press('ArrowRight');
    await expect(options.nth(3)).toHaveAttribute('aria-checked', 'true');
    await expect(group.locator('[data-filled="true"]')).toHaveCount(4);
    await options.nth(3).press('Home');
    await expect(options.first()).toHaveAttribute('aria-checked', 'true');

    await page.evaluate(() => window.starRatings.reset(document.querySelector('[data-star-rating]')));
    await expect(group.locator('[data-filled="true"]')).toHaveCount(1);

    await options.nth(3).click();
    const requestPromise = page.waitForRequest(request => request.url().endsWith('/products/YorumYap'));
    await page.getByRole('button', { name: 'Submit' }).click();
    const request = await requestPromise;
    expect(new URLSearchParams(request.postData()).get('Puan')).toBe('4');
  });
}
