import api from './api'

export default {
  async obtenerServicios(filters = {}) {
    const response = await api.get('/servicios', { params: filters })
    return response.data
  },

  async obtenerServicio(id) {
    const response = await api.get(`/servicios/${id}`)
    return response.data
  },

  async crearServicio(datos) {
    const response = await api.post('/servicios', datos)
    return response.data
  },

  async actualizarServicio(id, datos) {
    const response = await api.put(`/servicios/${id}`, datos)
    return response.data
  },

  async eliminarServicio(id) {
    const response = await api.delete(`/servicios/${id}`)
    return response.data
  },

  async obtenerCategorias() {
    const response = await api.get('/servicios/categorias')
    return response.data
  }
}
