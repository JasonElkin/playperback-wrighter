import { test, expect } from '@playwright/test';

test('footer snapshot', async ({ page }) => {
  await page.goto('https://localhost:44383/');
  await expect(page.locator('footer')).toHaveScreenshot();
});
