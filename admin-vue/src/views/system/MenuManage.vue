<script setup>
import { ref, onMounted } from 'vue'
import PageHeader from '../../components/ui/PageHeader.vue'
import FormCard from '../../components/ui/FormCard.vue'
import EmptyState from '../../components/ui/EmptyState.vue'
import { api } from '../../api/client'

const tree = ref([])
const loading = ref(false)
const error = ref('')
const showForm = ref(false)
const formLoading = ref(false)
const editingId = ref(null)
const form = ref({
  title: '',
  icon: '',
  route: '',
  parentId: null,
  orderNo: 0,
  moduleName: '',
  isActive: true,
  roleIds: []
})

async function fetchTree() {
  loading.value = true
  error.value = ''
  try {
    const { data } = await api.get('/api/menu')
    tree.value = data || []
  } catch {
    error.value = 'Menüler yüklenemedi.'
    tree.value = []
  } finally {
    loading.value = false
  }
}

onMounted(fetchTree)

function openNew(parentId = null) {
  editingId.value = null
  form.value = {
    title: '',
    icon: '',
    route: '',
    parentId: parentId,
    orderNo: 0,
    moduleName: '',
    isActive: true,
    roleIds: []
  }
  showForm.value = true
}

async function openEdit(item) {
  formLoading.value = true
  showForm.value = true
  editingId.value = item.id
  try {
    const { data } = await api.get(`/api/menu/${item.id}`)
    form.value = {
      title: data.title ?? '',
      icon: data.icon ?? '',
      route: data.route ?? '',
      parentId: data.parentId ?? null,
      orderNo: data.orderNo ?? 0,
      moduleName: data.moduleName ?? '',
      isActive: data.isActive ?? true,
      roleIds: Array.isArray(data.roleIds) ? data.roleIds : []
    }
  } catch {
    error.value = 'Menü bilgisi alınamadı.'
  } finally {
    formLoading.value = false
  }
}

function closeForm() {
  showForm.value = false
  editingId.value = null
  error.value = ''
}

async function submit() {
  error.value = ''
  if (!form.value.title?.trim()) {
    error.value = 'Başlık zorunludur.'
    return
  }
  formLoading.value = true
  try {
    const payload = {
      title: form.value.title.trim(),
      icon: form.value.icon || null,
      route: form.value.route || null,
      parentId: form.value.parentId,
      orderNo: form.value.orderNo,
      moduleName: form.value.moduleName || null,
      isActive: form.value.isActive,
      roleIds: form.value.roleIds || []
    }
    if (editingId.value) {
      await api.put(`/api/menu/${editingId.value}`, payload)
    } else {
      await api.post('/api/menu', payload)
    }
    closeForm()
    fetchTree()
  } catch (e) {
    error.value = e.response?.data?.message || 'Kayıt başarısız.'
  } finally {
    formLoading.value = false
  }
}

async function remove(item) {
  if (!confirm(`"${item.title}" menüsünü silmek istediğinize emin misiniz?`)) return
  try {
    await api.delete(`/api/menu/${item.id}`)
    fetchTree()
    if (editingId.value === item.id) closeForm()
  } catch (e) {
    alert(e.response?.data?.message || 'Silme başarısız.')
  }
}

async function reorder(item, direction) {
  const newOrder = item.orderNo + (direction === 'up' ? -1 : 1)
  if (newOrder < 0) return
  try {
    await api.put(`/api/menu/${item.id}/reorder`, null, { params: { orderNo: newOrder } })
    fetchTree()
  } catch (e) {
    alert(e.response?.data?.message || 'Sıra güncellenemedi.')
  }
}
</script>

<template>
  <div>
    <PageHeader
      title="Menü Yönetimi"
      subtitle="Sidebar menü öğelerini düzenleyin (Admin)"
    >
      <template #actions>
        <button type="button" class="btn btn-primary" @click="openNew()">Yeni Menü</button>
      </template>
    </PageHeader>

    <div class="menu-layout">
      <FormCard title="Menü Ağacı" :loading="loading">
        <template v-if="error && !showForm" #error>{{ error }}</template>
        <div v-else-if="tree.length === 0 && !loading" class="empty-wrap">
          <EmptyState message="Henüz menü kaydı yok. Yeni menü ekleyin." />
        </div>
        <ul v-else class="menu-tree">
          <li v-for="node in tree" :key="node.id" class="menu-node">
            <div class="menu-row">
              <span class="menu-title">{{ node.icon ? `${node.icon} ` : '' }}{{ node.title }}</span>
              <span class="menu-meta">{{ node.route || '—' }} · Sıra: {{ node.orderNo }}</span>
              <span class="menu-badges">
                <span v-if="!node.isActive" class="badge inactive">Pasif</span>
                <button type="button" class="btn-link" @click="openNew(node.id)">Alt Ekle</button>
                <button type="button" class="btn-link" @click="openEdit(node)">Düzenle</button>
                <button type="button" class="btn-link" @click="reorder(node, 'up')">↑</button>
                <button type="button" class="btn-link" @click="reorder(node, 'down')">↓</button>
                <button type="button" class="btn-link danger" @click="remove(node)">Sil</button>
              </span>
            </div>
            <ul v-if="node.children?.length" class="menu-children">
              <li v-for="child in node.children" :key="child.id" class="menu-node">
                <div class="menu-row">
                  <span class="menu-title">{{ child.icon ? `${child.icon} ` : '' }}{{ child.title }}</span>
                  <span class="menu-meta">{{ child.route || '—' }} · {{ child.orderNo }}</span>
                  <span class="menu-badges">
                    <span v-if="!child.isActive" class="badge inactive">Pasif</span>
                    <button type="button" class="btn-link" @click="openEdit(child)">Düzenle</button>
                    <button type="button" class="btn-link" @click="reorder(child, 'up')">↑</button>
                    <button type="button" class="btn-link" @click="reorder(child, 'down')">↓</button>
                    <button type="button" class="btn-link danger" @click="remove(child)">Sil</button>
                  </span>
                </div>
              </li>
            </ul>
          </li>
        </ul>
      </FormCard>

      <FormCard v-if="showForm" :title="editingId ? 'Menü Düzenle' : 'Yeni Menü'" :loading="formLoading">
        <template v-if="error" #error>{{ error }}</template>
        <form v-else @submit.prevent="submit" class="form-grid">
          <div class="field">
            <label for="menu-title">Başlık *</label>
            <input id="menu-title" v-model="form.title" type="text" required maxlength="128" />
          </div>
          <div class="field">
            <label for="menu-icon">İkon</label>
            <input id="menu-icon" v-model="form.icon" type="text" maxlength="64" placeholder="Örn: dashboard" />
          </div>
          <div class="field">
            <label for="menu-route">Route</label>
            <input id="menu-route" v-model="form.route" type="text" maxlength="256" placeholder="/dashboard" />
          </div>
          <div class="field">
            <label for="menu-orderNo">Sıra No</label>
            <input id="menu-orderNo" v-model.number="form.orderNo" type="number" min="0" />
          </div>
          <div class="field">
            <label for="menu-moduleName">Modül Adı</label>
            <input id="menu-moduleName" v-model="form.moduleName" type="text" maxlength="64" />
          </div>
          <div class="field checkbox">
            <label>
              <input v-model="form.isActive" type="checkbox" />
              Aktif
            </label>
          </div>
          <div class="form-actions">
            <button type="submit" class="btn btn-primary" :disabled="formLoading">
              {{ formLoading ? 'Kaydediliyor...' : (editingId ? 'Güncelle' : 'Ekle') }}
            </button>
            <button type="button" class="btn btn-secondary" @click="closeForm">İptal</button>
          </div>
        </form>
      </FormCard>
    </div>
  </div>
</template>

<style scoped>
.menu-layout { display: grid; grid-template-columns: 1fr; gap: 24px; }
@media (min-width: 900px) { .menu-layout { grid-template-columns: 1fr 340px; } }
.menu-tree { list-style: none; padding: 0; margin: 0; }
.menu-node { margin-bottom: 4px; }
.menu-children { list-style: none; padding-left: 24px; margin: 4px 0 0 0; border-left: 2px solid #e5e7eb; }
.menu-row {
  display: flex; align-items: center; flex-wrap: wrap; gap: 8px 16px;
  padding: 10px 12px; background: #f9fafb; border-radius: 8px; font-size: 0.9rem;
}
.menu-title { font-weight: 500; color: #111827; }
.menu-meta { color: #6b7280; font-size: 0.85rem; }
.menu-badges { display: flex; align-items: center; gap: 4px; margin-left: auto; flex-wrap: wrap; }
.badge { font-size: 0.75rem; padding: 2px 6px; border-radius: 4px; }
.badge.inactive { background: #fef2f2; color: #dc2626; }
.btn { padding: 8px 16px; font-size: 0.9rem; border-radius: 8px; cursor: pointer; border: none; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:hover:not(:disabled) { background: #1d4ed8; }
.btn-primary:disabled { opacity: 0.7; cursor: not-allowed; }
.btn-secondary { background: #f3f4f6; color: #374151; border: 1px solid #d1d5db; }
.btn-secondary:hover { background: #e5e7eb; }
.btn-link { background: none; border: none; color: #2563eb; cursor: pointer; padding: 4px 6px; font-size: 0.85rem; }
.btn-link:hover { text-decoration: underline; }
.btn-link.danger { color: #dc2626; }
.empty-wrap { padding: 16px 0; }
.form-grid { display: grid; grid-template-columns: 1fr; gap: 12px; }
.field { display: flex; flex-direction: column; gap: 4px; }
.field label { font-size: 0.9rem; font-weight: 500; color: #374151; }
.field input[type="text"], .field input[type="number"] {
  padding: 8px 12px; font-size: 1rem; border: 1px solid #d1d5db; border-radius: 8px;
}
.field.checkbox { flex-direction: row; align-items: center; }
.field.checkbox label { display: flex; align-items: center; gap: 8px; cursor: pointer; }
.form-actions { display: flex; gap: 12px; margin-top: 8px; }
</style>
