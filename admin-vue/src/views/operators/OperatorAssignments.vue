<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import PageHeader from '../../components/ui/PageHeader.vue'
import DataTable from '../../components/ui/DataTable.vue'
import { api } from '../../api/client'

const router = useRouter()
const loading = ref(false)
const page = ref(1)
const pageSize = ref(20)

const allAssignments = ref([])
const totalCount = computed(() => allAssignments.value.length)
const pagedItems = computed(() => {
  const start = (page.value - 1) * pageSize.value
  return allAssignments.value.slice(start, start + pageSize.value)
})

const columns = [
  { field: 'craneName', label: 'Vinç' },
  { field: 'operatorName', label: 'Atanan Operatör' },
  { field: 'constructionSiteName', label: 'Şantiye' }
]

async function fetchAssignments() {
  loading.value = true
  try {
    const { data } = await api.get('/api/cranes', { params: { pageSize: 500 } })
    const items = data?.items ?? []
    allAssignments.value = items
      .filter(c => c.assignedOperatorId)
      .map(c => ({
        id: c.id,
        craneName: c.name || c.code,
        operatorName: c.assignedOperatorName || '—',
        constructionSiteName: c.constructionSiteName || '—'
      }))
  } catch {
    allAssignments.value = []
  } finally {
    loading.value = false
  }
}

onMounted(fetchAssignments)

function goEditCrane(item) {
  router.push(`/cranes/edit/${item.id}`)
}
</script>

<template>
  <div>
    <PageHeader
      title="Operatör Atamaları"
      subtitle="Vinç–operatör atamaları (vinç düzenlemeden atama yapılır)"
    />
    <DataTable
      :columns="columns"
      :items="pagedItems"
      :loading="loading"
      :total-count="totalCount"
      v-model:page="page"
      :page-size="pageSize"
      empty-message="Atama bulunamadı. Vinç düzenle sayfasından operatör atayabilirsiniz."
    >
      <template #actions="{ item }">
        <button type="button" class="btn-link" @click="goEditCrane(item)">Vinçi Düzenle</button>
      </template>
    </DataTable>
  </div>
</template>

<style scoped>
.btn-link { background: none; border: none; color: #2563eb; cursor: pointer; padding: 4px 8px; font-size: 0.9rem; }
.btn-link:hover { text-decoration: underline; }
</style>
