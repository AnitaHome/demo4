const { defineConfig, devices } = require('@playwright/test');

module.exports = defineConfig({
  testDir: '.',
  testMatch: 'calculator.spec.js',
  outputDir: 'test-results',
  fullyParallel: false,
  reporter: 'list',
  use: {
    baseURL: 'http://127.0.0.1:5188',
    trace: 'retain-on-failure',
  },
  webServer: {
    command: 'dotnet run --no-build --project src/InBodyCalculator.Api --urls http://127.0.0.1:5188',
    cwd: '../..',
    url: 'http://127.0.0.1:5188',
    reuseExistingServer: true,
    timeout: 120000,
  },
  projects: [
    {
      name: 'desktop',
      use: {
        ...devices['Desktop Chrome'],
        viewport: { width: 1440, height: 900 },
      },
    },
    {
      name: 'mobile',
      use: {
        ...devices['Pixel 7'],
      },
    },
  ],
});