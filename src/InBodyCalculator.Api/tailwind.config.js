/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./wwwroot/**/*.html', './wwwroot/**/*.js'],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        ink:     'rgb(var(--color-ink) / <alpha-value>)',
        canvas:  'rgb(var(--color-canvas) / <alpha-value>)',
        pine:    'rgb(var(--color-pine) / <alpha-value>)',
        coral:   '#e06b4f',
        mist:    'rgb(var(--color-mist) / <alpha-value>)',
        surface: 'rgb(var(--color-surface) / <alpha-value>)',
      },
      fontFamily: {
        display: ['Georgia', 'Noto Serif TC', 'serif'],
        body: ['Aptos', 'Noto Sans TC', 'sans-serif'],
      },
      boxShadow: {
        panel: '0 18px 55px rgba(60, 26, 8, 0.12)',
      },
      animation: {
        'fade-in':    'fadeIn 0.5s ease-out both',
        'slide-up':   'slideUp 0.6s ease-out both',
        'result-pop': 'resultPop 0.45s cubic-bezier(0.34, 1.56, 0.64, 1) both',
        'spin-once':  'spinOnce 0.45s ease-in-out',
      },
      keyframes: {
        fadeIn: {
          '0%':   { opacity: '0' },
          '100%': { opacity: '1' },
        },
        slideUp: {
          '0%':   { opacity: '0', transform: 'translateY(20px)' },
          '100%': { opacity: '1', transform: 'translateY(0)' },
        },
        resultPop: {
          '0%':   { opacity: '0', transform: 'scale(0.85) translateY(10px)' },
          '100%': { opacity: '1', transform: 'scale(1) translateY(0)' },
        },
        spinOnce: {
          '0%':   { transform: 'rotate(0deg)' },
          '100%': { transform: 'rotate(360deg)' },
        },
      },
    },
  },
  plugins: [],
};