<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import PageHeader from '../../components/ui/PageHeader.vue'
import FormCard from '../../components/ui/FormCard.vue'
import { api } from '../../api/client'

const route = useRoute()
const loading = ref(false)
const submitLoading = ref(false)
const error = ref('')
const success = ref('')

const config = computed(() => {
  const name = route.name?.toString() || ''
  if (name === 'SettingsGeneral') return { title: 'Genel Ayarlar', keys: ['SiteAdi', 'LogoUrl', 'SiteAciklama'] }
  if (name === 'SettingsEmail') return { title: 'E-posta Ayarları', keys: ['SmtpHost', 'SmtpPort', 'SmtpUser', 'SmtpPassword', 'FromAddress', 'FromName'] }
  if (name === 'SettingsSms') return { title: 'SMS Ayarları', keys: ['SmsProvider', 'SmsApiKey', 'SmsSender'] }
  if (name === 'SettingsApi') return { title: 'API Ayarları', keys: ['ApiBaseUrl', 'ApiKey'] }
  return { title: 'Ayarlar', keys: [] }
})

const form = ref({})

onMounted(async () => {
  if (config.value.keys.length === 0) return
  loading.value = true
  try {
    const values = {}
    for (const key of config.value.keys) {
      try {
        const { data } = await api.get(`/api/systemsettings/key/${encodeURIComponent(key)}`)
        values[key] = data?.value ?? ''
      } catch {
        values[key] = ''
      }
    }
    form.value = values
  } finally {
    loading.value = false
  }
})

async function submit() {
  error.value = ''
  success.value = ''
  submitLoading.value = true
  try {
    for (const key of config.value.keys) {
      const value = form.value[key] ?? ''
      await api.post('/api/systemsettings', { key, value })
    }
    success.value = 'Ayarlar kaydedildi.'
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    submitLoading.value = false
  }
}
</script>

<template>
  <div>
    <PageHeader :title="config.title" subtitle="Sistem ayarlarını düzenleyin (Admin)" />
    <FormCard :title="config.title" :loading="loading">
      <template v-if="config.keys.length === 0" #error>Bu sayfa için tanımlı ayar yok.</template>
      <template v-else>
        <template v-if="error" #error>{{ error }}</template>
        <template v-else-if="success" #error><span class="success">{{ success }}</span></template>
        <form v-if="Object.keys(form).length" @submit.prevent="submit" class="settings-form">
          <div v-for="key in config.keys" :key="key" class="field">
            <label :for="key">{{ key }}</label>
            <input :id="key" v-model="form[key]" type="text" :name="key" />
          </div>
          <div class="form-actions">
            <button type="submit" class="btn btn-primary" :disabled="submitLoading">
              {{ submitLoading ? 'Kaydediliyor...' : 'Kaydet' }}
            </button>
          </div>
        </form>
      </template>
    </FormCard>
  </div>
</template>

<style scoped>
.settings-form { max-width: 480px; }
.field { margin-bottom: 16px; }
.field label { display: block; font-size: 0.9rem; font-weight: 500; color: #374151; margin-bottom: 4px; }
.field input { width: 100%; padding: 8px 12px; border: 1px solid #d1d5db; border-radius: 8px; }
.form-actions { margin-top: 20px; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.success { color: #059669; }
</style>
