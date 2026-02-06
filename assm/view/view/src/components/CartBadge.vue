<template>
  <span class="badge rounded-pill text-bg-dark">
    {{ totalQty }}
  </span>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { getCartCount } from "../services/cartApi";

const totalQty = ref(0);

async function refresh() {
  try {
    const res = await getCartCount();
    totalQty.value = res.totalQty ?? 0;
  } catch {}
}

onMounted(refresh);

// expose để nơi khác update
window.refreshCartBadge = refresh;
</script>