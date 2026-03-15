<script setup>
import { useRoute } from 'vue-router'

defineProps({
  items: { type: Array, default: () => [] }
})

const route = useRoute()

function isActive(menuRoute) {
  if (!menuRoute) return false
  const path = route.path
  return path === menuRoute || path.startsWith(menuRoute + '/')
}

function hasChildren(item) {
  return item.children && item.children.length > 0
}
</script>

<template>
  <ul class="sidebar-menu">
    <li v-for="item in items" :key="item.id" class="menu-item">
      <template v-if="hasChildren(item)">
        <div class="menu-group-label" :class="{ active: isActive(item.route) }">
          <span class="icon material-icons" v-if="item.icon">{{ item.icon }}</span>
          <span class="title">{{ item.title }}</span>
        </div>
        <SidebarMenu :items="item.children" class="submenu" />
      </template>
      <template v-else>
        <router-link
          :to="item.route || '#'"
          class="menu-link"
          :class="{ active: isActive(item.route) }"
        >
          <span class="icon material-icons" v-if="item.icon">{{ item.icon }}</span>
          <span class="title">{{ item.title }}</span>
        </router-link>
      </template>
    </li>
  </ul>
</template>

<style scoped>
.sidebar-menu {
  list-style: none;
  margin: 0;
  padding: 0;
}
.menu-item {
  margin-bottom: 2px;
}
.menu-group-label,
.menu-link {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 10px 14px;
  border-radius: 8px;
  text-decoration: none;
  color: #374151;
  font-size: 0.95rem;
  transition: background 0.15s;
}
.menu-link:hover,
.menu-group-label:hover {
  background: #f3f4f6;
}
.menu-link.active,
.menu-group-label.active {
  background: #dbeafe;
  color: #1d4ed8;
  font-weight: 500;
}
.submenu {
  padding-left: 12px;
  margin-top: 2px;
}
.submenu .menu-link,
.submenu .menu-group-label {
  padding: 8px 12px;
  font-size: 0.9rem;
}
.icon {
  font-size: 1.1rem;
  opacity: 0.9;
}
</style>
