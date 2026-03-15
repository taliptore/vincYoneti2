<script setup>
defineProps({
  size: { type: String, default: 'medium' }
})
</script>

<template>
  <div class="spinner-wrap" :class="size">
    <div class="spinner" />
  </div>
</template>

<style scoped>
.spinner-wrap {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
}
.spinner-wrap.small {
  padding: 12px;
}
.spinner-wrap.large {
  padding: 48px;
}
.spinner {
  width: 40px;
  height: 40px;
  border: 3px solid #e5e7eb;
  border-top-color: #2563eb;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}
.spinner-wrap.small .spinner {
  width: 24px;
  height: 24px;
  border-width: 2px;
}
.spinner-wrap.large .spinner {
  width: 56px;
  height: 56px;
  border-width: 4px;
}
@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>
