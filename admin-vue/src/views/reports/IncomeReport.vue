<script setup>
import { ref, watch } from 'vue'
import PageHeader from '../../components/ui/PageHeader.vue'
import FormCard from '../../components/ui/FormCard.vue'
import DataTable from '../../components/ui/DataTable.vue'
import { api } from '../../api/client'

const loading = ref(false)
const error = ref('')
const report = ref({ totalIncome: 0, totalExpense: 0, balance: 0, items: [] })
const startDate = ref('')
const endDate = ref('')

const columns = [
  { field: 'type', label: 'Tür', key: (item) => item.type === 0 ? 'Gelir' : 'Gider' },
  { field: 'category', label: 'Kategori' },
  { field: 'amount', label: 'Tutar', key: (item) => new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(item.amount) },
  { field: 'date', label: 'Tarih', key: (item) => new Date(item.date).toLocaleDateString('tr-TR') },
  { field: 'description', label: 'Açıklama' }
]

async function fetchReport() {
  loading.value = true
  error.value = ''
  try {
    const params = {}
    if (startDate.value) params.startDate = new Date(startDate.value).toISOString()
    if (endDate.value) params.endDate = new Date(endDate.value).toISOString()
    const { data } = await api.get('/api/reports/income-expense', { params })
    report.value = data || { totalIncome: 0, totalExpense: 0, balance: 0, items: [] }
  } catch {
    error.value = 'Rapor yüklenemedi.'
    report.value = { totalIncome: 0, totalExpense: 0, balance: 0, items: [] }
  } finally {
    loading.value = false
  }
}

watch([startDate, endDate], () => { if (startDate.value || endDate.value) fetchReport() })

function formatMoney(n) {
  return new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(n)
}
</script>

<template>
  <div>
    <PageHeader title="Gelir-Gider Raporu" subtitle="Tarih aralığına göre gelir ve gider özeti" />
    <FormCard title="Filtre">
      <div class="filter-row">
        <label>Başlangıç: <input v-model="startDate" type="date" /></label>
        <label>Bitiş: <input v-model="endDate" type="date" /></label>
        <button type="button" class="btn btn-primary" @click="fetchReport">Raporla</button>
      </div>
    </FormCard>
    <FormCard v-if="error" title="Rapor"><template #error>{{ error }}</template></FormCard>
    <FormCard v-else title="Özet" :loading="loading">
      <div v-if="!loading" class="summary-cards">
        <div class="card">Toplam Gelir: <strong>{{ formatMoney(report.totalIncome) }}</strong></div>
        <div class="card">Toplam Gider: <strong>{{ formatMoney(report.totalExpense) }}</strong></div>
        <div class="card balance">Bakiye: <strong>{{ formatMoney(report.balance) }}</strong></div>
      </div>
    </FormCard>
    <FormCard title="Detay Liste" :loading="loading">
      <DataTable
        :columns="columns"
        :items="report.items"
        :loading="loading"
        :total-count="report.items.length"
        :page-size="report.items.length || 20"
        :page="1"
        empty-message="Kayıt yok."
      />
    </FormCard>
  </div>
</template>

<style scoped>
.filter-row { display: flex; flex-wrap: wrap; align-items: center; gap: 16px; }
.filter-row label { display: flex; align-items: center; gap: 6px; }
.filter-row input { padding: 6px 10px; border: 1px solid #d1d5db; border-radius: 8px; }
.summary-cards { display: flex; flex-wrap: wrap; gap: 16px; margin: 16px 0; }
.card { padding: 16px 20px; background: #f9fafb; border-radius: 12px; border: 1px solid #e5e7eb; }
.card.balance { background: #eff6ff; border-color: #2563eb; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
</style>
