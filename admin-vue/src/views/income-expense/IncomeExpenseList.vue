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
const typeFilter = ref('') // '' | '0' (Gelir) | '1' (Gider)

const columns = [
  { field: 'type', label: 'Tür', key: (item) => item.type === 0 ? 'Gelir' : 'Gider' },
  { field: 'category', label: 'Kategori' },
  { field: 'amount', label: 'Tutar', key: (item) => formatAmount(item.amount) },
  { field: 'date', label: 'Tarih', key: (item) => formatDate(item.date) },
  { field: 'description', label: 'Açıklama' },
  { field: 'companyName', label: 'Firma' }
]

function formatDate(d) {
  if (!d) return '—'
  return new Date(d).toLocaleDateString('tr-TR')
}
function formatAmount(n) {
  if (n == null) return '—'
  return new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(n)
}

async function fetchList() {
  loading.value = true
  try {
    const params = { page: page.value, pageSize: pageSize.value }
    if (typeFilter.value !== '') params.type = typeFilter.value
    const { data } = await api.get('/api/incomeexpense', { params })
    items.value = data.items || []
    totalCount.value = data.totalCount ?? 0
  } catch {
    items.value = []
    totalCount.value = 0
  } finally {
    loading.value = false
  }
}

watch([page, typeFilter], fetchList, { immediate: true })

function goNew() {
  router.push('/income-expense/new')
}

function goEdit(item) {
  router.push(`/income-expense/edit/${item.id}`)
}

async function remove(item) {
  if (!confirm('Bu kaydı silmek istediğinize emin misiniz?')) return
  try {
    await api.delete(`/api/incomeexpense/${item.id}`)
    fetchList()
  } catch (e) {
    alert(e.response?.data?.message || 'Silme başarısız.')
  }
}
</script>

<template>
  <div>
    <PageHeader title="Gelir ve Gider" subtitle="Gelir/gider kayıtlarını yönetin">
      <template #actions>
        <button type="button" class="btn btn-primary" @click="goNew">Yeni Kayıt</button>
      </template>
    </PageHeader>
    <div class="toolbar">
      <label>
        Filtre:
        <select v-model="typeFilter" class="select-filter">
          <option value="">Tümü</option>
          <option value="0">Gelir</option>
          <option value="1">Gider</option>
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
      empty-message="Kayıt bulunamadı."
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
.select-filter { padding: 6px 10px; border-radius: 8px; border: 1px solid #d1d5db; margin-left: 8px; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:hover { background: #1d4ed8; }
.btn-link { background: none; border: none; color: #2563eb; cursor: pointer; padding: 4px 8px; font-size: 0.9rem; }
.btn-link:hover { text-decoration: underline; }
.btn-link.danger { color: #dc2626; }
.btn-link + .btn-link { margin-left: 8px; }
</style>
