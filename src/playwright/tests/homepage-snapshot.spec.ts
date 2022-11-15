import { test, expect } from '@playwright/test';

test('homepage snapshot', async ({ page }) => {
  await page.goto('https://localhost:44383/');
  await expect(page).toHaveScreenshot({ fullPage: true});
});
