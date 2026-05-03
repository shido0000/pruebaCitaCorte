import api from './api'

export default {
  async obtenerBarberos(filters = {}) {
    const response = await api.get('/barberos', { params: filters })
    return response.data
  },

  async obtenerBarbero(id) {
    const response = await api.get(`/barberos/${id}`)
    return response.data
  },

  async actualizarBarbero(id, datos) {
    const response = await api.put(`/barberos/${id}`, datos)
    return response.data
  },

  async validarDocumentos(id, documentosValidados) {
    const response = await api.post(`/barberos/${id}/validar-documentos`, { documentosValidados })
    return response.data
  },

  async activarBarbero(id) {
    const response = await api.post(`/barberos/${id}/activar`)
    return response.data
  },

  async desactivarBarbero(id, motivo) {
    const response = await api.post(`/barberos/${id}/desactivar`, { motivo })
    return response.data
  },

  async obtenerEstadisticas(id, periodo = 'mes') {
    const response = await api.get(`/barberos/${id}/estadisticas?periodo=${periodo}`)
    return response.data
  }
}
