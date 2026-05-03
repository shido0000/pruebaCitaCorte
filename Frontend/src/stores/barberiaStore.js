import { defineStore } from 'pinia'
import barberiaService from '@/services/barberiaService'

export const useBarberiaStore = defineStore('barberia', {
  state: () => ({
    barberias: [],
    barberiaSeleccionada: null,
    cargando: false,
    error: null,
    total: 0
  }),

  getters: {
    barberiasActivas: (state) => {
      return state.barberias.filter(b => b.estado)
    },
    
    barberiasInactivas: (state) => {
      return state.barberias.filter(b => !b.estado)
    },
    
    barberiasPorUbicacion: (state) => (ubicacion) => {
      return state.barberias.filter(b => b.ubicacion === ubicacion)
    }
  },

  actions: {
    async cargarBarberias(filtros = {}) {
      this.cargando = true
      this.error = null
      try {
        const response = await barberiaService.obtenerBarberias(filtros)
        this.barberias = response.data || []
        this.total = response.total || this.barberias.length
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar las barberías'
        console.error('Error al cargar barberías:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cargarBarberia(id) {
      this.cargando = true
      this.error = null
      try {
        this.barberiaSeleccionada = await barberiaService.obtenerBarberia(id)
        return this.barberiaSeleccionada
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar la barbería'
        console.error('Error al cargar barbería:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async actualizarBarberia(id, datos) {
      this.cargando = true
      this.error = null
      try {
        const barberiaActualizada = await barberiaService.actualizarBarberia(id, datos)
        const index = this.barberias.findIndex(b => b.id === id)
        if (index !== -1) {
          this.barberias[index] = barberiaActualizada
        }
        if (this.barberiaSeleccionada?.id === id) {
          this.barberiaSeleccionada = barberiaActualizada
        }
        return barberiaActualizada
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al actualizar la barbería'
        console.error('Error al actualizar barbería:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async activarBarberia(id) {
      return this.actualizarBarberia(id, { estado: true })
    },

    async desactivarBarberia(id) {
      return this.actualizarBarberia(id, { estado: false })
    },

    async aprobarBarberia(id, comentarios = null) {
      this.cargando = true
      this.error = null
      try {
        const response = await barberiaService.aprobarBarberia(id, { comentarios })
        const index = this.barberias.findIndex(b => b.id === id)
        if (index !== -1) {
          this.barberias[index] = response.data || response
        }
        if (this.barberiaSeleccionada?.id === id) {
          this.barberiaSeleccionada = response.data || response
        }
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al aprobar la barbería'
        console.error('Error al aprobar barbería:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async rechazarBarberia(id, motivo) {
      this.cargando = true
      this.error = null
      try {
        const response = await barberiaService.rechazarBarberia(id, { motivo })
        const index = this.barberias.findIndex(b => b.id === id)
        if (index !== -1) {
          this.barberias[index] = response.data || response
        }
        if (this.barberiaSeleccionada?.id === id) {
          this.barberiaSeleccionada = response.data || response
        }
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al rechazar la barbería'
        console.error('Error al rechazar barbería:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    limpiarError() {
      this.error = null
    },

    limpiarBarberiaSeleccionada() {
      this.barberiaSeleccionada = null
    }
  }
})
