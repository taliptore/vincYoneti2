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
  { field: 'title', label: 'Başlık' },
  { field: 'craneName', label: 'Vinç' },
  { field: 'constructionSiteName', label: 'Şantiye' },
  { field: 'plannedStart', label: 'Plan Başlangıç', key: (item) => formatDate(item.plannedStart) },
  { field: 'plannedEnd', label: 'Plan Bitiş', key: (item) => formatDate(item.plannedEnd) },
  { field: 'status', label: 'Durum' },
  { field: 'companyName', label: 'Firma' }
]

function formatDate(d) {
  if (!d) return '—'
  const dt = new Date(d)
  return dt.toLocaleDateString('tr-TR')
}

async function fetchList() {
  loading.value = true
  try {
    const { data } = await api.get('/api/workplans', {
      params: { page: page.value, pageSize: pageSize.value }
    })
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

function goNew() {
  router.push('/work-plans/new')
}

function goEdit(item) {
  router.push(`/work-plans/edit/${item.id}`)
}

async function remove(item) {
  if (!confirm('Bu iş planını silmek istediğinize emin misiniz?')) return
  try {
    await api.delete(`/api/workplans/${item.id}`)
    fetchList()
  } catch (e) {
    alert(e.response?.data?.message || 'Silme başarısız.')
  }
}
</script>

<template>
  <div>
    <PageHeader title="İş Planları" subtitle="Planları görüntüleyin ve yönetin">
      <template #actions>
        <button type="button" class="btn btn-primary" @click="goNew">Yeni Plan</button>
      </template>
    </PageHeader>
    <DataTable
      :columns="columns"
      :items="items"
      :loading="loading"
      :total-count="totalCount"
      v-model:page="page"
      :page-size="pageSize"
      empty-message="Henüz iş planı yok."
    >
      <template #actions="{ item }">
        <button type="button" class="btn-link" @click="goEdit(item)">Düzenle</button>
        <button type="button" class="btn-link danger" @click="remove(item)">Sil</button>
      </template>
    </DataTable>
  </div>
</template>

<style scoped>
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:hover { background: #1d4ed8; }
.btn-link { background: none; border: none; color: #2563eb; cursor: pointer; padding: 4px 8px; font-size: 0.9rem; }
.btn-link:hover { text-decoration: underline; }
.btn-link.danger { color: #dc2626; }
.btn-link + .btn-link { margin-left: 8px; }
</style>
