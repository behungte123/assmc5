<template>
  <form class="search-bar" @submit.prevent="submit">
    <input
      v-model="query"
      type="text"
      placeholder="Tìm món ăn..."
    />
    <button type="submit">
      🔍 Tìm
    </button>
  </form>
</template>

<script setup>
import { ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";

const router = useRouter();
const route = useRoute();

const query = ref(route.query.q ?? "");

// nếu user back / forward → sync lại input
watch(
  () => route.query.q,
  (v) => {
    query.value = v ?? "";
  }
);

function submit() {
  router.push({
    path: "/menu",
    query: query.value ? { q: query.value } : {}
  });
}
</script>

<style scoped>
.search-bar {
  display: flex;
  align-items: center;
  gap: 8px;
  background: rgba(255,255,255,.95);
  border: 1px solid rgba(0,0,0,.12);
  border-radius: 999px;
  padding: 8px 14px;
  box-shadow: 0 8px 20px rgba(0,0,0,.10);
}

.search-bar input {
  border: none;
  outline: none;
  background: transparent;
  width: 220px;
  font-size: 13px;
}

.search-bar button {
  border: none;
  background: rgba(0,0,0,.65);
  color: #fff;
  padding: 6px 14px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
}

.search-bar button:hover {
  background: rgba(0,0,0,.85);
}
</style>