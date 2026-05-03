import { defineStore } from 'pinia'
import servicioService from '@/services/servicioService'

export const useServicioStore = defineStore('servicio', {
  state: () => ({
    servicios: [],
    servicioSeleccionado: null,
    categorias: [],
    cargando: false,
    error: null
  }),

  getters: {
    obtenerServiciosPorBarberia: (state) => (idBarberia) => {
      return state.servicios.filter(s => s.idBarberia === idBarberia)
    },
    
    obtenerServiciosPorCategoria: (state) => (idCategoria) => {
      return state.servicios.filter(s => s.idCategoriaServicio === idCategoria)
    },
    
    serviciosActivos: (state) => {
      return state.servicios.filter(s => s.estado)
    }
  },

  actions: {
    async cargarServicios(filters = {}) {
      this.cargando = true
      this.error = null
      try {
        this.servicios = await servicioService.obtenerServicios(filters)
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar los servicios'
        console.error('Error al cargar servicios:', error)
      } finally {
        this.cargando = false
      }
    },

    async cargarServicio(id) {
      this.cargando = true
      this.error = null
      try {
        this.servicioSeleccionado = await servicioService.obtenerServicio(id)
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar el servicio'
        console.error('Error al cargar servicio:', error)
      } finally {
        this.cargando = false
      }
    },

    async crearServicio(datos) {
      this.cargando = true
      this.error = null
      try {
        const nuevoServicio = await servicioService.crearServicio(datos)
        this.servicios.push(nuevoServicio)
        return nuevoServicio
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al crear el servicio'
        console.error('Error al crear servicio:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async actualizarServicio(id, datos) {
      this.cargando = true
      this.error = null
      try {
        const servicioActualizado = await servicioService.actualizarServicio(id, datos)
        const index = this.servicios.findIndex(s => s.id === id)
        if (index !== -1) {
          this.servicios[index] = servicioActualizado
        }
        if (this.servicioSeleccionado?.id === id) {
          this.servicioSeleccionado = servicioActualizado
        }
        return servicioActualizado
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al actualizar el servicio'
        console.error('Error al actualizar servicio:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async eliminarServicio(id) {
      this.cargando = true
      this.error = null
      try {
        await servicioService.eliminarServicio(id)
        this.servicios = this.servicios.filter(s => s.id !== id)
        if (this.servicioSeleccionado?.id === id) {
          this.servicioSeleccionado = null
        }
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al eliminar el servicio'
        console.error('Error al eliminar servicio:', error)
        throw error
      } finally {
        this.cargando = false
      }
    },

    async cargarCategorias() {
      this.cargando = true
      this.error = null
      try {
        this.categorias = await servicioService.obtenerCategorias()
      } catch (error) {
        this.error = error.response?.data?.message || 'Error al cargar las categorías'
        console.error('Error al cargar categorías:', error)
      } finally {
        this.cargando = false
      }
    },

    limpiarError() {
      this.error = null
    },

    limpiarServicioSeleccionado() {
      this.servicioSeleccionado = null
    }
  }
})
