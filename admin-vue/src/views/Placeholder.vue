<script setup>
import { useRoute } from 'vue-router'
import EmptyState from '../components/ui/EmptyState.vue'

const route = useRoute()
const title = route.meta?.title || (typeof route.name === 'string' ? route.name : route.name?.toString()) || 'Sayfa'
</script>

<template>
  <div class="placeholder-page">
    <h1 class="placeholder-title">{{ title }}</h1>
    <EmptyState
      message="Bu modül yakında eklenecek."
      icon="🚧"
    />
  </div>
</template>

<style scoped>
.placeholder-page { padding: 24px; text-align: center; }
.placeholder-title { font-size: 1.25rem; color: #374151; margin-bottom: 16px; }
</style>
