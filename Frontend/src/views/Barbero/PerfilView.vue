<template>
  <div class="view-container perfil-view">
    <div class="perfil-header">
      <h1>Mi Perfil</h1>
      <button @click="guardarPerfil" class="btn btn-primary" :disabled="loading || !hayCambios">
        💾 Guardar Cambios
      </button>
    </div>

    <!-- Estado de carga -->
    <div v-if="cargando" class="loading-state">
      <div class="spinner"></div>
      <p>Cargando perfil...</p>
    </div>

    <div v-else class="perfil-content">
      <!-- Foto de perfil -->
      <div class="foto-perfil-section">
        <div class="foto-avatar">
          <img 
            v-if="formData.fotoUrl" 
            :src="formData.fotoUrl" 
            alt="Foto de perfil"
            @error="handleImageError"
          />
          <span v-else>{{ obtenerIniciales(formData.nombre) }}</span>
        </div>
        <div class="foto-actions">
          <input 
            type="text" 
            v-model="formData.fotoUrl" 
            placeholder="URL de la foto de perfil"
            class="form-control"
          />
          <p class="help-text">Ingresa la URL de tu foto de perfil o deja vacío para usar el avatar por defecto</p>
        </div>
      </div>

      <!-- Información personal -->
      <div class="section">
        <h2>Información Personal</h2>
        <div class="form-grid">
          <div class="form-group">
            <label for="nombre">Nombre Completo *</label>
            <input 
              id="nombre"
              type="text" 
              v-model="formData.nombre" 
              @change="marcarCambio"
              class="form-control"
              required
            />
          </div>

          <div class="form-group">
            <label for="email">Email *</label>
            <input 
              id="email"
              type="email" 
              v-model="formData.email" 
              @change="marcarCambio"
              class="form-control"
              required
              disabled
            />
            <p class="help-text">El email no se puede modificar</p>
          </div>

          <div class="form-group">
            <label for="telefono">Teléfono</label>
            <input 
              id="telefono"
              type="tel" 
              v-model="formData.telefono" 
              @change="marcarCambio"
              class="form-control"
              placeholder="+1 234 567 8900"
            />
          </div>

          <div class="form-group">
            <label for="fechaNacimiento">Fecha de Nacimiento</label>
            <input 
              id="fechaNacimiento"
              type="date" 
              v-model="formData.fechaNacimiento" 
              @change="marcarCambio"
              class="form-control"
            />
          </div>
        </div>
      </div>

      <!-- Información profesional -->
      <div class="section">
        <h2>Información Profesional</h2>
        <div class="form-grid">
          <div class="form-group full-width">
            <label for="descripcion">Descripción Profesional</label>
            <textarea 
              id="descripcion"
              v-model="formData.descripcion" 
              @change="marcarCambio"
              class="form-control"
              rows="4"
              placeholder="Describe tu experiencia, especialidades y estilo de trabajo..."
            ></textarea>
          </div>

          <div class="form-group">
            <label for="experiencia">Años de Experiencia</label>
            <input 
              id="experiencia"
              type="number" 
              v-model.number="formData.experiencia" 
              @change="marcarCambio"
              class="form-control"
              min="0"
              max="50"
              placeholder="0"
            />
          </div>

          <div class="form-group">
            <label for="especialidades">Especialidades</label>
            <input 
              id="especialidades"
              type="text" 
              v-model="formData.especialidades" 
              @change="marcarCambio"
              class="form-control"
              placeholder="Corte, Barba, Color, ..."
            />
            <p class="help-text">Separa las especialidades por comas</p>
          </div>
        </div>
      </div>

      <!-- Redes sociales -->
      <div class="section">
        <h2>Redes Sociales</h2>
        <div class="form-grid">
          <div class="form-group">
            <label for="instagram">Instagram</label>
            <input 
              id="instagram"
              type="url" 
              v-model="formData.instagram" 
              @change="marcarCambio"
              class="form-control"
              placeholder="https://instagram.com/tu_usuario"
            />
          </div>

          <div class="form-group">
            <label for="facebook">Facebook</label>
            <input 
              id="facebook"
              type="url" 
              v-model="formData.facebook" 
              @change="marcarCambio"
              class="form-control"
              placeholder="https://facebook.com/tu_pagina"
            />
          </div>

          <div class="form-group">
            <label for="twitter">Twitter / X</label>
            <input 
              id="twitter"
              type="url" 
              v-model="formData.twitter" 
              @change="marcarCambio"
              class="form-control"
              placeholder="https://twitter.com/tu_usuario"
            />
          </div>

          <div class="form-group">
            <label for="tiktok">TikTok</label>
            <input 
              id="tiktok"
              type="url" 
              v-model="formData.tiktok" 
              @change="marcarCambio"
              class="form-control"
              placeholder="https://tiktok.com/@tu_usuario"
            />
          </div>
        </div>
      </div>

      <!-- Cambio de contraseña -->
      <div class="section">
        <h2>Cambiar Contraseña</h2>
        <div class="form-grid">
          <div class="form-group">
            <label for="passwordActual">Contraseña Actual</label>
            <input 
              id="passwordActual"
              type="password" 
              v-model="passwordData.passwordActual" 
              class="form-control"
              placeholder="••••••••"
            />
          </div>

          <div class="form-group">
            <label for="passwordNuevo">Contraseña Nueva</label>
            <input 
              id="passwordNuevo"
              type="password" 
              v-model="passwordData.passwordNuevo" 
              class="form-control"
              placeholder="••••••••"
            />
          </div>

          <div class="form-group">
            <label for="passwordConfirmar">Confirmar Contraseña Nueva</label>
            <input 
              id="passwordConfirmar"
              type="password" 
              v-model="passwordData.passwordConfirmar" 
              class="form-control"
              placeholder="••••••••"
            />
          </div>
        </div>
        <button 
          @click="cambiarPassword" 
          class="btn btn-warning"
          :disabled="loading || !validarPasswordChange"
        >
          🔒 Cambiar Contraseña
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import authService from '@/services/authService'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

const authStore = useAuthStore()

// Estado
const cargando = ref(false)
const loading = ref(false)
const datosOriginales = ref(null)

// Formulario principal
const formData = reactive({
  nombre: '',
  email: '',
  telefono: '',
  fechaNacimiento: '',
  fotoUrl: '',
  descripcion: '',
  experiencia: null,
  especialidades: '',
  instagram: '',
  facebook: '',
  twitter: '',
  tiktok: ''
})

// Cambio de contraseña
const passwordData = reactive({
  passwordActual: '',
  passwordNuevo: '',
  passwordConfirmar: ''
})

// Verificar si hay cambios
const hayCambios = computed(() => {
  if (!datosOriginales.value) return false
  
  return JSON.stringify(formData) !== JSON.stringify(datosOriginales.value)
})

// Validar cambio de contraseña
const validarPasswordChange = computed(() => {
  const { passwordActual, passwordNuevo, passwordConfirmar } = passwordData
  return passwordActual && 
         passwordNuevo && 
         passwordConfirmar && 
         passwordNuevo === passwordConfirmar &&
         passwordNuevo.length >= 6
})

// Cargar perfil
async function cargarPerfil() {
  cargando.value = true
  try {
    const usuario = authStore.user
    if (usuario) {
      formData.nombre = usuario.nombre || ''
      formData.email = usuario.email || ''
      formData.telefono = usuario.telefono || ''
      formData.fechaNacimiento = usuario.fechaNacimiento ? usuario.fechaNacimiento.split('T')[0] : ''
      formData.fotoUrl = usuario.fotoUrl || ''
      formData.descripcion = usuario.descripcion || ''
      formData.experiencia = usuario.experiencia || null
      formData.especialidades = usuario.especialidades || ''
      formData.instagram = usuario.instagram || ''
      formData.facebook = usuario.facebook || ''
      formData.twitter = usuario.twitter || ''
      formData.tiktok = usuario.tiktok || ''
      
      // Guardar originales
      datosOriginales.value = { ...formData }
    }
  } catch (error) {
    console.error('Error al cargar perfil:', error)
    toast.error('Error al cargar los datos del perfil')
  } finally {
    cargando.value = false
  }
}

// Marcar cambio
function marcarCambio() {
  // Solo para reactividad
}

// Guardar perfil
async function guardarPerfil() {
  if (!hayCambios.value) {
    toast.info('No hay cambios para guardar')
    return
  }

  loading.value = true
  try {
    const datosActualizar = {
      nombre: formData.nombre,
      telefono: formData.telefono,
      fechaNacimiento: formData.fechaNacimiento,
      fotoUrl: formData.fotoUrl,
      descripcion: formData.descripcion,
      experiencia: formData.experiencia,
      especialidades: formData.especialidades,
      instagram: formData.instagram,
      facebook: formData.facebook,
      twitter: formData.twitter,
      tiktok: formData.tiktok
    }

    await authStore.actualizarPerfil(datosActualizar)
    toast.success('Perfil actualizado exitosamente')
    
    // Actualizar originales
    datosOriginales.value = { ...formData }
  } catch (error) {
    console.error('Error al guardar perfil:', error)
    toast.error(error.response?.data?.message || 'Error al actualizar el perfil')
  } finally {
    loading.value = false
  }
}

// Cambiar contraseña
async function cambiarPassword() {
  if (!validarPasswordChange.value) {
    toast.error('Las contraseñas no coinciden o son muy cortas')
    return
  }

  loading.value = true
  try {
    await authService.cambiarPassword({
      passwordActual: passwordData.passwordActual,
      passwordNuevo: passwordData.passwordNuevo
    })
    
    toast.success('Contraseña cambiada exitosamente')
    
    // Limpiar campos
    passwordData.passwordActual = ''
    passwordData.passwordNuevo = ''
    passwordData.passwordConfirmar = ''
  } catch (error) {
    console.error('Error al cambiar contraseña:', error)
    toast.error(error.response?.data?.message || 'Error al cambiar la contraseña')
  } finally {
    loading.value = false
  }
}

// Manejar error de imagen
function handleImageError(e) {
  e.target.style.display = 'none'
}

// Obtener iniciales
function obtenerIniciales(nombre) {
  if (!nombre) return 'U'
  const partes = nombre.split(' ')
  if (partes.length >= 2) {
    return (partes[0][0] + partes[1][0]).toUpperCase()
  }
  return nombre.substring(0, 2).toUpperCase()
}

onMounted(() => {
  cargarPerfil()
})
</script>

<style scoped>
.perfil-view {
  max-width: 1000px;
  margin: 0 auto;
}

.perfil-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.perfil-header h1 {
  margin: 0;
}

.loading-state {
  text-align: center;
  padding: 4rem 2rem;
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.spinner {
  width: 50px;
  height: 50px;
  border: 4px solid #f3f3f3;
  border-top: 4px solid var(--color-primary);
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.perfil-content {
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  padding: 2rem;
}

/* Foto de perfil */
.foto-perfil-section {
  display: flex;
  align-items: center;
  gap: 2rem;
  margin-bottom: 2rem;
  padding-bottom: 2rem;
  border-bottom: 2px solid #e0e0e0;
  flex-wrap: wrap;
}

.foto-avatar {
  width: 120px;
  height: 120px;
  border-radius: 50%;
  background: var(--color-primary);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2.5rem;
  font-weight: 700;
  overflow: hidden;
  flex-shrink: 0;
}

.foto-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.foto-actions {
  flex: 1;
  min-width: 250px;
}

.foto-actions input {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 1rem;
  margin-bottom: 0.5rem;
}

.foto-actions input:focus {
  outline: none;
  border-color: var(--color-primary);
}

.help-text {
  font-size: 0.85rem;
  color: #666;
  margin: 0;
}

/* Secciones */
.section {
  margin-bottom: 2rem;
}

.section h2 {
  font-size: 1.25rem;
  color: #333;
  margin: 0 0 1.5rem 0;
  padding-bottom: 0.75rem;
  border-bottom: 2px solid #e0e0e0;
}

/* Grid de formulario */
.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group.full-width {
  grid-column: 1 / -1;
}

.form-group label {
  font-weight: 600;
  color: #333;
  margin-bottom: 0.5rem;
  font-size: 0.95rem;
}

.form-group input,
.form-group textarea,
.form-group select {
  padding: 0.75rem 1rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 1rem;
  transition: border-color 0.3s ease;
  font-family: inherit;
}

.form-group input:focus,
.form-group textarea:focus,
.form-group select:focus {
  outline: none;
  border-color: var(--color-primary);
}

.form-group textarea {
  resize: vertical;
  min-height: 100px;
}

.form-group input:disabled {
  background: #f5f5f5;
  cursor: not-allowed;
  opacity: 0.7;
}

/* Botones */
.btn {
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  border: none;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  font-size: 1rem;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background: var(--color-primary);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: var(--color-primary-dark);
  transform: translateY(-2px);
}

.btn-warning {
  background: #ffc107;
  color: #333;
}

.btn-warning:hover:not(:disabled) {
  background: #e0a800;
}

/* Responsive */
@media (max-width: 768px) {
  .perfil-header {
    flex-direction: column;
    align-items: stretch;
  }
  
  .perfil-header .btn {
    width: 100%;
  }
  
  .foto-perfil-section {
    flex-direction: column;
    text-align: center;
  }
  
  .form-grid {
    grid-template-columns: 1fr;
  }
}
</style>
