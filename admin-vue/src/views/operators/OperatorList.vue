<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import PageHeader from '../../components/ui/PageHeader.vue'
import DataTable from '../../components/ui/DataTable.vue'
import { api } from '../../api/client'

const router = useRouter()
const allOperators = ref([])
const page = ref(1)
const pageSize = ref(20)
const loading = ref(false)

const OPERATOR_ROLE = 2
const totalCount = computed(() => allOperators.value.length)
const pagedItems = computed(() => {
  const start = (page.value - 1) * pageSize.value
  return allOperators.value.slice(start, start + pageSize.value)
})

const columns = [
  { field: 'email', label: 'E-posta' },
  { field: 'fullName', label: 'Ad Soyad' },
  { field: 'companyName', label: 'Firma' },
  { field: 'isActive', label: 'Aktif', key: (item) => item.isActive ? 'Evet' : 'Hayır' }
]

async function fetchList() {
  loading.value = true
  try {
    const { data } = await api.get('/api/users', { params: { pageSize: 1000 } })
    allOperators.value = (data.items || []).filter(u => u.role === OPERATOR_ROLE)
  } catch {
    allOperators.value = []
  } finally {
    loading.value = false
  }
}

onMounted(fetchList)

function goEdit(item) {
  router.push('/users')
}
</script>

<template>
  <div>
    <PageHeader
      title="Operatör Listesi"
      subtitle="Operatör kullanıcılarını görüntüleyin (Admin)"
    />
    <DataTable
      :columns="columns"
      :items="pagedItems"
      :loading="loading"
      :total-count="totalCount"
      v-model:page="page"
      :page-size="pageSize"
      empty-message="Operatör bulunamadı."
    >
      <template #actions="{ item }">
        <button type="button" class="btn-link" @click="goEdit(item)">Düzenle</button>
      </template>
    </DataTable>
  </div>
</template>

<style scoped>
.btn-link { background: none; border: none; color: #2563eb; cursor: pointer; padding: 4px 8px; font-size: 0.9rem; }
.btn-link:hover { text-decoration: underline; }
.note { margin-top: 12px; color: #6b7280; font-size: 0.9rem; }
</style>
