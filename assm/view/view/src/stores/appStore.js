import { defineStore } from "pinia";
import { APP_CONFIG } from "../config/app.config";

const API = APP_CONFIG.BACKEND_BASE_URL;

export const useAppStore = defineStore("app", {
  state: () => ({
    user: null,
    cartQty: 0
  }),

  getters: {
    isLoggedIn: (state) => state.user !== null
  },

  actions: {
    async loadUser() {
      try {
        const res = await fetch(`${API}/api/AccountApi/me`, {
          credentials: "include",
          cache: "no-store" // 🔥 cực kỳ quan trọng
        });

        if (!res.ok) {
          this.user = null;
          return;
        }

        this.user = await res.json();
        console.log("USER:", this.user); // debug
      } catch (e) {
        this.user = null;
      }
    },

    async loadCartQty() {
      try {
        const res = await fetch(`${API}/api/cart/count`, {
          credentials: "include",
          cache: "no-store"
        });

        const data = await res.json();
        this.cartQty = data.totalQty || 0;
      } catch {
        this.cartQty = 0;
      }
    }
  }
});