import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  server: {
    host: '0.0.0.0',
    proxy: {
      '/api': {
        target: 'https://probable-umbrella-gx745rjpjp4w297gw-5274.app.github.dev',
        changeOrigin: true,
        secure: true
      }
    }
  }
})
