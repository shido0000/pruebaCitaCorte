<template>
  <div class="registro-container">
    <div class="registro-card">
      <div class="registro-header">
        <span class="logo-icon">✂️</span>
        <h1>MultiBarbero</h1>
        <p>Crea tu cuenta gratuita</p>
      </div>

      <form @submit.prevent="handleRegistro" class="registro-form">
        <div class="form-row">
          <div class="form-group">
            <label for="nombre">Nombre</label>
            <input 
              type="text" 
              id="nombre" 
              v-model="form.nombre" 
              placeholder="Tu nombre"
              required
              :disabled="loading"
            />
          </div>

          <div class="form-group">
            <label for="apellidos">Apellidos</label>
            <input 
              type="text" 
              id="apellidos" 
              v-model="form.apellidos" 
              placeholder="Tus apellidos"
              required
              :disabled="loading"
            />
          </div>
        </div>

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
          <label for="telefono">Teléfono</label>
          <input 
            type="tel" 
            id="telefono" 
            v-model="form.telefono" 
            placeholder="+1 234 567 8900"
            :disabled="loading"
          />
        </div>

        <div class="form-group">
          <label for="role">Tipo de Cuenta</label>
          <select 
            id="role" 
            v-model="form.role" 
            required
            :disabled="loading"
          >
            <option value="">Selecciona un tipo</option>
            <option value="Cliente">Cliente</option>
            <option value="Barbero">Barbero</option>
            <option value="Barberia">Barbería</option>
            <option value="Comercial">Comercial</option>
          </select>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label for="password">Contraseña</label>
            <input 
              type="password" 
              id="password" 
              v-model="form.password" 
              placeholder="••••••••"
              required
              minlength="6"
              :disabled="loading"
            />
          </div>

          <div class="form-group">
            <label for="confirmPassword">Confirmar Contraseña</label>
            <input 
              type="password" 
              id="confirmPassword" 
              v-model="form.confirmPassword" 
              placeholder="••••••••"
              required
              minlength="6"
              :disabled="loading"
            />
          </div>
        </div>

        <div v-if="error" class="error-message">
          {{ error }}
        </div>

        <button type="submit" class="btn-submit" :disabled="loading">
          <span v-if="loading">Creando cuenta...</span>
          <span v-else>Crear Cuenta</span>
        </button>
      </form>

      <div class="registro-footer">
        <p>¿Ya tienes cuenta? <router-link to="/login">Inicia sesión</router-link></p>
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
  nombre: '',
  apellidos: '',
  email: '',
  telefono: '',
  role: '',
  password: '',
  confirmPassword: ''
})

const loading = ref(false)
const error = ref('')

async function handleRegistro() {
  if (form.password !== form.confirmPassword) {
    error.value = 'Las contraseñas no coinciden'
    return
  }

  loading.value = true
  error.value = ''

  try {
    const datosRegistro = {
      nombre: form.nombre,
      apellidos: form.apellidos,
      email: form.email,
      telefono: form.telefono,
      role: form.role,
      password: form.password
    }

    await authStore.registro(datosRegistro)
    
    // Redirigir al login después del registro exitoso
    router.push('/login?registrado=true')
  } catch (err) {
    error.value = err.response?.data?.message || 'Error al crear la cuenta. Inténtalo de nuevo.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.registro-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
}

.registro-card {
  background: white;
  border-radius: 1rem;
  padding: 2.5rem;
  width: 100%;
  max-width: 32rem;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
}

.registro-header {
  text-align: center;
  margin-bottom: 2rem;
}

.logo-icon {
  font-size: 3rem;
  display: block;
  margin-bottom: 0.5rem;
}

.registro-header h1 {
  color: #1a1a2e;
  margin: 0 0 0.5rem 0;
  font-size: 1.75rem;
}

.registro-header p {
  color: #6b7280;
  margin: 0;
}

.registro-form {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
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

.form-group input,
.form-group select {
  padding: 0.75rem 1rem;
  border: 1px solid #d1d5db;
  border-radius: 0.5rem;
  font-size: 1rem;
  transition: border-color 0.2s, box-shadow 0.2s;
}

.form-group input:focus,
.form-group select:focus {
  outline: none;
  border-color: #e94560;
  box-shadow: 0 0 0 3px rgba(233, 69, 96, 0.1);
}

.form-group input:disabled,
.form-group select:disabled {
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
  margin-top: 0.5rem;
}

.btn-submit:hover:not(:disabled) {
  background-color: #d63850;
}

.btn-submit:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.registro-footer {
  margin-top: 2rem;
  text-align: center;
  color: #6b7280;
  font-size: 0.875rem;
}

.registro-footer a {
  color: #e94560;
  text-decoration: none;
  font-weight: 500;
}

.registro-footer a:hover {
  text-decoration: underline;
}

@media (max-width: 640px) {
  .form-row {
    grid-template-columns: 1fr;
  }
}
</style>
