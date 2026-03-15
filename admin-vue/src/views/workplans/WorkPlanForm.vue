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
const cranes = ref([])
const constructionSites = ref([])
const companies = ref([])

const form = ref({
  title: '',
  craneId: '',
  constructionSiteId: '',
  plannedStart: '',
  plannedEnd: '',
  status: 'Planlandı',
  companyId: null
})

onMounted(async () => {
  try {
    const [cranesRes, sitesRes, companiesRes] = await Promise.all([
      api.get('/api/cranes', { params: { pageSize: 500 } }),
      api.get('/api/constructionsites', { params: { pageSize: 500 } }),
      api.get('/api/companies', { params: { pageSize: 500 } })
    ])
    cranes.value = cranesRes.data?.items ?? []
    constructionSites.value = sitesRes.data?.items ?? []
    companies.value = companiesRes.data?.items ?? []
  } catch (_) {}
  if (isEdit.value) {
    loading.value = true
    try {
      const { data } = await api.get(`/api/workplans/${route.params.id}`)
      form.value = {
        title: data.title ?? '',
        craneId: data.craneId ?? '',
        constructionSiteId: data.constructionSiteId ?? '',
        plannedStart: data.plannedStart ? new Date(data.plannedStart).toISOString().slice(0, 16) : '',
        plannedEnd: data.plannedEnd ? new Date(data.plannedEnd).toISOString().slice(0, 16) : '',
        status: data.status ?? 'Planlandı',
        companyId: data.companyId || null
      }
    } catch {
      error.value = 'Plan yüklenemedi.'
    } finally {
      loading.value = false
    }
  }
})

async function submit() {
  error.value = ''
  if (!form.value.title?.trim()) {
    error.value = 'Başlık zorunludur.'
    return
  }
  if (!form.value.craneId || !form.value.constructionSiteId) {
    error.value = 'Vinç ve şantiye seçiniz.'
    return
  }
  if (!form.value.plannedStart || !form.value.plannedEnd) {
    error.value = 'Plan başlangıç ve bitiş tarihi giriniz.'
    return
  }
  submitLoading.value = true
  try {
    const payload = {
      title: form.value.title.trim(),
      craneId: form.value.craneId,
      constructionSiteId: form.value.constructionSiteId,
      plannedStart: new Date(form.value.plannedStart).toISOString(),
      plannedEnd: new Date(form.value.plannedEnd).toISOString(),
      status: form.value.status || null,
      companyId: form.value.companyId || null
    }
    if (isEdit.value) {
      await api.put(`/api/workplans/${route.params.id}`, payload)
      router.push('/work-plans')
    } else {
      await api.post('/api/workplans', payload)
      router.push('/work-plans')
    }
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    submitLoading.value = false
  }
}

function cancel() {
  router.push('/work-plans')
}
</script>

<template>
  <div>
    <PageHeader
      :title="isEdit ? 'İş Planı Düzenle' : 'Yeni İş Planı'"
      :subtitle="isEdit ? 'Plan bilgilerini güncelleyin' : 'Yeni iş planı ekleyin'"
    />
    <FormCard title="Plan Bilgileri" :loading="loading">
      <template v-if="error" #error>{{ error }}</template>
      <form v-else @submit.prevent="submit" class="form-grid">
        <div class="field full">
          <label for="title">Başlık *</label>
          <input id="title" v-model="form.title" type="text" required maxlength="256" />
        </div>
        <div class="field">
          <label for="craneId">Vinç *</label>
          <select id="craneId" v-model="form.craneId" required>
            <option value="">Seçiniz</option>
            <option v-for="c in cranes" :key="c.id" :value="c.id">{{ c.name || c.code }}</option>
          </select>
        </div>
        <div class="field">
          <label for="constructionSiteId">Şantiye *</label>
          <select id="constructionSiteId" v-model="form.constructionSiteId" required>
            <option value="">Seçiniz</option>
            <option v-for="s in constructionSites" :key="s.id" :value="s.id">{{ s.name }}</option>
          </select>
        </div>
        <div class="field">
          <label for="plannedStart">Plan Başlangıç *</label>
          <input id="plannedStart" v-model="form.plannedStart" type="datetime-local" required />
        </div>
        <div class="field">
          <label for="plannedEnd">Plan Bitiş *</label>
          <input id="plannedEnd" v-model="form.plannedEnd" type="datetime-local" required />
        </div>
        <div class="field">
          <label for="status">Durum</label>
          <select id="status" v-model="form.status">
            <option value="Planlandı">Planlandı</option>
            <option value="Devam Ediyor">Devam Ediyor</option>
            <option value="Tamamlandı">Tamamlandı</option>
            <option value="İptal">İptal</option>
          </select>
        </div>
        <div class="field">
          <label for="companyId">Firma</label>
          <select id="companyId" v-model="form.companyId">
            <option :value="null">Seçiniz</option>
            <option v-for="co in companies" :key="co.id" :value="co.id">{{ co.name }}</option>
          </select>
        </div>
        <div class="form-actions full">
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
.form-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(220px, 1fr)); gap: 16px; }
.field.full, .form-actions.full { grid-column: 1 / -1; }
.field { display: flex; flex-direction: column; gap: 6px; }
.field label { font-size: 0.9rem; font-weight: 500; color: #374151; }
.field input, .field select {
  padding: 8px 12px; font-size: 1rem; border: 1px solid #d1d5db; border-radius: 8px;
}
.field input:focus, .field select:focus {
  outline: none; border-color: #2563eb; box-shadow: 0 0 0 2px rgba(37, 99, 235, 0.2);
}
.form-actions { display: flex; gap: 12px; margin-top: 8px; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:hover:not(:disabled) { background: #1d4ed8; }
.btn-primary:disabled { opacity: 0.7; cursor: not-allowed; }
.btn-secondary { background: #f3f4f6; color: #374151; border: 1px solid #d1d5db; }
.btn-secondary:hover { background: #e5e7eb; }
</style>
