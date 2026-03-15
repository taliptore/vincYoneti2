import { defineStore } from 'pinia'
import { api } from '../api/client'

export const useMenuStore = defineStore('menu', {
  state: () => ({ items: [] }),
  actions: {
    async fetchMenus() {
      const { data } = await api.get('/api/menu/user')
      this.items = Array.isArray(data) ? data : []
      return this.items
    }
  }
})
