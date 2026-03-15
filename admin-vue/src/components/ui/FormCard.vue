<script setup>
import LoadingSpinner from './LoadingSpinner.vue'
defineProps({
  title: { type: String, default: '' },
  loading: { type: Boolean, default: false }
})
</script>

<template>
  <div class="form-card">
    <h2 v-if="title" class="form-card-title">{{ title }}</h2>
    <div v-if="loading" class="form-card-loading">
      <LoadingSpinner size="small" />
    </div>
    <template v-else>
      <p v-if="$slots.error" class="form-error"><slot name="error" /></p>
      <slot />
      <div v-if="$slots.actions" class="form-actions">
        <slot name="actions" />
      </div>
    </template>
  </div>
</template>

<style scoped>
.form-card {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 24px;
  margin-bottom: 24px;
}
.form-card-title {
  margin: 0 0 20px;
  font-size: 1.15rem;
  font-weight: 600;
  color: #111827;
}
.form-card-loading {
  padding: 24px;
  display: flex;
  justify-content: center;
}
.form-error {
  margin: 0 0 16px;
  padding: 12px;
  background: #fef2f2;
  color: #dc2626;
  border-radius: 8px;
  font-size: 0.9rem;
}
.form-actions {
  margin-top: 24px;
  padding-top: 20px;
  border-top: 1px solid #e5e7eb;
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
}
</style>
