// cypress.config.js  (CommonJS 写法)
const { defineConfig } = require('cypress');

module.exports = defineConfig({
  e2e: {
    baseUrl: 'http://localhost:5173',   // 按需调整
    env: {
      apiUrl: 'http://localhost:5129'   // 按需调整
    },
    video: false,                       // 需要录像时改 true
  },
});
