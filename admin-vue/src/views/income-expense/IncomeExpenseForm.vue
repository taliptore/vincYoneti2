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
const companies = ref([])

const form = ref({
  type: 0,
  category: '',
  amount: 0,
  date: new Date().toISOString().slice(0, 10),
  description: '',
  referenceType: 0,
  referenceId: null,
  companyId: null
})

onMounted(async () => {
  try {
    const { data } = await api.get('/api/companies', { params: { pageSize: 500 } })
    companies.value = data?.items ?? []
  } catch (_) {}
  if (isEdit.value) {
    loading.value = true
    try {
      const { data } = await api.get(`/api/incomeexpense/${route.params.id}`)
      form.value = {
        type: data.type ?? 0,
        category: data.category ?? '',
        amount: data.amount ?? 0,
        date: data.date ? new Date(data.date).toISOString().slice(0, 10) : form.value.date,
        description: data.description ?? '',
        referenceType: data.referenceType ?? 0,
        referenceId: data.referenceId ?? null,
        companyId: data.companyId ?? null
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
  if (form.value.amount == null || form.value.amount === '') {
    error.value = 'Tutar giriniz.'
    return
  }
  const amount = Number(form.value.amount)
  if (isNaN(amount) || amount < 0) {
    error.value = 'Geçerli bir tutar giriniz.'
    return
  }
  submitLoading.value = true
  try {
    const payload = {
      type: Number(form.value.type),
      category: form.value.category || null,
      amount,
      date: new Date(form.value.date).toISOString(),
      description: form.value.description || null,
      referenceType: Number(form.value.referenceType),
      referenceId: form.value.referenceId || null,
      companyId: form.value.companyId || null
    }
    if (isEdit.value) {
      await api.put(`/api/incomeexpense/${route.params.id}`, payload)
      router.push('/income-expense')
    } else {
      await api.post('/api/incomeexpense', payload)
      router.push('/income-expense')
    }
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    submitLoading.value = false
  }
}

function cancel() {
  router.push('/income-expense')
}
</script>

<template>
  <div>
    <PageHeader
      :title="isEdit ? 'Gelir/Gider Düzenle' : 'Yeni Gelir/Gider'"
      :subtitle="isEdit ? 'Kaydı güncelleyin' : 'Yeni gelir veya gider ekleyin'"
    />
    <FormCard title="Kayıt Bilgileri" :loading="loading">
      <template v-if="error" #error>{{ error }}</template>
      <form v-else @submit.prevent="submit" class="form-grid">
        <div class="field">
          <label for="type">Tür *</label>
          <select id="type" v-model.number="form.type" required>
            <option :value="0">Gelir</option>
            <option :value="1">Gider</option>
          </select>
        </div>
        <div class="field">
          <label for="amount">Tutar *</label>
          <input id="amount" v-model.number="form.amount" type="number" step="0.01" min="0" required />
        </div>
        <div class="field">
          <label for="date">Tarih *</label>
          <input id="date" v-model="form.date" type="date" required />
        </div>
        <div class="field">
          <label for="category">Kategori</label>
          <input id="category" v-model="form.category" type="text" maxlength="128" />
        </div>
        <div class="field full">
          <label for="description">Açıklama</label>
          <input id="description" v-model="form.description" type="text" maxlength="500" />
        </div>
        <div class="field">
          <label for="companyId">Firma</label>
          <select id="companyId" v-model="form.companyId">
            <option :value="null">Seçiniz</option>
            <option v-for="c in companies" :key="c.id" :value="c.id">{{ c.name }}</option>
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
.form-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(200px, 1fr)); gap: 16px; }
.field.full, .form-actions.full { grid-column: 1 / -1; }
.field { display: flex; flex-direction: column; gap: 6px; }
.field label { font-size: 0.9rem; font-weight: 500; color: #374151; }
.field input, .field select {
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
