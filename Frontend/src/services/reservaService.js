import api from './api'

export default {
  async obtenerReservas(filters = {}) {
    const response = await api.get('/reservas', { params: filters })
    return response.data
  },

  async obtenerReserva(id) {
    const response = await api.get(`/reservas/${id}`)
    return response.data
  },

  async crearReserva(datos) {
    const response = await api.post('/reservas', datos)
    return response.data
  },

  async actualizarReserva(id, datos) {
    const response = await api.put(`/reservas/${id}`, datos)
    return response.data
  },

  async cancelarReserva(id, motivo) {
    const response = await api.post(`/reservas/${id}/cancelar`, { motivo })
    return response.data
  },

  async confirmarReserva(id) {
    const response = await api.post(`/reservas/${id}/confirmar`)
    return response.data
  },

  async rechazarReserva(id, motivo) {
    const response = await api.post(`/reservas/${id}/rechazar`, { motivo })
    return response.data
  },

  async completarReserva(id, calificacion = null, comentario = null) {
    const response = await api.post(`/reservas/${id}/completar`, { 
      calificacion, 
      comentario 
    })
    return response.data
  }
}
