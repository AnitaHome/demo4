const STORAGE_KEY = 'theme';

const applyTheme = theme => {
  document.documentElement.classList.toggle('dark', theme === 'dark');
};

const getStoredTheme = () => {
  const stored = localStorage.getItem(STORAGE_KEY);
  return stored === 'dark' || stored === 'light' ? stored : null;
};

const getSystemTheme = () =>
  window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';

const updateToggleButton = () => {
  const btn = document.getElementById('theme-toggle');
  if (!btn) return;
  const isDark = document.documentElement.classList.contains('dark');
  btn.setAttribute('aria-label', isDark ? '切換為亮色主題' : '切換為暗色主題');
  btn.querySelector('.icon-sun').classList.toggle('hidden', !isDark);
  btn.querySelector('.icon-moon').classList.toggle('hidden', isDark);
};

export const toggleTheme = () => {
  const isDark = document.documentElement.classList.contains('dark');
  const next = isDark ? 'light' : 'dark';
  applyTheme(next);
  localStorage.setItem(STORAGE_KEY, next);
  updateToggleButton();
};

const btn = document.getElementById('theme-toggle');
if (btn) {
  btn.addEventListener('click', toggleTheme);
}
updateToggleButton();
