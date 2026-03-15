<script setup>
import { useRoute } from 'vue-router'
import EmptyState from '../../components/ui/EmptyState.vue'

defineProps({
  message: { type: String, default: 'Bu modül yakında eklenecek.' },
  title: { type: String, default: '' }
})

const route = useRoute()
const pageTitle = route.meta?.title || route.name || ''
</script>

<template>
  <div class="coming-soon">
    <h1 v-if="title || pageTitle" class="coming-title">{{ title || pageTitle }}</h1>
    <EmptyState :message="message" icon="🚧" />
  </div>
</template>

<style scoped>
.coming-soon { padding: 24px; text-align: center; }
.coming-title { font-size: 1.25rem; color: #374151; margin-bottom: 16px; }
</style>
