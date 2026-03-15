<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useMenuStore } from '../stores/menu'

const router = useRouter()
const auth = useAuthStore()
const menuStore = useMenuStore()

const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

async function submit() {
  error.value = ''
  loading.value = true
  try {
    await auth.login(email.value, password.value)
    await menuStore.fetchMenus()
    router.push('/dashboard')
  } catch (e) {
    error.value = e.response?.data?.message || 'Giriş başarısız.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="login-page">
    <div class="login-card">
      <h1>Yönetim Paneli</h1>
      <p class="subtitle">Vinç Yönetim Sistemi</p>
      <form @submit.prevent="submit" class="login-form">
        <div class="field">
          <label for="email">E-posta</label>
          <input id="email" v-model="email" type="email" required autocomplete="email" />
        </div>
        <div class="field">
          <label for="password">Şifre</label>
          <input id="password" v-model="password" type="password" required autocomplete="current-password" />
        </div>
        <p v-if="error" class="error">{{ error }}</p>
        <button type="submit" class="btn-primary" :disabled="loading">
          {{ loading ? 'Giriş yapılıyor...' : 'Giriş' }}
        </button>
      </form>
    </div>
  </div>
</template>

<style scoped>
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #f3f4f6 0%, #e5e7eb 100%);
}
.login-card {
  width: 100%;
  max-width: 380px;
  padding: 40px;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.08);
}
.login-card h1 {
  margin: 0 0 8px;
  font-size: 1.5rem;
  color: #111827;
}
.subtitle {
  margin: 0 0 28px;
  color: #6b7280;
  font-size: 0.95rem;
}
.login-form .field {
  margin-bottom: 18px;
}
.login-form label {
  display: block;
  margin-bottom: 6px;
  font-size: 0.9rem;
  font-weight: 500;
  color: #374151;
}
.login-form input {
  width: 100%;
  padding: 10px 12px;
  font-size: 1rem;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  box-sizing: border-box;
}
.login-form input:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.2);
}
.error {
  margin: 0 0 12px;
  color: #dc2626;
  font-size: 0.9rem;
}
.btn-primary {
  width: 100%;
  padding: 12px;
  font-size: 1rem;
  font-weight: 500;
  color: #fff;
  background: #2563eb;
  border: none;
  border-radius: 8px;
  cursor: pointer;
}
.btn-primary:hover:not(:disabled) {
  background: #1d4ed8;
}
.btn-primary:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}
</style>
