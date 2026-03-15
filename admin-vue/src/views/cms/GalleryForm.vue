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
  imageUrl: '',
  sortOrder: 0,
  isActive: true
})

onMounted(async () => {
  if (isEdit.value) {
    loading.value = true
    try {
      const { data } = await api.get(`/api/gallery/${route.params.id}`)
      form.value = {
        title: data.title ?? '',
        imageUrl: data.imageUrl ?? '',
        sortOrder: data.sortOrder ?? 0,
        isActive: data.isActive ?? true
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
  if (!form.value.title?.trim()) {
    error.value = 'Başlık zorunludur.'
    return
  }
  submitLoading.value = true
  try {
    const payload = {
      title: form.value.title.trim(),
      imageUrl: form.value.imageUrl || null,
      sortOrder: Number(form.value.sortOrder) || 0,
      isActive: !!form.value.isActive
    }
    if (isEdit.value) {
      await api.put(`/api/gallery/${route.params.id}`, payload)
      router.push('/cms/gallery')
    } else {
      await api.post('/api/gallery', payload)
      router.push('/cms/gallery')
    }
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    submitLoading.value = false
  }
}

function cancel() {
  router.push('/cms/gallery')
}
</script>

<template>
  <div>
    <PageHeader :title="isEdit ? 'Galeri Düzenle' : 'Yeni Galeri Öğesi'" :subtitle="isEdit ? 'Galeri öğesini güncelleyin' : 'Yeni galeri öğesi ekleyin'" />
    <FormCard title="Galeri Bilgileri" :loading="loading">
      <template v-if="error" #error>{{ error }}</template>
      <form v-else @submit.prevent="submit" class="form-grid">
        <div class="field full">
          <label for="title">Başlık *</label>
          <input id="title" v-model="form.title" type="text" required maxlength="256" />
        </div>
        <div class="field full">
          <label for="imageUrl">Görsel URL</label>
          <input id="imageUrl" v-model="form.imageUrl" type="url" maxlength="500" />
        </div>
        <div class="field">
          <label for="sortOrder">Sıra No</label>
          <input id="sortOrder" v-model.number="form.sortOrder" type="number" min="0" />
        </div>
        <div class="field checkbox">
          <label>
            <input v-model="form.isActive" type="checkbox" />
            Aktif
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
.field input { padding: 8px 12px; font-size: 1rem; border: 1px solid #d1d5db; border-radius: 8px; }
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
