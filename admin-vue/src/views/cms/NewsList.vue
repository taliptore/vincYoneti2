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
  { field: 'summary', label: 'Özet' },
  { field: 'isPublished', label: 'Yayında', key: (item) => item.isPublished ? 'Evet' : 'Hayır' },
  { field: 'publishedAt', label: 'Yayın Tarihi', key: (item) => item.publishedAt ? new Date(item.publishedAt).toLocaleDateString('tr-TR') : '—' }
]

async function fetchList() {
  loading.value = true
  try {
    const { data } = await api.get('/api/news', { params: { publishedOnly: false, page: page.value, pageSize: pageSize.value } })
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
  router.push('/cms/news/new')
}

function goEdit(item) {
  router.push(`/cms/news/edit/${item.id}`)
}

async function remove(item) {
  if (!confirm('Bu haberi silmek istediğinize emin misiniz?')) return
  try {
    await api.delete(`/api/news/${item.id}`)
    fetchList()
  } catch (e) {
    alert(e.response?.data?.message || 'Silme başarısız.')
  }
}
</script>

<template>
  <div>
    <PageHeader title="Haber Yönetimi" subtitle="Site haberlerini yönetin">
      <template #actions>
        <button type="button" class="btn btn-primary" @click="goNew">Yeni Haber</button>
      </template>
    </PageHeader>
    <DataTable
      :columns="columns"
      :items="items"
      :loading="loading"
      :total-count="totalCount"
      v-model:page="page"
      :page-size="pageSize"
      empty-message="Haber kaydı yok."
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
