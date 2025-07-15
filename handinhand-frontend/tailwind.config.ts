import plugin from 'tailwindcss/plugin';
import type { Config } from 'tailwindcss';

const config: Config = {
  darkMode: 'class',
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        'surface-base':'#fdfdfd',        // body 背景（亮）
        'surface-card':'#f4f4f5',        // 卡片（亮）
        'surface-dark':'#1f2937',        // body 背景（暗）
        'surface-card-dark':'#273244',    // 卡片（暗）
        'brand-light': '#FFE8A3',       // 品牌色（亮）
        'brand-dark': '#1C2B2D',         // 品牌色（暗
      },
    },
  },
  plugins: [
    plugin(({ addUtilities, theme }) => {
      addUtilities({
        '.bg-surface-base': {
          backgroundColor: theme('colors.surface-base'),
        },
        '.bg-surface-card': {
          backgroundColor: theme('colors.surface-card'),
        },
        '.bg-surface-dark': {
          backgroundColor: theme('colors.surface-dark'),
        },
        '.bg-surface-card-dark': {
          backgroundColor: theme('colors.surface-card-dark'),
        },
        '.text-brand-light': {
          color: theme('colors.brand-light'),
        },
        '.text-brand-dark': {
          color: theme('colors.brand-dark'),
        },
      });
    }),
  ],
};

export default config;
