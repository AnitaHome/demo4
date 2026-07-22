/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./wwwroot/**/*.html', './wwwroot/**/*.js'],
  theme: {
    extend: {
      colors: {
        ink: '#172321',
        canvas: '#f4f7f5',
        pine: '#146b58',
        coral: '#e06b4f',
        mist: '#dce9e4',
      },
      fontFamily: {
        display: ['Georgia', 'Noto Serif TC', 'serif'],
        body: ['Aptos', 'Noto Sans TC', 'sans-serif'],
      },
      boxShadow: {
        panel: '0 18px 55px rgba(23, 35, 33, 0.10)',
      },
    },
  },
  plugins: [],
};