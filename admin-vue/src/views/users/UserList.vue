<script setup>
import { ref, watch } from 'vue'
import PageHeader from '../../components/ui/PageHeader.vue'
import DataTable from '../../components/ui/DataTable.vue'
import { api } from '../../api/client'

const items = ref([])
const totalCount = ref(0)
const page = ref(1)
const pageSize = ref(20)
const loading = ref(false)

const roleNames = { 0: 'Admin', 1: 'Muhasebe', 2: 'Operatör', 3: 'Firma' }
const columns = [
  { field: 'email', label: 'E-posta' },
  { field: 'fullName', label: 'Ad Soyad' },
  { field: 'role', label: 'Rol', key: (item) => roleNames[item.role] ?? item.role },
  { field: 'companyName', label: 'Firma' },
  { field: 'isActive', label: 'Aktif', key: (item) => item.isActive ? 'Evet' : 'Hayır' }
]

async function fetchList() {
  loading.value = true
  try {
    const { data } = await api.get('/api/users', {
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
</script>

<template>
  <div>
    <PageHeader
      title="Kullanıcı Listesi"
      subtitle="Sistem kullanıcılarını görüntüleyin (Admin)"
    />
    <DataTable
      :columns="columns"
      :items="items"
      :loading="loading"
      :total-count="totalCount"
      v-model:page="page"
      :page-size="pageSize"
      empty-message="Kullanıcı bulunamadı."
    />
  </div>
</template>
