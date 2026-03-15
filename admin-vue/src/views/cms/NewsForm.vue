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
  title: '',
  summary: '',
  body: '',
  imageUrl: '',
  isPublished: false,
  publishedAt: ''
})

onMounted(async () => {
  if (isEdit.value) {
    loading.value = true
    try {
      const { data } = await api.get(`/api/news/${route.params.id}`)
      form.value = {
        title: data.title ?? '',
        summary: data.summary ?? '',
        body: data.body ?? '',
        imageUrl: data.imageUrl ?? '',
        isPublished: data.isPublished ?? false,
        publishedAt: data.publishedAt ? new Date(data.publishedAt).toISOString().slice(0, 16) : ''
      }
    } catch {
      error.value = 'Haber yüklenemedi.'
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
  submitLoading.value = true
  try {
    const payload = {
      title: form.value.title.trim(),
      summary: form.value.summary || null,
      body: form.value.body || null,
      imageUrl: form.value.imageUrl || null,
      isPublished: !!form.value.isPublished,
      publishedAt: form.value.publishedAt ? new Date(form.value.publishedAt).toISOString() : null
    }
    if (isEdit.value) {
      await api.put(`/api/news/${route.params.id}`, payload)
      router.push('/cms/news')
    } else {
      await api.post('/api/news', payload)
      router.push('/cms/news')
    }
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    submitLoading.value = false
  }
}

function cancel() {
  router.push('/cms/news')
}
</script>

<template>
  <div>
    <PageHeader :title="isEdit ? 'Haber Düzenle' : 'Yeni Haber'" :subtitle="isEdit ? 'Haber bilgilerini güncelleyin' : 'Yeni haber ekleyin'" />
    <FormCard title="Haber Bilgileri" :loading="loading">
      <template v-if="error" #error>{{ error }}</template>
      <form v-else @submit.prevent="submit" class="form-grid">
        <div class="field full">
          <label for="title">Başlık *</label>
          <input id="title" v-model="form.title" type="text" required maxlength="256" />
        </div>
        <div class="field full">
          <label for="summary">Özet</label>
          <input id="summary" v-model="form.summary" type="text" maxlength="500" />
        </div>
        <div class="field full">
          <label for="imageUrl">Görsel URL</label>
          <input id="imageUrl" v-model="form.imageUrl" type="url" maxlength="500" />
        </div>
        <div class="field full">
          <label for="body">İçerik</label>
          <textarea id="body" v-model="form.body" rows="6" maxlength="5000"></textarea>
        </div>
        <div class="field">
          <label for="publishedAt">Yayın Tarihi</label>
          <input id="publishedAt" v-model="form.publishedAt" type="datetime-local" />
        </div>
        <div class="field checkbox">
          <label>
            <input v-model="form.isPublished" type="checkbox" />
            Yayında
          </label>
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
.form-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(200px, 1fr)); gap: 16px; }
.field.full, .form-actions.full { grid-column: 1 / -1; }
.field { display: flex; flex-direction: column; gap: 6px; }
.field label { font-size: 0.9rem; font-weight: 500; color: #374151; }
.field input, .field textarea { padding: 8px 12px; font-size: 1rem; border: 1px solid #d1d5db; border-radius: 8px; }
.field.checkbox { flex-direction: row; align-items: center; }
.field.checkbox label { display: flex; align-items: center; gap: 8px; cursor: pointer; }
.form-actions { display: flex; gap: 12px; margin-top: 8px; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:hover:not(:disabled) { background: #1d4ed8; }
.btn-primary:disabled { opacity: 0.7; cursor: not-allowed; }
.btn-secondary { background: #f3f4f6; color: #374151; border: 1px solid #d1d5db; }
.btn-secondary:hover { background: #e5e7eb; }
</style>
