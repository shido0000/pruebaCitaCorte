import api from './api'

export default {
  async obtenerDashboard() {
    const response = await api.get('/dashboard/cliente')
    return response.data
  },

  async obtenerProximasReservas() {
    const response = await api.get('/dashboard/cliente/proximas-reservas')
    return response.data
  },

  async obtenerPromociones() {
    const response = await api.get('/dashboard/cliente/promociones')
    return response.data
  },

  async obtenerBarberosRecomendados() {
    const response = await api.get('/dashboard/cliente/barberos-recomendados')
    return response.data
  },

  async obtenerHistorialReciente() {
    const response = await api.get('/dashboard/cliente/historial-reciente')
    return response.data
  }
}
