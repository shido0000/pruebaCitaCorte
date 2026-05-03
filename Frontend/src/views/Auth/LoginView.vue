<template>
  <div class="login-container">
    <div class="login-card">
      <div class="login-header">
        <span class="logo-icon">✂️</span>
        <h1>MultiBarbero</h1>
        <p>Inicia sesión en tu cuenta</p>
      </div>

      <form @submit.prevent="handleLogin" class="login-form">
        <div class="form-group">
          <label for="email">Email</label>
          <input 
            type="email" 
            id="email" 
            v-model="form.email" 
            placeholder="tu@email.com"
            required
            :disabled="loading"
          />
        </div>

        <div class="form-group">
          <label for="password">Contraseña</label>
          <input 
            type="password" 
            id="password" 
            v-model="form.password" 
            placeholder="••••••••"
            required
            :disabled="loading"
          />
        </div>

        <div v-if="error" class="error-message">
          {{ error }}
        </div>

        <button type="submit" class="btn-submit" :disabled="loading">
          <span v-if="loading">Iniciando sesión...</span>
          <span v-else>Iniciar Sesión</span>
        </button>
      </form>

      <div class="login-footer">
        <p>¿No tienes cuenta? <router-link to="/registro">Regístrate aquí</router-link></p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/authStore'

const router = useRouter()
const authStore = useAuthStore()

const form = reactive({
  email: '',
  password: ''
})

const loading = ref(false)
const error = ref('')

async function handleLogin() {
  loading.value = true
  error.value = ''

  try {
    await authStore.login(form.email, form.password)
    
    // Redirigir según el rol
    const role = authStore.userRole
    const routes = {
      'Admin': '/admin',
      'Barbero': '/barbero',
      'Barberia': '/barberia',
      'Comercial': '/comercial',
      'Cliente': '/cliente'
    }
    
    router.push(routes[role] || '/')
  } catch (err) {
    error.value = err.response?.data?.message || 'Error al iniciar sesión. Verifica tus credenciales.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.login-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
}

.login-card {
  background: white;
  border-radius: 1rem;
  padding: 2.5rem;
  width: 100%;
  max-width: 28rem;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
}

.login-header {
  text-align: center;
  margin-bottom: 2rem;
}

.logo-icon {
  font-size: 3rem;
  display: block;
  margin-bottom: 0.5rem;
}

.login-header h1 {
  color: #1a1a2e;
  margin: 0 0 0.5rem 0;
  font-size: 1.75rem;
}

.login-header p {
  color: #6b7280;
  margin: 0;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-group label {
  font-weight: 500;
  color: #374151;
  font-size: 0.875rem;
}

.form-group input {
  padding: 0.75rem 1rem;
  border: 1px solid #d1d5db;
  border-radius: 0.5rem;
  font-size: 1rem;
  transition: border-color 0.2s, box-shadow 0.2s;
}

.form-group input:focus {
  outline: none;
  border-color: #e94560;
  box-shadow: 0 0 0 3px rgba(233, 69, 96, 0.1);
}

.form-group input:disabled {
  background-color: #f3f4f6;
  cursor: not-allowed;
}

.error-message {
  background-color: #fee2e2;
  color: #dc2626;
  padding: 0.75rem 1rem;
  border-radius: 0.5rem;
  font-size: 0.875rem;
}

.btn-submit {
  background-color: #e94560;
  color: white;
  border: none;
  padding: 0.875rem 1.5rem;
  border-radius: 0.5rem;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-submit:hover:not(:disabled) {
  background-color: #d63850;
}

.btn-submit:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.login-footer {
  margin-top: 2rem;
  text-align: center;
  color: #6b7280;
  font-size: 0.875rem;
}

.login-footer a {
  color: #e94560;
  text-decoration: none;
  font-weight: 500;
}

.login-footer a:hover {
  text-decoration: underline;
}
</style>
