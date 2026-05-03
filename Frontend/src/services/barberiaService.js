import api from './api'

export default {
  async obtenerBarberias(filters = {}) {
    const response = await api.get('/barberias', { params: filters })
    return response.data
  },

  async obtenerBarberia(id) {
    const response = await api.get(`/barberias/${id}`)
    return response.data
  },

  async crearBarberia(datos) {
    const response = await api.post('/barberias', datos)
    return response.data
  },

  async actualizarBarberia(id, datos) {
    const response = await api.put(`/barberias/${id}`, datos)
    return response.data
  },

  async aprobarSolicitud(id, notas = null) {
    const response = await api.post(`/barberias/${id}/aprobar-solicitud`, { notas })
    return response.data
  },

  async rechazarSolicitud(id, motivo) {
    const response = await api.post(`/barberias/${id}/rechazar-solicitud`, { motivo })
    return response.data
  },

  async obtenerBarberosAfiliados(id) {
    const response = await api.get(`/barberias/${id}/barberos`)
    return response.data
  },

  async obtenerServicios(id) {
    const response = await api.get(`/barberias/${id}/servicios`)
    return response.data
  },

  async obtenerEstadisticas(id, periodo = 'mes') {
    const response = await api.get(`/barberias/${id}/estadisticas?periodo=${periodo}`)
    return response.data
  }
}
