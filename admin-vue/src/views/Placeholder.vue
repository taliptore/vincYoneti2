<script setup>
import { useRoute } from 'vue-router'
const route = useRoute()
</script>

<template>
  <div class="placeholder">
    <h1>{{ route.name || route.path }}</h1>
    <p>Bu sayfa henüz implemente edilmedi.</p>
  </div>
</template>

<style scoped>
.placeholder {
  padding: 20px 0;
}
.placeholder h1 {
  margin: 0 0 8px;
  font-size: 1.25rem;
  color: #374151;
}
.placeholder p {
  margin: 0;
  color: #6b7280;
}
</style>
