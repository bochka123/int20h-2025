import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vite.dev/config/
export default defineConfig({
  resolve: {
    alias: {
      "@": "/src"
    },
  },
  plugins: [react()],
  server: {
    port: 3001,
  },
  base: "/"
})
