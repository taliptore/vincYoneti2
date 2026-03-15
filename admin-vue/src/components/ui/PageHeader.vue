<script setup>
defineProps({
  title: { type: String, required: true },
  subtitle: { type: String, default: '' }
})
</script>

<template>
  <div class="page-header">
    <div>
      <h1 class="page-title">{{ title }}</h1>
      <p v-if="subtitle" class="page-subtitle">{{ subtitle }}</p>
    </div>
    <div class="page-actions">
      <slot name="actions" />
    </div>
  </div>
</template>

<style scoped>
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 24px;
  gap: 16px;
  flex-wrap: wrap;
}
.page-title {
  margin: 0;
  font-size: 1.5rem;
  font-weight: 600;
  color: #111827;
}
.page-subtitle {
  margin: 4px 0 0;
  font-size: 0.95rem;
  color: #6b7280;
}
.page-actions {
  display: flex;
  align-items: center;
  gap: 8px;
}
</style>
