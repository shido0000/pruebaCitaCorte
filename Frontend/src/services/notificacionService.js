import api from './api'

export default {
  async obtenerNotificaciones(page = 1, limit = 20) {
    const response = await api.get(`/notificaciones?page=${page}&limit=${limit}`)
    return response.data
  },

  async marcarComoLeida(id) {
    const response = await api.put(`/notificaciones/${id}/leida`)
    return response.data
  },

  async marcarTodasComoLeidas() {
    const response = await api.put('/notificaciones/leer-todas')
    return response.data
  },

  async eliminarNotificacion(id) {
    const response = await api.delete(`/notificaciones/${id}`)
    return response.data
  }
}
