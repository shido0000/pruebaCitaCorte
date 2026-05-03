import api from './api'

export default {
  async obtenerPlanes(tipo = null) {
    const params = {}
    if (tipo) params.tipo = tipo
    const response = await api.get('/planes', { params })
    return response.data
  },

  async obtenerPlan(id) {
    const response = await api.get(`/planes/${id}`)
    return response.data
  },

  async crearPlan(datos) {
    const response = await api.post('/planes', datos)
    return response.data
  },

  async actualizarPlan(id, datos) {
    const response = await api.put(`/planes/${id}`, datos)
    return response.data
  },

  async eliminarPlan(id) {
    const response = await api.delete(`/planes/${id}`)
    return response.data
  },

  async solicitarCambioPlan(planId) {
    const response = await api.post('/planes/solicitar-cambio', { planId })
    return response.data
  }
}
