<script setup>
defineProps({
  message: { type: String, default: 'Kayıt bulunamadı.' },
  icon: { type: String, default: 'inbox' }
})
</script>

<template>
  <div class="empty-state">
    <span class="empty-icon material-icons">{{ icon }}</span>
    <p class="empty-message">{{ message }}</p>
    <slot name="action" />
  </div>
</template>

<style scoped>
.empty-state {
  text-align: center;
  padding: 48px 24px;
  color: #6b7280;
}
.empty-icon {
  font-size: 48px;
  opacity: 0.5;
  display: block;
  margin-bottom: 12px;
}
.empty-message {
  margin: 0 0 16px;
  font-size: 1rem;
}
</style>
