import api from './api'

export default {
  async obtenerSolicitudes(filters = {}) {
    const response = await api.get('/solicitudes-afiliacion', { params: filters })
    return response.data
  },

  async obtenerSolicitud(id) {
    const response = await api.get(`/solicitudes-afiliacion/${id}`)
    return response.data
  },

  async crearSolicitud(barberiaId, notas = null) {
    const response = await api.post('/solicitudes-afiliacion', { barberiaId, notas })
    return response.data
  },

  async aprobarSolicitud(id, notas = null) {
    const response = await api.post(`/solicitudes-afiliacion/${id}/aprobar`, { notas })
    return response.data
  },

  async rechazarSolicitud(id, motivo) {
    const response = await api.post(`/solicitudes-afiliacion/${id}/rechazar`, { motivo })
    return response.data
  },

  async retirarSolicitud(id, motivo = null) {
    const response = await api.post(`/solicitudes-afiliacion/${id}/retirar`, { motivo })
    return response.data
  }
}
