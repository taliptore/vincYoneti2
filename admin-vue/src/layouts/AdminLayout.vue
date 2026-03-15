<script setup>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useMenuStore } from '../stores/menu'
import SidebarMenu from '../components/SidebarMenu.vue'

const router = useRouter()
const auth = useAuthStore()
const menuStore = useMenuStore()

function logout() {
  auth.logout()
  router.push('/login')
}

onMounted(async () => {
  await menuStore.fetchMenus()
})
</script>

<template>
  <div class="admin-layout">
    <aside class="sidebar">
      <div class="sidebar-header">
        <h1 class="logo">Vinç Yönetimi</h1>
      </div>
      <nav class="sidebar-nav">
        <SidebarMenu :items="menuStore.items" />
      </nav>
    </aside>
    <div class="main">
      <header class="topbar">
        <span class="user">{{ auth.user?.fullName || auth.user?.email || 'Kullanıcı' }}</span>
        <span class="role">{{ auth.role }}</span>
        <button type="button" class="btn-logout" @click="logout">Çıkış</button>
      </header>
      <main class="content">
        <router-view />
      </main>
    </div>
  </div>
</template>

<style scoped>
.admin-layout {
  display: flex;
  min-height: 100vh;
}
.sidebar {
  width: 260px;
  background: #f9fafb;
  border-right: 1px solid #e5e7eb;
  display: flex;
  flex-direction: column;
}
.sidebar-header {
  padding: 20px;
  border-bottom: 1px solid #e5e7eb;
}
.logo {
  margin: 0;
  font-size: 1.2rem;
  font-weight: 600;
  color: #111827;
}
.sidebar-nav {
  flex: 1;
  overflow-y: auto;
  padding: 12px 8px;
}
.main {
  flex: 1;
  display: flex;
  flex-direction: column;
  min-width: 0;
}
.topbar {
  height: 56px;
  padding: 0 24px;
  background: #fff;
  border-bottom: 1px solid #e5e7eb;
  display: flex;
  align-items: center;
  gap: 12px;
}
.user {
  font-weight: 500;
  color: #374151;
}
.role {
  font-size: 0.85rem;
  color: #6b7280;
  padding: 2px 8px;
  background: #f3f4f6;
  border-radius: 6px;
}
.btn-logout {
  margin-left: auto;
  padding: 6px 14px;
  font-size: 0.9rem;
  color: #6b7280;
  background: transparent;
  border: 1px solid #e5e7eb;
  border-radius: 6px;
  cursor: pointer;
}
.btn-logout:hover {
  background: #f3f4f6;
  color: #111827;
}
.content {
  flex: 1;
  padding: 24px;
  overflow-y: auto;
  background: #fff;
}
</style>
