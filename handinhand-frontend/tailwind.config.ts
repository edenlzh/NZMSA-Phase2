import type { Config } from 'tailwindcss';

export default {
  darkMode: 'class',                       // 继续支持深色模式切换
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: { extend: {} },
} satisfies Config;