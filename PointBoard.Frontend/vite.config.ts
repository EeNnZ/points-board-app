import { defineConfig } from 'vite';

export default defineConfig({
  root: '.',
  server: {
    port: 5173,
    proxy: { 
      '/api': {
        target: 'http://localhost:5021',
        changeOrigin: true,
        secure: false
      } 
    }
  },
  build: {
    outDir: 'dist',
    emptyOutDir: true,
    sourcemap: true
  }
});
