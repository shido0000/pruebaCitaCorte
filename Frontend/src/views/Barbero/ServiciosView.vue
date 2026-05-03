<template>
  <div class="view-container">
    <div class="header">
      <h1>Mis Servicios</h1>
      <button @click="abrirModal()" class="btn btn-primary">
        + Nuevo Servicio
      </button>
    </div>

    <!-- Filtros y búsqueda -->
    <div class="filters-section">
      <div class="search-box">
        <input 
          v-model="filtros.busqueda" 
          @input="cargarServicios"
          placeholder="Buscar por nombre..." 
          class="search-input"
        />
      </div>
      <div class="filter-select">
        <select v-model="filtros.categoria" @change="cargarServicios" class="select-input">
          <option value="">Todas las categorías</option>
          <option v-for="cat in categorias" :key="cat.id" :value="cat.id">
            {{ cat.nombre }}
          </option>
        </select>
      </div>
    </div>

    <!-- Estado de carga -->
    <div v-if="loading" class="loading-state">
      <div class="spinner"></div>
      <p>Cargando servicios...</p>
    </div>

    <!-- Lista vacía -->
    <div v-else-if="servicios.length === 0" class="empty-state">
      <p>No tienes servicios registrados</p>
      <button @click="abrirModal()" class="btn btn-primary">Crear primer servicio</button>
    </div>

    <!-- Tabla de servicios -->
    <div v-else class="services-grid">
      <div 
        v-for="servicio in servicios" 
        :key="servicio.id" 
        class="service-card"
      >
        <div class="service-header">
          <h3>{{ servicio.nombre }}</h3>
          <span class="categoria-badge">{{ servicio.categoriaNombre }}</span>
        </div>
        <div class="service-body">
          <p class="descripcion">{{ servicio.descripcion || 'Sin descripción' }}</p>
          <div class="service-info">
            <span class="precio">${{ servicio.precio }}</span>
            <span class="duracion">⏱ {{ servicio.duracionMinutos }} min</span>
          </div>
        </div>
        <div class="service-actions">
          <button @click="editarServicio(servicio)" class="btn btn-secondary">
            Editar
          </button>
          <button @click="eliminarServicio(servicio.id)" class="btn btn-danger">
            Eliminar
          </button>
        </div>
      </div>
    </div>

    <!-- Modal para crear/editar -->
    <div v-if="mostrarModal" class="modal-overlay" @click.self="cerrarModal">
      <div class="modal-content">
        <div class="modal-header">
          <h2>{{ esEdicion ? 'Editar Servicio' : 'Nuevo Servicio' }}</h2>
          <button @click="cerrarModal" class="btn-close">&times;</button>
        </div>
        
        <form @submit.prevent="guardarServicio" class="modal-form">
          <div class="form-group">
            <label for="nombre">Nombre *</label>
            <input 
              id="nombre"
              v-model="formulario.nombre" 
              type="text" 
              required 
              class="form-input"
              placeholder="Ej: Corte de cabello"
            />
          </div>

          <div class="form-group">
            <label for="descripcion">Descripción</label>
            <textarea 
              id="descripcion"
              v-model="formulario.descripcion" 
              rows="3" 
              class="form-input"
              placeholder="Describe el servicio..."
            ></textarea>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="precio">Precio ($) *</label>
              <input 
                id="precio"
                v-model.number="formulario.precio" 
                type="number" 
                step="0.01" 
                min="0"
                required 
                class="form-input"
              />
            </div>

            <div class="form-group">
              <label for="duracion">Duración (min) *</label>
              <input 
                id="duracion"
                v-model.number="formulario.duracionMinutos" 
                type="number" 
                min="5"
                required 
                class="form-input"
              />
            </div>
          </div>

          <div class="form-group">
            <label for="categoria">Categoría *</label>
            <select 
              id="categoria"
              v-model="formulario.categoriaId" 
              required 
              class="form-input"
            >
              <option value="">Seleccionar categoría</option>
              <option v-for="cat in categorias" :key="cat.id" :value="cat.id">
                {{ cat.nombre }}
              </option>
            </select>
          </div>

          <div class="modal-actions">
            <button type="button" @click="cerrarModal" class="btn btn-secondary">
              Cancelar
            </button>
            <button type="submit" :disabled="guardando" class="btn btn-primary">
              {{ guardando ? 'Guardando...' : 'Guardar' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import servicioService from '../../services/servicioService'

const loading = ref(false)
const guardando = ref(false)
const mostrarModal = ref(false)
const esEdicion = ref(false)
const servicioEditando = ref(null)

const servicios = ref([])
const categorias = ref([])

const filtros = reactive({
  busqueda: '',
  categoria: ''
})

const formulario = reactive({
  nombre: '',
  descripcion: '',
  precio: 0,
  duracionMinutos: 30,
  categoriaId: '',
  idBarberia: '' // Se obtendrá del perfil del barbero
})

onMounted(() => {
  cargarServicios()
  cargarCategorias()
})

async function cargarServicios() {
  loading.value = true
  try {
    const response = await servicioService.obtenerServicios(filtros)
    servicios.value = Array.isArray(response) ? response : (response.items || [])
  } catch (error) {
    console.error('Error al cargar servicios:', error)
    alert('Error al cargar los servicios')
  } finally {
    loading.value = false
  }
}

async function cargarCategorias() {
  try {
    const response = await servicioService.obtenerCategorias()
    categorias.value = Array.isArray(response) ? response : (response.items || [])
  } catch (error) {
    console.error('Error al cargar categorías:', error)
  }
}

function abrirModal(servicio = null) {
  esEdicion.value = !!servicio
  servicioEditando.value = servicio
  
  if (servicio) {
    formulario.nombre = servicio.nombre
    formulario.descripcion = servicio.descripcion || ''
    formulario.precio = servicio.precio
    formulario.duracionMinutos = servicio.duracionMinutos
    formulario.categoriaId = servicio.idCategoriaServicio || servicio.categoriaId
    formulario.idBarberia = servicio.idBarberia || servicio.barberiaId
  } else {
    resetearFormulario()
  }
  
  mostrarModal.value = true
}

function cerrarModal() {
  mostrarModal.value = false
  resetearFormulario()
  servicioEditando.value = null
}

function resetearFormulario() {
  formulario.nombre = ''
  formulario.descripcion = ''
  formulario.precio = 0
  formulario.duracionMinutos: 30
  formulario.categoriaId = ''
  formulario.idBarberia = ''
}

function editarServicio(servicio) {
  abrirModal(servicio)
}

async function guardarServicio() {
  guardando.value = true
  try {
    const datosParaEnviar = {
      ...formulario,
      idCategoriaServicio: formulario.categoriaId,
      categoriaId: undefined
    }
    
    if (esEdicion.value && servicioEditando.value) {
      await servicioService.actualizarServicio(servicioEditando.value.id, datosParaEnviar)
      alert('Servicio actualizado correctamente')
    } else {
      await servicioService.crearServicio(datosParaEnviar)
      alert('Servicio creado correctamente')
    }
    
    cerrarModal()
    await cargarServicios()
  } catch (error) {
    console.error('Error al guardar servicio:', error)
    alert(error.response?.data?.message || 'Error al guardar el servicio')
  } finally {
    guardando.value = false
  }
}

async function eliminarServicio(id) {
  if (!confirm('¿Estás seguro de que deseas eliminar este servicio?')) {
    return
  }
  
  try {
    await servicioService.eliminarServicio(id)
    alert('Servicio eliminado correctamente')
    await cargarServicios()
  } catch (error) {
    console.error('Error al eliminar servicio:', error)
    alert('Error al eliminar el servicio')
  }
}
</script>

<style scoped>
.view-container {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.header h1 {
  color: #1a1a2e;
  margin: 0;
  font-size: 2rem;
}

.filters-section {
  display: flex;
  gap: 1rem;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.search-box {
  flex: 1;
  min-width: 250px;
}

.search-input {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 0.5rem;
  font-size: 1rem;
  transition: border-color 0.2s;
}

.search-input:focus {
  outline: none;
  border-color: #e94560;
}

.filter-select {
  min-width: 200px;
}

.select-input {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 0.5rem;
  font-size: 1rem;
  background-color: white;
  cursor: pointer;
}

.select-input:focus {
  outline: none;
  border-color: #e94560;
}

.loading-state,
.empty-state {
  text-align: center;
  padding: 3rem;
  color: #6b7280;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 3px solid #f3f3f3;
  border-top: 3px solid #e94560;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.services-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

.service-card {
  background: white;
  border-radius: 1rem;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s, box-shadow 0.2s;
}

.service-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.service-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.service-header h3 {
  margin: 0;
  font-size: 1.125rem;
  font-weight: 600;
}

.categoria-badge {
  background: rgba(255, 255, 255, 0.2);
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
  font-size: 0.75rem;
  font-weight: 500;
}

.service-body {
  padding: 1.25rem;
}

.descripcion {
  color: #6b7280;
  font-size: 0.875rem;
  margin: 0 0 1rem 0;
  line-height: 1.5;
}

.service-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 1rem;
  border-top: 1px solid #e5e7eb;
}

.precio {
  font-size: 1.5rem;
  font-weight: 700;
  color: #e94560;
}

.duracion {
  color: #6b7280;
  font-size: 0.875rem;
}

.service-actions {
  display: flex;
  gap: 0.5rem;
  padding: 1rem 1.25rem;
  background: #f9fafb;
  border-top: 1px solid #e5e7eb;
}

.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 0.5rem;
  font-weight: 500;
  cursor: pointer;
  transition: opacity 0.2s, transform 0.1s;
  font-size: 0.875rem;
}

.btn:hover {
  opacity: 0.9;
}

.btn:active {
  transform: scale(0.98);
}

.btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-primary {
  background: linear-gradient(135deg, #e94560 0%, #d63850 100%);
  color: white;
}

.btn-secondary {
  background-color: #6b7280;
  color: white;
}

.btn-danger {
  background-color: #ef4444;
  color: white;
}

/* Modal styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1rem;
}

.modal-content {
  background: white;
  border-radius: 1rem;
  width: 100%;
  max-width: 500px;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid #e5e7eb;
}

.modal-header h2 {
  margin: 0;
  color: #1a1a2e;
  font-size: 1.25rem;
}

.btn-close {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: #6b7280;
  padding: 0;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  transition: background-color 0.2s;
}

.btn-close:hover {
  background-color: #f3f4f6;
}

.modal-form {
  padding: 1.5rem;
}

.form-group {
  margin-bottom: 1.25rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  color: #1a1a2e;
  font-weight: 500;
  font-size: 0.875rem;
}

.form-input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #e5e7eb;
  border-radius: 0.5rem;
  font-size: 1rem;
  transition: border-color 0.2s;
  box-sizing: border-box;
}

.form-input:focus {
  outline: none;
  border-color: #e94560;
}

textarea.form-input {
  resize: vertical;
  min-height: 80px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
  margin-top: 1.5rem;
  padding-top: 1.5rem;
  border-top: 1px solid #e5e7eb;
}

@media (max-width: 768px) {
  .view-container {
    padding: 1rem;
  }

  .header {
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }

  .header h1 {
    font-size: 1.5rem;
  }

  .filters-section {
    flex-direction: column;
  }

  .search-box,
  .filter-select {
    min-width: 100%;
  }

  .services-grid {
    grid-template-columns: 1fr;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .modal-actions {
    flex-direction: column-reverse;
  }

  .modal-actions .btn {
    width: 100%;
  }
}
</style>
