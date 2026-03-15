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
  name: '',
  taxNumber: '',
  address: '',
  phone: '',
  email: ''
})

onMounted(async () => {
  if (isEdit.value) {
    loading.value = true
    try {
      const { data } = await api.get(`/api/companies/${route.params.id}`)
      form.value = {
        name: data.name ?? '',
        taxNumber: data.taxNumber ?? '',
        address: data.address ?? '',
        phone: data.phone ?? '',
        email: data.email ?? ''
      }
    } catch {
      error.value = 'Firma yüklenemedi.'
    } finally {
      loading.value = false
    }
  }
})

async function submit() {
  error.value = ''
  if (!form.value.name?.trim()) {
    error.value = 'Firma adı zorunludur.'
    return
  }
  submitLoading.value = true
  try {
    const payload = {
      name: form.value.name.trim(),
      taxNumber: form.value.taxNumber || null,
      address: form.value.address || null,
      phone: form.value.phone || null,
      email: form.value.email || null
    }
    if (isEdit.value) {
      await api.put(`/api/companies/${route.params.id}`, payload)
      router.push('/companies')
    } else {
      await api.post('/api/companies', payload)
      router.push('/companies')
    }
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    submitLoading.value = false
  }
}

function cancel() {
  router.push('/companies')
}
</script>

<template>
  <div>
    <PageHeader
      :title="isEdit ? 'Firma Düzenle' : 'Yeni Firma'"
      :subtitle="isEdit ? 'Firma bilgilerini güncelleyin' : 'Yeni müşteri firması ekleyin'"
    />
    <FormCard title="Firma Bilgileri" :loading="loading">
      <template v-if="error" #error>{{ error }}</template>
      <form v-else @submit.prevent="submit" class="form-grid">
        <div class="field">
          <label for="name">Firma Adı *</label>
          <input id="name" v-model="form.name" type="text" required maxlength="256" />
        </div>
        <div class="field">
          <label for="taxNumber">Vergi No</label>
          <input id="taxNumber" v-model="form.taxNumber" type="text" maxlength="50" />
        </div>
        <div class="field">
          <label for="address">Adres</label>
          <input id="address" v-model="form.address" type="text" maxlength="500" />
        </div>
        <div class="field">
          <label for="phone">Telefon</label>
          <input id="phone" v-model="form.phone" type="text" maxlength="50" />
        </div>
        <div class="field">
          <label for="email">E-posta</label>
          <input id="email" v-model="form.email" type="email" maxlength="256" />
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
.field input {
  padding: 8px 12px; font-size: 1rem; border: 1px solid #d1d5db; border-radius: 8px;
}
.field input:focus {
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
