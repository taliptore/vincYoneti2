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
const workPlans = ref([])

const form = ref({
  workPlanId: null,
  companyId: '',
  amount: 0,
  period: '',
  status: 'Beklemede'
})

onMounted(async () => {
  try {
    const [companiesRes, plansRes] = await Promise.all([
      api.get('/api/companies', { params: { pageSize: 500 } }),
      api.get('/api/workplans', { params: { pageSize: 500 } })
    ])
    companies.value = companiesRes.data?.items ?? []
    workPlans.value = plansRes.data?.items ?? []
  } catch (_) {}
  if (isEdit.value) {
    loading.value = true
    try {
      const { data } = await api.get(`/api/progresspayments/${route.params.id}`)
      form.value = {
        workPlanId: data.workPlanId ?? null,
        companyId: data.companyId ?? '',
        amount: data.amount ?? 0,
        period: data.period ?? '',
        status: data.status ?? 'Beklemede'
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
  if (!form.value.companyId) {
    error.value = 'Firma seçiniz.'
    return
  }
  const amount = Number(form.value.amount)
  if (isNaN(amount) || amount < 0) {
    error.value = 'Geçerli tutar giriniz.'
    return
  }
  submitLoading.value = true
  try {
    const payload = {
      workPlanId: form.value.workPlanId || null,
      companyId: form.value.companyId,
      amount,
      period: form.value.period || null,
      status: form.value.status || null
    }
    if (isEdit.value) {
      await api.put(`/api/progresspayments/${route.params.id}`, payload)
      router.push('/finance/invoices')
    } else {
      await api.post('/api/progresspayments', payload)
      router.push('/finance/invoices')
    }
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    submitLoading.value = false
  }
}

function cancel() {
  router.push('/finance/invoices')
}
</script>

<template>
  <div>
    <PageHeader :title="isEdit ? 'Hakediş Düzenle' : 'Yeni Hakediş'" :subtitle="isEdit ? 'Hakediş kaydını güncelleyin' : 'Yeni hakediş ekleyin'" />
    <FormCard title="Hakediş Bilgileri" :loading="loading">
      <template v-if="error" #error>{{ error }}</template>
      <form v-else @submit.prevent="submit" class="form-grid">
        <div class="field">
          <label for="companyId">Firma *</label>
          <select id="companyId" v-model="form.companyId" required>
            <option value="">Seçiniz</option>
            <option v-for="c in companies" :key="c.id" :value="c.id">{{ c.name }}</option>
          </select>
        </div>
        <div class="field">
          <label for="workPlanId">İş Planı</label>
          <select id="workPlanId" v-model="form.workPlanId">
            <option :value="null">Seçiniz</option>
            <option v-for="w in workPlans" :key="w.id" :value="w.id">{{ w.title }}</option>
          </select>
        </div>
        <div class="field">
          <label for="amount">Tutar *</label>
          <input id="amount" v-model.number="form.amount" type="number" step="0.01" min="0" required />
        </div>
        <div class="field">
          <label for="period">Dönem</label>
          <input id="period" v-model="form.period" type="text" maxlength="64" placeholder="Örn: 2024-01" />
        </div>
        <div class="field">
          <label for="status">Durum</label>
          <select id="status" v-model="form.status">
            <option value="Beklemede">Beklemede</option>
            <option value="Onaylandı">Onaylandı</option>
            <option value="Ödendi">Ödendi</option>
          </select>
        </div>
        <div class="form-actions full">
          <button type="submit" class="btn btn-primary" :disabled="submitLoading">{{ submitLoading ? 'Kaydediliyor...' : (isEdit ? 'Güncelle' : 'Kaydet') }}</button>
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
.field input, .field select { padding: 8px 12px; font-size: 1rem; border: 1px solid #d1d5db; border-radius: 8px; }
.form-actions { display: flex; gap: 12px; margin-top: 8px; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:disabled { opacity: 0.7; cursor: not-allowed; }
.btn-secondary { background: #f3f4f6; color: #374151; border: 1px solid #d1d5db; }
.btn-secondary:hover { background: #e5e7eb; }
</style>
