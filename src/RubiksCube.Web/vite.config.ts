/// <reference types="vitest/config" />
import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [plugin()],
  server: {
    port: 65142,
  },
  test: {
    environment: "jsdom",
    setupFiles: ["./src/test/setup.ts"],
    globals: true,
    env: { VITE_API_URL: "http://test" },
  },
});
