import api from './api'

export default {
  async obtenerDashboard() {
    const response = await api.get('/dashboard/comercial')
    return response.data
  },

  async obtenerMetricasGenerales() {
    const response = await api.get('/dashboard/comercial/metricas')
    return response.data
  },

  async obtenerBarberosPendientesValidacion() {
    const response = await api.get('/dashboard/comercial/barberos-pendientes')
    return response.data
  },

  async obtenerBarberiasPendientesAprobacion() {
    const response = await api.get('/dashboard/comercial/barberias-pendientes')
    return response.data
  },

  async obtenerCrecimiento(periodo = 'mes') {
    const response = await api.get(`/dashboard/comercial/crecimiento?periodo=${periodo}`)
    return response.data
  }
}
