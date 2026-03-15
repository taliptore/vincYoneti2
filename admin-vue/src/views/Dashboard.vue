<script setup>
import { ref, onMounted } from 'vue'
import { useAuthStore } from '../stores/auth'
import { api } from '../api/client'

const auth = useAuthStore()
const summary = ref(null)
const loading = ref(true)
const error = ref('')

onMounted(async () => {
  try {
    const { data } = await api.get('/api/dashboard/summary')
    summary.value = data
  } catch (e) {
    error.value = e.response?.data?.message || 'Özet yüklenemedi.'
  } finally {
    loading.value = false
  }
})
</script>

<template>
  <div class="dashboard">
    <h1 class="dashboard-title">Dashboard</h1>
    <p class="dashboard-welcome">Hoş geldiniz, {{ auth.user?.fullName || auth.user?.email }} ({{ auth.role }})</p>

    <div v-if="loading" class="dashboard-loading">
      <div class="spinner" />
    </div>
    <p v-else-if="error" class="dashboard-error">{{ error }}</p>
    <div v-else-if="summary" class="dashboard-cards">
      <div class="card">
        <span class="card-value">{{ summary.totalCranes }}</span>
        <span class="card-label">Toplam Vinç</span>
      </div>
      <div class="card">
        <span class="card-value">{{ summary.activeCranes }}</span>
        <span class="card-label">Aktif Vinç</span>
      </div>
      <div class="card">
        <span class="card-value">{{ summary.totalCompanies }}</span>
        <span class="card-label">Firma</span>
      </div>
      <div class="card">
        <span class="card-value">{{ summary.totalConstructionSites }}</span>
        <span class="card-label">Şantiye</span>
      </div>
      <div class="card">
        <span class="card-value">{{ summary.totalOperators }}</span>
        <span class="card-label">Operatör</span>
      </div>
      <div class="card">
        <span class="card-value">{{ summary.pendingAppointments }}</span>
        <span class="card-label">Bekleyen Randevu</span>
      </div>
      <div class="card">
        <span class="card-value">{{ summary.unreadContactMessages }}</span>
        <span class="card-label">Okunmamış İletişim</span>
      </div>
      <div class="card highlight">
        <span class="card-value">₺{{ summary.totalIncomeThisMonth?.toLocaleString('tr-TR') || 0 }}</span>
        <span class="card-label">Bu Ay Gelir</span>
      </div>
      <div class="card highlight">
        <span class="card-value">₺{{ summary.totalExpenseThisMonth?.toLocaleString('tr-TR') || 0 }}</span>
        <span class="card-label">Bu Ay Gider</span>
      </div>
    </div>
  </div>
</template>

<style scoped>
.dashboard-title {
  margin: 0 0 4px;
  font-size: 1.5rem;
  font-weight: 600;
  color: #111827;
}
.dashboard-welcome {
  margin: 0 0 24px;
  color: #6b7280;
  font-size: 0.95rem;
}
.dashboard-loading {
  padding: 48px;
  display: flex;
  justify-content: center;
}
.spinner {
  width: 40px;
  height: 40px;
  border: 3px solid #e5e7eb;
  border-top-color: #2563eb;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}
@keyframes spin {
  to { transform: rotate(360deg); }
}
.dashboard-error {
  color: #dc2626;
  padding: 16px;
  background: #fef2f2;
  border-radius: 8px;
}
.dashboard-cards {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
  gap: 16px;
}
.card {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 20px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}
.card.highlight {
  border-color: #dbeafe;
  background: #f8fafc;
}
.card-value {
  font-size: 1.5rem;
  font-weight: 700;
  color: #111827;
}
.card-label {
  font-size: 0.85rem;
  color: #6b7280;
}
</style>
