<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import PageHeader from '../../components/ui/PageHeader.vue'
import FormCard from '../../components/ui/FormCard.vue'
import { api } from '../../api/client'

const route = useRoute()
const router = useRouter()
const isEdit = computed(() => !!route.params.id)
const loading = ref(false)
const submitLoading = ref(false)
const error = ref('')

const form = ref({
  code: '',
  name: '',
  type: '',
  location: '',
  status: '',
  constructionSiteId: null,
  assignedOperatorId: null
})

const constructionSites = ref([])
const users = ref([])

onMounted(async () => {
  loading.value = true
  try {
    const [sitesRes, usersRes] = await Promise.all([
      api.get('/api/constructionsites', { params: { pageSize: 500 } }),
      api.get('/api/users', { params: { pageSize: 500 } })
    ])
    constructionSites.value = sitesRes.data?.items || []
    users.value = usersRes.data?.items || []
  } finally {
    loading.value = false
  }

  if (isEdit.value) {
    try {
      const { data } = await api.get(`/api/cranes/${route.params.id}`)
      form.value = {
        code: data.code ?? '',
        name: data.name ?? '',
        type: data.type ?? '',
        location: data.location ?? '',
        status: data.status ?? '',
        constructionSiteId: data.constructionSiteId ?? null,
        assignedOperatorId: data.assignedOperatorId ?? null
      }
    } catch (e) {
      error.value = 'Vinç yüklenemedi.'
    }
  }
})

async function submit() {
  error.value = ''
  if (!form.value.code?.trim() || !form.value.name?.trim()) {
    error.value = 'Kod ve Ad zorunludur.'
    return
  }
  submitLoading.value = true
  try {
    const payload = {
      code: form.value.code.trim(),
      name: form.value.name.trim(),
      type: form.value.type || null,
      location: form.value.location || null,
      status: form.value.status || null,
      constructionSiteId: form.value.constructionSiteId || null,
      assignedOperatorId: form.value.assignedOperatorId || null
    }
    if (isEdit.value) {
      await api.put(`/api/cranes/${route.params.id}`, payload)
      router.push('/cranes')
    } else {
      await api.post('/api/cranes', payload)
      router.push('/cranes')
    }
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    submitLoading.value = false
  }
}

function cancel() {
  router.push('/cranes')
}
</script>

<template>
  <div>
    <PageHeader
      :title="isEdit ? 'Vinç Düzenle' : 'Yeni Vinç'"
      :subtitle="isEdit ? 'Vinç bilgilerini güncelleyin' : 'Yeni vinç ekleyin'"
    />
    <FormCard title="Vinç Bilgileri" :loading="loading">
      <template v-if="error" #error>{{ error }}</template>
      <form v-else @submit.prevent="submit" class="form-grid">
        <div class="field">
          <label for="code">Kod *</label>
          <input id="code" v-model="form.code" type="text" required maxlength="50" />
        </div>
        <div class="field">
          <label for="name">Ad *</label>
          <input id="name" v-model="form.name" type="text" required maxlength="256" />
        </div>
        <div class="field">
          <label for="type">Tip</label>
          <input id="type" v-model="form.type" type="text" maxlength="100" />
        </div>
        <div class="field">
          <label for="location">Konum</label>
          <input id="location" v-model="form.location" type="text" maxlength="500" />
        </div>
        <div class="field">
          <label for="status">Durum</label>
          <input id="status" v-model="form.status" type="text" maxlength="50" />
        </div>
        <div class="field">
          <label for="constructionSiteId">Şantiye</label>
          <select id="constructionSiteId" v-model="form.constructionSiteId">
            <option :value="null">Seçiniz</option>
            <option v-for="s in constructionSites" :key="s.id" :value="s.id">{{ s.name }}</option>
          </select>
        </div>
        <div class="field">
          <label for="assignedOperatorId">Atanan Operatör</label>
          <select id="assignedOperatorId" v-model="form.assignedOperatorId">
            <option :value="null">Seçiniz</option>
            <option v-for="u in users" :key="u.id" :value="u.id">{{ u.fullName }} ({{ u.email }})</option>
          </select>
        </div>
        <div class="form-actions">
          <button type="submit" class="btn btn-primary" :disabled="submitLoading">
            {{ submitLoading ? 'Kaydediliyor...' : (isEdit ? 'Güncelle' : 'Kaydet') }}
          </button>
          <button type="button" class="btn btn-secondary" @click="cancel">İptal</button>
        </div>
      </form>
    </FormCard>
  </div>
</template>

<style scoped>
.form-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(260px, 1fr)); gap: 16px; }
.field { display: flex; flex-direction: column; gap: 6px; }
.field label { font-size: 0.9rem; font-weight: 500; color: #374151; }
.field input, .field select {
  padding: 8px 12px; font-size: 1rem; border: 1px solid #d1d5db; border-radius: 8px;
}
.field input:focus, .field select:focus {
  outline: none; border-color: #2563eb; box-shadow: 0 0 0 2px rgba(37, 99, 235, 0.2);
}
.form-actions { grid-column: 1 / -1; margin-top: 8px; display: flex; gap: 12px; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:hover:not(:disabled) { background: #1d4ed8; }
.btn-primary:disabled { opacity: 0.7; cursor: not-allowed; }
.btn-secondary { background: #f3f4f6; color: #374151; border: 1px solid #d1d5db; }
.btn-secondary:hover { background: #e5e7eb; }
</style>
