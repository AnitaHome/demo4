const { test, expect } = require('@playwright/test');

const enterBmiFields = async page => {
  await page.getByLabel('身高 cm').fill('175.5');
  await page.getByLabel('體重 kg').fill('70');
};

const enterAllFields = async page => {
  await enterBmiFields(page);
  await page.getByLabel('年齡 歲').fill('25');
  await page.getByText('男性', { exact: true }).click();
  await page.getByLabel('活動量').selectOption('moderate');
};

test.beforeEach(async ({ page }) => {
  await page.goto('/');
});

test('buttons follow required field completeness', async ({ page }) => {
  const bmiButton = page.getByRole('button', { name: '計算 BMI' });
  const bmrButton = page.getByRole('button', { name: '計算 BMR' });
  const tdeeButton = page.getByRole('button', { name: '計算 TDEE' });

  await expect(bmiButton).toBeDisabled();
  await expect(bmrButton).toBeDisabled();
  await expect(tdeeButton).toBeDisabled();

  await enterBmiFields(page);
  await expect(bmiButton).toBeEnabled();
  await expect(bmrButton).toBeDisabled();

  await page.getByLabel('年齡 歲').fill('25');
  await page.getByText('男性', { exact: true }).click();
  await expect(bmrButton).toBeEnabled();
  await expect(tdeeButton).toBeDisabled();

  await page.getByLabel('活動量').selectOption('moderate');
  await expect(tdeeButton).toBeEnabled();

  await page.getByLabel('體重 kg').clear();
  await expect(bmiButton).toBeDisabled();
  await expect(bmrButton).toBeDisabled();
  await expect(tdeeButton).toBeDisabled();
});

test('BMI BMR and TDEE calculations render expected values', async ({ page }) => {
  await enterAllFields(page);

  await page.getByRole('button', { name: '計算 BMI' }).click();
  await expect(page.locator('#result')).toContainText('22.73');
  await expect(page.locator('#result')).toContainText('Normal');

  await page.getByRole('button', { name: '計算 BMR' }).click();
  await expect(page.locator('#result')).toContainText('1676.88');

  await page.getByRole('button', { name: '計算 TDEE' }).click();
  await expect(page.locator('#result')).toContainText('2599.16');
});

test('API validation errors are readable', async ({ page }) => {
  await page.route('**/api/calculate/bmi', route => route.fulfill({
    status: 400,
    contentType: 'application/problem+json',
    body: JSON.stringify({
      title: 'One or more validation errors occurred.',
      status: 400,
      errors: { Height: ['Height must be greater than 0.'] },
    }),
  }));
  await enterBmiFields(page);

  await page.getByRole('button', { name: '計算 BMI' }).click();

  await expect(page.getByRole('alert').filter({ hasText: 'Height must be greater than 0.' })).toBeVisible();
});

test('compiled styles load without layout overflow', async ({ page }, testInfo) => {
  const cssResponse = await page.request.get('/css/output.css');
  expect(cssResponse.ok()).toBeTruthy();
  expect((await cssResponse.text()).length).toBeGreaterThan(1000);
  await expect(page.locator('script[src*="tailwindcss.com"]')).toHaveCount(0);

  const hasHorizontalOverflow = await page.evaluate(() => (
    document.documentElement.scrollWidth > document.documentElement.clientWidth
  ));
  expect(hasHorizontalOverflow).toBeFalsy();

  const buttonHeights = await page.locator('.calculate-button').evaluateAll(buttons => (
    buttons.map(button => button.getBoundingClientRect().height)
  ));
  expect(buttonHeights).toEqual([48, 48, 48]);

  await page.screenshot({
    path: testInfo.outputPath(`${testInfo.project.name}-layout.png`),
    fullPage: true,
  });
});