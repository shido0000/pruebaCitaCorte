import api from './api'

export default {
  async obtenerDashboard() {
    const response = await api.get('/dashboard/barbero')
    return response.data
  },

  async obtenerEstadisticasReservas(periodo = 'mes') {
    const response = await api.get(`/dashboard/barbero/reservas?periodo=${periodo}`)
    return response.data
  },

  async obtenerIngresos(periodo = 'mes') {
    const response = await api.get(`/dashboard/barbero/ingresos?periodo=${periodo}`)
    return response.data
  },

  async obtenerClientesFrecuentes() {
    const response = await api.get('/dashboard/barbero/clientes-frecuentes')
    return response.data
  },

  async obtenerServiciosPopulares() {
    const response = await api.get('/dashboard/barbero/servicios-populares')
    return response.data
  }
}
