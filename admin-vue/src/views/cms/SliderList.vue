<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import PageHeader from '../../components/ui/PageHeader.vue'
import DataTable from '../../components/ui/DataTable.vue'
import { api } from '../../api/client'

const router = useRouter()
const items = ref([])
const loading = ref(false)

const columns = [
  { field: 'title', label: 'Başlık' },
  { field: 'shortText', label: 'Kısa Metin' },
  { field: 'sortOrder', label: 'Sıra' },
  { field: 'isActive', label: 'Aktif', key: (item) => item.isActive ? 'Evet' : 'Hayır' }
]

async function fetchList() {
  loading.value = true
  try {
    const { data } = await api.get('/api/sliders', { params: { activeOnly: false } })
    items.value = data || []
  } catch {
    items.value = []
  } finally {
    loading.value = false
  }
}

onMounted(fetchList)

function goNew() {
  router.push('/cms/sliders/new')
}

function goEdit(item) {
  router.push(`/cms/sliders/edit/${item.id}`)
}

async function remove(item) {
  if (!confirm('Bu slider\'ı silmek istediğinize emin misiniz?')) return
  try {
    await api.delete(`/api/sliders/${item.id}`)
    fetchList()
  } catch (e) {
    alert(e.response?.data?.message || 'Silme başarısız.')
  }
}
</script>

<template>
  <div>
    <PageHeader title="Slider Yönetimi" subtitle="Ana sayfa slider öğelerini yönetin">
      <template #actions>
        <button type="button" class="btn btn-primary" @click="goNew">Yeni Slider</button>
      </template>
    </PageHeader>
    <DataTable
      :columns="columns"
      :items="items"
      :loading="loading"
      :total-count="items.length"
      :page-size="items.length || 20"
      :page="1"
      empty-message="Slider kaydı yok."
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
.btn-link { background: none; border: none; color: #2563eb; cursor: pointer; padding: 4px 8px; font-size: 0.9rem; }
.btn-link.danger { color: #dc2626; }
.btn-link + .btn-link { margin-left: 8px; }
</style>
