import { defineStore } from 'pinia'
import barberoService from '@/services/barberoService'

export const useBarberoStore = defineStore('barbero', {
  state: () => ({
    barberos: [],
    barberoSeleccionado: null,
    cargando: false,
    error: null,
    total: 0
  }),

  getters: {
    barberosActivos: (state) => {
      return state.barberos.filter(b => b.estado)
    },
    
    barberosInactivos: (state) => {
      return state.barberos.filter(b => !b.estado)
    },
    
    barberosPorEspecialidad: (state) => (especialidad) => {
      return state.barberos.filter(b => b.especialidad === especialidad)
    }
  },

  actions: {
    async cargarBarberos(filtros = {}) {
      this.cargando = true
      this.error = null
      try {
        const response = await barberoService.obtenerBarberos(filtros)
        this.barberos = response.data || []
        this.total = response.total || this.barberos.length
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar los barberos'
        console.error('Error al cargar barberos:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cargarBarbero(id) {
      this.cargando = true
      this.error = null
      try {
        this.barberoSeleccionado = await barberoService.obtenerBarbero(id)
        return this.barberoSeleccionado
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar el barbero'
        console.error('Error al cargar barbero:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async actualizarBarbero(id, datos) {
      this.cargando = true
      this.error = null
      try {
        const barberoActualizado = await barberoService.actualizarBarbero(id, datos)
        const index = this.barberos.findIndex(b => b.id === id)
        if (index !== -1) {
          this.barberos[index] = barberoActualizado
        }
        if (this.barberoSeleccionado?.id === id) {
          this.barberoSeleccionado = barberoActualizado
        }
        return barberoActualizado
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al actualizar el barbero'
        console.error('Error al actualizar barbero:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async activarBarbero(id) {
      return this.actualizarBarbero(id, { estado: true })
    },

    async desactivarBarbero(id) {
      return this.actualizarBarbero(id, { estado: false })
    },

    async validarDocumentos(id, documentosValidados) {
      this.cargando = true
      this.error = null
      try {
        const response = await barberoService.validarDocumentos(id, documentosValidados)
        const index = this.barberos.findIndex(b => b.id === id)
        if (index !== -1) {
          this.barberos[index] = response.data || response
        }
        if (this.barberoSeleccionado?.id === id) {
          this.barberoSeleccionado = response.data || response
        }
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al validar documentos'
        console.error('Error al validar documentos:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    limpiarError() {
      this.error = null
    },

    limpiarBarberoSeleccionado() {
      this.barberoSeleccionado = null
    }
  }
})
