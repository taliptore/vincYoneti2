<script setup>
import { ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import PageHeader from '../../components/ui/PageHeader.vue'
import DataTable from '../../components/ui/DataTable.vue'
import { api } from '../../api/client'

const router = useRouter()
const items = ref([])
const totalCount = ref(0)
const page = ref(1)
const pageSize = ref(20)
const loading = ref(false)

const columns = [
  { field: 'workPlanTitle', label: 'İş Planı / Proje' },
  { field: 'companyName', label: 'Firma' },
  { field: 'amount', label: 'Tutar', key: (item) => new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(item.amount) },
  { field: 'period', label: 'Dönem' },
  { field: 'status', label: 'Durum' }
]

async function fetchList() {
  loading.value = true
  try {
    const { data } = await api.get('/api/progresspayments', { params: { page: page.value, pageSize: pageSize.value } })
    items.value = data.items || []
    totalCount.value = data.totalCount ?? 0
  } catch {
    items.value = []
    totalCount.value = 0
  } finally {
    loading.value = false
  }
}

watch([page], fetchList, { immediate: true })

function goEdit(item) {
  router.push(`/finance/invoices/edit/${item.id}`)
}
</script>

<template>
  <div>
    <PageHeader title="Teklifler / Hakediş" subtitle="Teklif ve hakediş kayıtları (Fatura sayfasından yeni ekleyebilirsiniz)" />
    <DataTable
      :columns="columns"
      :items="items"
      :loading="loading"
      :total-count="totalCount"
      v-model:page="page"
      :page-size="pageSize"
      empty-message="Kayıt yok."
    >
      <template #actions="{ item }">
        <button type="button" class="btn-link" @click="goEdit(item)">Görüntüle / Düzenle</button>
      </template>
    </DataTable>
  </div>
</template>

<style scoped>
.btn-link { background: none; border: none; color: #2563eb; cursor: pointer; padding: 4px 8px; font-size: 0.9rem; }
.btn-link:hover { text-decoration: underline; }
</style>
