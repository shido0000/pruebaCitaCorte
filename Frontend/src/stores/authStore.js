import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import authService from '../services/authService'

export const useAuthStore = defineStore('auth', () => {
  // Estado
  const user = ref(null)
  const token = ref(localStorage.getItem('token') || null)
  const loading = ref(false)
  const error = ref(null)

  // Getters
  const isAuthenticated = computed(() => !!token.value)
  const userId = computed(() => user.value?.id || null)
  const userRole = computed(() => user.value?.role || null)
  const userName = computed(() => user.value?.nombre || user.value?.email || '')

  // Actions
  async function login(email, password) {
    loading.value = true
    error.value = null
    
    try {
      const response = await authService.login(email, password)
      token.value = response.token
      user.value = response.usuario
      
      localStorage.setItem('token', response.token)
      localStorage.setItem('usuario', JSON.stringify(response.usuario))
      
      return response
    } catch (err) {
      error.value = err.message || 'Error al iniciar sesión'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function registro(datosRegistro) {
    loading.value = true
    error.value = null
    
    try {
      const response = await authService.registro(datosRegistro)
      return response
    } catch (err) {
      error.value = err.message || 'Error al registrar usuario'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('usuario')
  }

  async function cargarUsuario() {
    if (!token.value) return
    
    try {
      const usuarioGuardado = localStorage.getItem('usuario')
      if (usuarioGuardado) {
        user.value = JSON.parse(usuarioGuardado)
      }
    } catch (err) {
      console.error('Error al cargar usuario:', err)
      logout()
    }
  }

  async function actualizarPerfil(datosActualizacion) {
    loading.value = true
    error.value = null
    
    try {
      const response = await authService.actualizarPerfil(datosActualizacion)
      user.value = response.usuario
      localStorage.setItem('usuario', JSON.stringify(response.usuario))
      return response
    } catch (err) {
      error.value = err.message || 'Error al actualizar perfil'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    // State
    user,
    token,
    loading,
    error,
    
    // Getters
    isAuthenticated,
    userId,
    userRole,
    userName,
    
    // Actions
    login,
    registro,
    logout,
    cargarUsuario,
    actualizarPerfil
  }
})
