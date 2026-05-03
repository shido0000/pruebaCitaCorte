import { defineStore } from 'pinia'
import afiliacionService from '@/services/afiliacionService'

export const useAfiliacionStore = defineStore('afiliacion', {
  state: () => ({
    afiliaciones: [],
    afiliacionSeleccionada: null,
    solicitudes: [],
    cargando: false,
    error: null,
    total: 0
  }),

  getters: {
    solicitudesPendientes: (state) => {
      return state.solicitudes.filter(s => s.estado === 'Pendiente')
    },
    
    solicitudesAprobadas: (state) => {
      return state.solicitudes.filter(s => s.estado === 'Aprobada')
    },
    
    solicitudesRechazadas: (state) => {
      return state.solicitudes.filter(s => s.estado === 'Rechazada')
    },
    
    misAfiliaciones: (state) => {
      return state.afiliaciones.filter(a => a.esMiBarberia)
    }
  },

  actions: {
    async cargarSolicitudes(filtros = {}) {
      this.cargando = true
      this.error = null
      try {
        const response = await afiliacionService.obtenerSolicitudes(filtros)
        this.solicitudes = response.data || []
        this.total = response.total || this.solicitudes.length
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar las solicitudes'
        console.error('Error al cargar solicitudes:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cargarSolicitud(id) {
      this.cargando = true
      this.error = null
      try {
        this.afiliacionSeleccionada = await afiliacionService.obtenerSolicitud(id)
        return this.afiliacionSeleccionada
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar la solicitud'
        console.error('Error al cargar solicitud:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async crearSolicitud(datos) {
      this.cargando = true
      this.error = null
      try {
        const nuevaSolicitud = await afiliacionService.crearSolicitud(datos)
        this.solicitudes.unshift(nuevaSolicitud)
        this.total++
        return nuevaSolicitud
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al crear la solicitud'
        console.error('Error al crear solicitud:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async aprobarSolicitud(id, comentarios = null) {
      this.cargando = true
      this.error = null
      try {
        const response = await afiliacionService.aprobarSolicitud(id, { comentarios })
        const index = this.solicitudes.findIndex(s => s.id === id)
        if (index !== -1) {
          this.solicitudes[index] = response.data || response
        }
        if (this.afiliacionSeleccionada?.id === id) {
          this.afiliacionSeleccionada = response.data || response
        }
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al aprobar la solicitud'
        console.error('Error al aprobar solicitud:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async rechazarSolicitud(id, motivo) {
      this.cargando = true
      this.error = null
      try {
        const response = await afiliacionService.rechazarSolicitud(id, { motivo })
        const index = this.solicitudes.findIndex(s => s.id === id)
        if (index !== -1) {
          this.solicitudes[index] = response.data || response
        }
        if (this.afiliacionSeleccionada?.id === id) {
          this.afiliacionSeleccionada = response.data || response
        }
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al rechazar la solicitud'
        console.error('Error al rechazar solicitud:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cargarAfiliacionesPorBarbero(barberoId) {
      this.cargando = true
      this.error = null
      try {
        const response = await afiliacionService.obtenerAfiliacionesPorBarbero(barberoId)
        this.afiliaciones = response.data || []
        this.total = response.total || this.afiliaciones.length
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar afiliaciones del barbero'
        console.error('Error al cargar afiliaciones:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cargarAfiliacionesPorBarberia(barberiaId) {
      this.cargando = true
      this.error = null
      try {
        const response = await afiliacionService.obtenerAfiliacionesPorBarberia(barberiaId)
        this.afiliaciones = response.data || []
        this.total = response.total || this.afiliaciones.length
        return response
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar afiliaciones de la barbería'
        console.error('Error al cargar afiliaciones:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cancelarSolicitud(id) {
      this.cargando = true
      this.error = null
      try {
        await afiliacionService.cancelarSolicitud(id)
        this.solicitudes = this.solicitudes.filter(s => s.id !== id)
        this.total--
        if (this.afiliacionSeleccionada?.id === id) {
          this.afiliacionSeleccionada = null
        }
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cancelar la solicitud'
        console.error('Error al cancelar solicitud:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    limpiarError() {
      this.error = null
    },

    limpiarAfiliacionSeleccionada() {
      this.afiliacionSeleccionada = null
    }
  }
})
