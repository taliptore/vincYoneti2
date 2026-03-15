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
const craneFilter = ref('')
const cranes = ref([])

const columns = [
  { field: 'craneName', label: 'Vinç' },
  { field: 'maintenanceDate', label: 'Bakım Tarihi', key: (item) => formatDate(item.maintenanceDate) },
  { field: 'type', label: 'Tip' },
  { field: 'description', label: 'Açıklama' },
  { field: 'nextDueDate', label: 'Sonraki Bakım', key: (item) => item.nextDueDate ? formatDate(item.nextDueDate) : '—' }
]

function formatDate(d) {
  if (!d) return '—'
  return new Date(d).toLocaleDateString('tr-TR')
}

onMounted(async () => {
  try {
    const { data } = await api.get('/api/cranes', { params: { pageSize: 500 } })
    cranes.value = data?.items ?? []
  } catch (_) {}
})

async function fetchList() {
  loading.value = true
  try {
    const params = { page: page.value, pageSize: pageSize.value }
    if (craneFilter.value) params.craneId = craneFilter.value
    const { data } = await api.get('/api/maintenance', { params })
    items.value = data.items || []
    totalCount.value = data.totalCount ?? 0
  } catch {
    items.value = []
    totalCount.value = 0
  } finally {
    loading.value = false
  }
}

watch([page, craneFilter], fetchList, { immediate: true })

function goNew() {
  router.push('/maintenance/records/new')
}

function goEdit(item) {
  router.push(`/maintenance/records/edit/${item.id}`)
}

async function remove(item) {
  if (!confirm('Bu bakım kaydını silmek istediğinize emin misiniz?')) return
  try {
    await api.delete(`/api/maintenance/${item.id}`)
    fetchList()
  } catch (e) {
    alert(e.response?.data?.message || 'Silme başarısız.')
  }
}
</script>

<template>
  <div>
    <PageHeader title="Bakım Kayıtları" subtitle="Vinç bakım kayıtlarını görüntüleyin ve yönetin">
      <template #actions>
        <button type="button" class="btn btn-primary" @click="goNew">Yeni Bakım Kaydı</button>
      </template>
    </PageHeader>
    <div class="toolbar">
      <label>
        Vinç:
        <select v-model="craneFilter" class="select-filter">
          <option value="">Tümü</option>
          <option v-for="c in cranes" :key="c.id" :value="c.id">{{ c.name || c.code }}</option>
        </select>
      </label>
    </div>
    <DataTable
      :columns="columns"
      :items="items"
      :loading="loading"
      :total-count="totalCount"
      v-model:page="page"
      :page-size="pageSize"
      empty-message="Bakım kaydı bulunamadı."
    >
      <template #actions="{ item }">
        <button type="button" class="btn-link" @click="goEdit(item)">Düzenle</button>
        <button type="button" class="btn-link danger" @click="remove(item)">Sil</button>
      </template>
    </DataTable>
  </div>
</template>

<style scoped>
.toolbar { margin-bottom: 16px; }
.select-filter { padding: 6px 10px; border-radius: 8px; border: 1px solid #d1d5db; margin-left: 8px; min-width: 180px; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:hover { background: #1d4ed8; }
.btn-link { background: none; border: none; color: #2563eb; cursor: pointer; padding: 4px 8px; font-size: 0.9rem; }
.btn-link:hover { text-decoration: underline; }
.btn-link.danger { color: #dc2626; }
.btn-link + .btn-link { margin-left: 8px; }
</style>
