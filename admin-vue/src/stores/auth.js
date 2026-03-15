import { defineStore } from 'pinia'
import { api } from '../api/client'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token'),
    role: localStorage.getItem('role') || null,
    user: null
  }),
  getters: {
    isAuthenticated: (s) => !!s.token,
    roleName: (s) => s.role
  },
  actions: {
    roleFromCode(code) {
      const names = { 0: 'Admin', 1: 'Muhasebe', 2: 'Operatör', 3: 'Firma' }
      return names[code] ?? null
    },
    async login(email, password) {
      const { data } = await api.post('/api/auth/login', { email, password })
      const token = data.token
      const role = this.roleFromCode(data.role) ?? data.role ?? null
      localStorage.setItem('token', token)
      if (role != null) localStorage.setItem('role', String(role))
      this.token = token
      this.role = role
      this.user = { email: data.email, fullName: data.fullName, userId: data.userId }
      return data
    },
    logout() {
      this.token = null
      this.role = null
      this.user = null
      localStorage.removeItem('token')
      localStorage.removeItem('role')
    },
    initFromStorage() {
      this.token = localStorage.getItem('token')
      this.role = localStorage.getItem('role')
    }
  }
})
