import api from './api'

export default {
  async login(email, password) {
    const response = await api.post('/auth/login', { email, password })
    return response.data
  },

  async registro(datos) {
    const response = await api.post('/auth/registro', datos)
    return response.data
  },

  async actualizarPerfil(datos) {
    const response = await api.put('/auth/perfil', datos)
    return response.data
  },

  async cambiarPassword(passwordActual, passwordNuevo) {
    const response = await api.post('/auth/cambiar-password', { 
      passwordActual, 
      passwordNuevo 
    })
    return response.data
  }
}
