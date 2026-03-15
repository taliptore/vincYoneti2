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

const form = ref({
  craneId: '',
  maintenanceDate: new Date().toISOString().slice(0, 10),
  type: 'Periyodik',
  description: '',
  nextDueDate: ''
})

onMounted(async () => {
  try {
    const { data } = await api.get('/api/cranes', { params: { pageSize: 500 } })
    cranes.value = data?.items ?? []
  } catch (_) {}
  if (isEdit.value) {
    loading.value = true
    try {
      const { data } = await api.get(`/api/maintenance/${route.params.id}`)
      form.value = {
        craneId: data.craneId ?? '',
        maintenanceDate: data.maintenanceDate ? new Date(data.maintenanceDate).toISOString().slice(0, 10) : form.value.maintenanceDate,
        type: data.type ?? 'Periyodik',
        description: data.description ?? '',
        nextDueDate: data.nextDueDate ? new Date(data.nextDueDate).toISOString().slice(0, 10) : ''
      }
    } catch {
      error.value = 'Kayıt yüklenemedi.'
    } finally {
      loading.value = false
    }
  }
})

async function submit() {
  error.value = ''
  if (!form.value.craneId) {
    error.value = 'Vinç seçiniz.'
    return
  }
  submitLoading.value = true
  try {
    const payload = {
      craneId: form.value.craneId,
      maintenanceDate: new Date(form.value.maintenanceDate).toISOString(),
      type: form.value.type || null,
      description: form.value.description || null,
      nextDueDate: form.value.nextDueDate ? new Date(form.value.nextDueDate).toISOString() : null
    }
    if (isEdit.value) {
      await api.put(`/api/maintenance/${route.params.id}`, payload)
      router.push('/maintenance/records')
    } else {
      await api.post('/api/maintenance', payload)
      router.push('/maintenance/records')
    }
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    submitLoading.value = false
  }
}

function cancel() {
  router.push('/maintenance/records')
}
</script>

<template>
  <div>
    <PageHeader
      :title="isEdit ? 'Bakım Kaydı Düzenle' : 'Yeni Bakım Kaydı'"
      :subtitle="isEdit ? 'Kaydı güncelleyin' : 'Yeni bakım kaydı ekleyin'"
    />
    <FormCard title="Bakım Bilgileri" :loading="loading">
      <template v-if="error" #error>{{ error }}</template>
      <form v-else @submit.prevent="submit" class="form-grid">
        <div class="field">
          <label for="craneId">Vinç *</label>
          <select id="craneId" v-model="form.craneId" required>
            <option value="">Seçiniz</option>
            <option v-for="c in cranes" :key="c.id" :value="c.id">{{ c.name || c.code }}</option>
          </select>
        </div>
        <div class="field">
          <label for="maintenanceDate">Bakım Tarihi *</label>
          <input id="maintenanceDate" v-model="form.maintenanceDate" type="date" required />
        </div>
        <div class="field">
          <label for="type">Tip</label>
          <select id="type" v-model="form.type">
            <option value="Periyodik">Periyodik</option>
            <option value="Arıza">Arıza</option>
            <option value="Revizyon">Revizyon</option>
            <option value="Diğer">Diğer</option>
          </select>
        </div>
        <div class="field">
          <label for="nextDueDate">Sonraki Bakım Tarihi</label>
          <input id="nextDueDate" v-model="form.nextDueDate" type="date" />
        </div>
        <div class="field full">
          <label for="description">Açıklama</label>
          <textarea id="description" v-model="form.description" rows="3" maxlength="500"></textarea>
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
.form-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(200px, 1fr)); gap: 16px; }
.field.full, .form-actions.full { grid-column: 1 / -1; }
.field { display: flex; flex-direction: column; gap: 6px; }
.field label { font-size: 0.9rem; font-weight: 500; color: #374151; }
.field input, .field select, .field textarea {
  padding: 8px 12px; font-size: 1rem; border: 1px solid #d1d5db; border-radius: 8px;
}
.form-actions { display: flex; gap: 12px; margin-top: 8px; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:hover:not(:disabled) { background: #1d4ed8; }
.btn-primary:disabled { opacity: 0.7; cursor: not-allowed; }
.btn-secondary { background: #f3f4f6; color: #374151; border: 1px solid #d1d5db; }
.btn-secondary:hover { background: #e5e7eb; }
</style>
