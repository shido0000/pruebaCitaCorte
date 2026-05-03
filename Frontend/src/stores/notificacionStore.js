import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import notificacionService from '../services/notificacionService'

export const useNotificacionStore = defineStore('notificaciones', () => {
  // Estado
  const notificaciones = ref([])
  const noLeidasCount = ref(0)
  const loading = ref(false)
  const error = ref(null)

  // Getters
  const noLeidas = computed(() => 
    notificaciones.value.filter(n => !n.leida)
  )

  // Actions
  async function cargarNotificaciones(page = 1, limit = 20) {
    loading.value = true
    error.value = null
    
    try {
      const response = await notificacionService.obtenerNotificaciones(page, limit)
      notificaciones.value = response.items || response
      noLeidasCount.value = response.noLeidasCount || 0
      return response
    } catch (err) {
      error.value = err.message || 'Error al cargar notificaciones'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function marcarComoLeida(id) {
    try {
      await notificacionService.marcarComoLeida(id)
      const notificacion = notificaciones.value.find(n => n.id === id)
      if (notificacion) {
        notificacion.leida = true
        notificacion.fechaLectura = new Date().toISOString()
      }
      noLeidasCount.value = Math.max(0, noLeidasCount.value - 1)
    } catch (err) {
      error.value = err.message || 'Error al marcar notificación como leída'
      throw err
    }
  }

  async function marcarTodasComoLeidas() {
    try {
      await notificacionService.marcarTodasComoLeidas()
      notificaciones.value.forEach(n => {
        n.leida = true
        n.fechaLectura = new Date().toISOString()
      })
      noLeidasCount.value = 0
    } catch (err) {
      error.value = err.message || 'Error al marcar todas como leídas'
      throw err
    }
  }

  async function eliminarNotificacion(id) {
    try {
      await notificacionService.eliminarNotificacion(id)
      notificaciones.value = notificaciones.value.filter(n => n.id !== id)
      if (!notificaciones.value.find(n => n.id === id)?.leida) {
        noLeidasCount.value = Math.max(0, noLeidasCount.value - 1)
      }
    } catch (err) {
      error.value = err.message || 'Error al eliminar notificación'
      throw err
    }
  }

  return {
    // State
    notificaciones,
    noLeidasCount,
    loading,
    error,
    
    // Getters
    noLeidas,
    
    // Actions
    cargarNotificaciones,
    marcarComoLeida,
    marcarTodasComoLeidas,
    eliminarNotificacion
  }
})
