<script setup>
import { computed } from 'vue'
import LoadingSpinner from './LoadingSpinner.vue'
import EmptyState from './EmptyState.vue'

const props = defineProps({
  columns: { type: Array, required: true },
  items: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  totalCount: { type: Number, default: 0 },
  page: { type: Number, default: 1 },
  pageSize: { type: Number, default: 20 },
  emptyMessage: { type: String, default: 'Kayıt bulunamadı.' }
})

const emit = defineEmits(['update:page', 'action'])

const totalPages = computed(() =>
  Math.max(1, Math.ceil((props.totalCount || 0) / (props.pageSize || 20)))
)

function getCellValue(item, col) {
  const key = col.key || col.field
  if (typeof key === 'function') return key(item)
  return key ? item[key] : ''
}

function onPageChange(p) {
  emit('update:page', p)
}
</script>

<template>
  <div class="data-table-wrap">
    <LoadingSpinner v-if="loading" size="small" />
    <template v-else>
      <div v-if="items.length === 0" class="table-empty">
        <EmptyState :message="emptyMessage" />
      </div>
      <div v-else class="table-scroll">
        <table class="data-table">
          <thead>
            <tr>
              <th v-for="col in columns" :key="col.field || col.key" :style="{ width: col.width }">
                {{ col.label }}
              </th>
              <th v-if="$slots.actions" class="col-actions">İşlemler</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, idx) in items" :key="item.id || idx">
              <td v-for="col in columns" :key="col.field || col.key">
                <slot v-if="$slots[col.field]" :name="col.field" :item="item" />
                <span v-else>{{ getCellValue(item, col) }}</span>
              </td>
              <td v-if="$slots.actions" class="col-actions">
                <slot name="actions" :item="item" />
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-if="totalCount > pageSize" class="pagination">
        <button
          type="button"
          class="btn-page"
          :disabled="page <= 1"
          @click="onPageChange(page - 1)"
        >
          Önceki
        </button>
        <span class="page-info">
          {{ (page - 1) * pageSize + 1 }}-{{ Math.min(page * pageSize, totalCount) }} / {{ totalCount }}
        </span>
        <button
          type="button"
          class="btn-page"
          :disabled="page >= totalPages"
          @click="onPageChange(page + 1)"
        >
          Sonraki
        </button>
      </div>
    </template>
  </div>
</template>

<style scoped>
.data-table-wrap {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  overflow: hidden;
}
.table-scroll {
  overflow-x: auto;
}
.data-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.9rem;
}
.data-table th,
.data-table td {
  padding: 12px 16px;
  text-align: left;
  border-bottom: 1px solid #e5e7eb;
}
.data-table th {
  background: #f9fafb;
  font-weight: 600;
  color: #374151;
}
.data-table tbody tr:hover {
  background: #f9fafb;
}
.col-actions {
  white-space: nowrap;
}
.table-empty {
  padding: 24px;
}
.pagination {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 12px;
  padding: 12px 16px;
  border-top: 1px solid #e5e7eb;
  background: #f9fafb;
}
.btn-page {
  padding: 6px 12px;
  font-size: 0.9rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  background: #fff;
  cursor: pointer;
}
.btn-page:hover:not(:disabled) {
  background: #f3f4f6;
}
.btn-page:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
.page-info {
  font-size: 0.9rem;
  color: #6b7280;
}
</style>
