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
  { field: 'code', label: 'Kod', width: '100px' },
  { field: 'name', label: 'Ad' },
  { field: 'type', label: 'Tip' },
  { field: 'location', label: 'Konum' },
  { field: 'status', label: 'Durum', width: '100px' },
  { field: 'constructionSiteName', label: 'Şantiye' },
  { field: 'assignedOperatorName', label: 'Operatör' }
]

async function fetchList() {
  loading.value = true
  try {
    const { data } = await api.get('/api/cranes', {
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
  router.push('/cranes/new')
}

function goEdit(item) {
  router.push(`/cranes/edit/${item.id}`)
}

async function remove(item) {
  if (!confirm('Bu vinci silmek istediğinize emin misiniz?')) return
  try {
    await api.delete(`/api/cranes/${item.id}`)
    fetchList()
  } catch (e) {
    alert(e.response?.data?.message || 'Silme başarısız.')
  }
}
</script>

<template>
  <div>
    <PageHeader title="Vinç Listesi" subtitle="Tüm vinçleri görüntüleyin ve yönetin">
      <template #actions>
        <button type="button" class="btn btn-primary" @click="goNew">Yeni Vinç</button>
      </template>
    </PageHeader>
    <DataTable
      :columns="columns"
      :items="items"
      :loading="loading"
      :total-count="totalCount"
      v-model:page="page"
      :page-size="pageSize"
      empty-message="Henüz vinç kaydı yok."
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
