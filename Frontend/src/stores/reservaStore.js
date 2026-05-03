import { defineStore } from 'pinia'
import reservaService from '@/services/reservaService'

export const useReservaStore = defineStore('reserva', {
  state: () => ({
    reservas: [],
    reservaSeleccionada: null,
    cargando: false,
    error: null,
    total: 0
  }),

  getters: {
    reservasPendientes: (state) => {
      return state.reservas.filter(r => r.estado === 'Pendiente')
    },
    
    reservasConfirmadas: (state) => {
      return state.reservas.filter(r => r.estado === 'Confirmada')
    },
    
    reservasCanceladas: (state) => {
      return state.reservas.filter(r => r.estado === 'Cancelada')
    },
    
    proximasReservas: (state) => {
      const ahora = new Date()
      return state.reservas
        .filter(r => new Date(r.fechaHora) >= ahora && r.estado !== 'Cancelada')
        .sort((a, b) => new Date(a.fechaHora) - new Date(b.fechaHora))
    }
  },

  actions: {
    async cargarReservas(filtros = {}) {
      this.cargando = true
      this.error = null
      try {
        const response = await reservaService.obtenerReservas(filtros)
        this.reservas = response.data || []
        this.total = response.total || this.reservas.length
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar las reservas'
        console.error('Error al cargar reservas:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cargarReserva(id) {
      this.cargando = true
      this.error = null
      try {
        this.reservaSeleccionada = await reservaService.obtenerReserva(id)
        return this.reservaSeleccionada
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar la reserva'
        console.error('Error al cargar reserva:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async crearReserva(datos) {
      this.cargando = true
      this.error = null
      try {
        const nuevaReserva = await reservaService.crearReserva(datos)
        this.reservas.unshift(nuevaReserva)
        this.total++
        return nuevaReserva
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al crear la reserva'
        console.error('Error al crear reserva:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async actualizarReserva(id, datos) {
      this.cargando = true
      this.error = null
      try {
        const reservaActualizada = await reservaService.actualizarReserva(id, datos)
        const index = this.reservas.findIndex(r => r.id === id)
        if (index !== -1) {
          this.reservas[index] = reservaActualizada
        }
        if (this.reservaSeleccionada?.id === id) {
          this.reservaSeleccionada = reservaActualizada
        }
        return reservaActualizada
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al actualizar la reserva'
        console.error('Error al actualizar reserva:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async confirmarReserva(id) {
      return this.actualizarReserva(id, { estado: 'Confirmada' })
    },

    async cancelarReserva(id, motivo = null) {
      return this.actualizarReserva(id, { 
        estado: 'Cancelada',
        motivoCancelacion: motivo 
      })
    },

    async rechazarReserva(id, motivo) {
      return this.actualizarReserva(id, { 
        estado: 'Rechazada',
        motivoRechazo: motivo 
      })
    },

    async eliminarReserva(id) {
      this.cargando = true
      this.error = null
      try {
        await reservaService.eliminarReserva(id)
        this.reservas = this.reservas.filter(r => r.id !== id)
        this.total--
        if (this.reservaSeleccionada?.id === id) {
          this.reservaSeleccionada = null
        }
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al eliminar la reserva'
        console.error('Error al eliminar reserva:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cargarReservasPorBarbero(barberoId, filtros = {}) {
      this.cargando = true
      this.error = null
      try {
        const response = await reservaService.obtenerReservasPorBarbero(barberoId, filtros)
        this.reservas = response.data || []
        this.total = response.total || this.reservas.length
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar reservas del barbero'
        console.error('Error al cargar reservas:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cargarReservasPorCliente(clienteId, filtros = {}) {
      this.cargando = true
      this.error = null
      try {
        const response = await reservaService.obtenerReservasPorCliente(clienteId, filtros)
        this.reservas = response.data || []
        this.total = response.total || this.reservas.length
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar reservas del cliente'
        console.error('Error al cargar reservas:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    limpiarError() {
      this.error = null
    },

    limpiarReservaSeleccionada() {
      this.reservaSeleccionada = null
    }
  }
})
