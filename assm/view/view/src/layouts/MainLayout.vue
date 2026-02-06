<template>
  <div>
    <div class="frame"></div>

    <div class="page">
      <header class="topbar">
        <!-- TẦNG TRÊN -->
        <div class="top-row">
          <div class="avatar">
            <img :src="`${MVC}/images/nen_home.png`" />
          </div>

          <form class="search-bar" @submit.prevent="search">
            <input v-model="query" placeholder="Tìm món ăn..." />
            <button>🔍 Tìm</button>
          </form>
        </div>

        <!-- MENU -->
        <nav class="menu">
  <!-- HOME = MVC -->
  <a :href="MVC">HOME</a>
  <span class="sep">|</span>

  <!-- MENU = VUE -->
  <RouterLink to="/menu" active-class="active">MENU</RouterLink>
  <span class="sep">|</span>

  <!-- CART = MVC -->
  <a :href="`${MVC}/Cart/Index`">CART</a>
  <span class="badge rounded-pill text-bg-dark">
    {{ store.cartQty }}
  </span>
  <span class="sep">|</span>

  <!-- ORDER HISTORY = MVC -->
  <a :href="`${MVC}/Cart/OrderHistory`">ORDER HISTORY</a>
  <span class="sep">|</span>

  <!-- CONTACT = MVC -->
  <a :href="`${MVC}/Home/Contact`">CONTACT</a>
  <span class="sep">|</span>

  <!-- AUTH -->
  <template v-if="store.isLoggedIn">
    <a :href="urls.profile">
      {{ store.user.userName }}
    </a>
    <span class="sep">|</span>
    <form method="post" :action="urls.logout">
      <button class="btn btn-link p-0">LOGOUT</button>
    </form>
  </template>

  <template v-else>
    <a :href="urls.login">LOGIN</a>
    <span class="sep">|</span>
    <a :href="urls.register">REGISTER</a>
  </template>
</nav>
      </header>

      <router-view />
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import { useAppStore } from "../stores/appStore";
import { APP_CONFIG } from "../config/app.config";

const store = useAppStore();
const query = ref("");

const MVC = APP_CONFIG.BACKEND_BASE_URL;

const urls = computed(() => ({
  menu: `${MVC}${APP_CONFIG.MENU_PATH}`,
  search: `${MVC}${APP_CONFIG.SEARCH_PATH}`,
  login: `${MVC}/Identity/Account/Login`,
  register: `${MVC}/Identity/Account/Register`,
  logout: `${MVC}/Identity/Account/Logout`,
  profile: `${MVC}/Identity/Account/Manage`
}));

onMounted(async () => {
  await store.loadUser();
  await store.loadCartQty();
});

function search() {
  window.location.href =
    `${urls.value.search}?query=${encodeURIComponent(query.value)}`;
}
</script>



<style scoped>
.page {
  min-height: 100vh;
  padding: 26px 0 40px;
  position: relative;
  z-index: 1;
}

/* FRAME */
.frame {
  position: fixed;
  inset: 18px;
  border-radius: 18px;
  pointer-events: none;
  z-index: 0;
  box-shadow:
    inset 0 0 0 1px rgba(0,0,0,.06),
    0 26px 60px rgba(0,0,0,.08);
}

/* TOPBAR */
.topbar {
  width: min(1100px, calc(100% - 64px));
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.top-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.avatar {
  width: 46px;
  height: 46px;
  border-radius: 50%;
  overflow: hidden;
  background: #fff;
  border: 2px solid rgba(255,255,255,.7);
  box-shadow: 0 10px 18px rgba(0,0,0,.12);
}

.avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

/* MOBILE */
@media (max-width: 768px) {
  .top-row {
    flex-direction: column;
    gap: 12px;
  }
}
</style>