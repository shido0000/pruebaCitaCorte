<template>
  <div class="servicios-view">
    <div class="page-header">
      <h1><i class="fas fa-cut"></i> Servicios</h1>
      <p>Gestiona los servicios que ofrece tu barbería</p>
    </div>

    <div class="actions-bar">
      <button class="btn-primary" @click="abrirModal()">
        <i class="fas fa-plus"></i> Nuevo Servicio
      </button>
    </div>

    <div class="servicios-grid">
      <div v-for="servicio in servicios" :key="servicio.id" class="servicio-card">
        <div class="servicio-header">
          <div class="servicio-icon">
            <i :class="servicio.icono || 'fas fa-cut'"></i>
          </div>
          <div class="servicio-actions">
            <button class="btn-icon" @click="abrirModal(servicio)"><i class="fas fa-edit"></i></button>
            <button class="btn-icon danger" @click="eliminarServicio(servicio)"><i class="fas fa-trash"></i></button>
          </div>
        </div>
        <div class="servicio-body">
          <h3>{{ servicio.nombre }}</h3>
          <p class="descripcion">{{ servicio.descripcion || 'Sin descripción' }}</p>
          <div class="servicio-info">
            <span class="precio">${{ servicio.precio?.toFixed(2) }}</span>
            <span class="duracion"><i class="far fa-clock"></i> {{ servicio.duracion }} min</span>
          </div>
          <span class="badge" :class="servicio.activo ? 'badge-activo' : 'badge-inactivo'">
            {{ servicio.activo ? 'Activo' : 'Inactivo' }}
          </span>
        </div>
      </div>
    </div>

    <!-- Modal -->
    <div v-if="mostrarModal" class="modal-overlay" @click.self="mostrarModal = false">
      <div class="modal">
        <div class="modal-header">
          <h2>{{ editando ? 'Editar Servicio' : 'Nuevo Servicio' }}</h2>
          <button class="btn-close" @click="mostrarModal = false"><i class="fas fa-times"></i></button>
        </div>
        <form @submit.prevent="guardarServicio">
          <div class="modal-body">
            <div class="form-group">
              <label>Nombre *</label>
              <input type="text" v-model="formulario.nombre" required />
            </div>
            <div class="form-group">
              <label>Descripción</label>
              <textarea v-model="formulario.descripcion" rows="3"></textarea>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label>Precio ($) *</label>
                <input type="number" v-model.number="formulario.precio" step="0.01" min="0" required />
              </div>
              <div class="form-group">
                <label>Duración (min) *</label>
                <input type="number" v-model.number="formulario.duracion" min="5" step="5" required />
              </div>
            </div>
            <div class="form-group">
              <label>Categoría</label>
              <select v-model="formulario.categoriaId">
                <option value="">Seleccionar categoría</option>
                <option v-for="cat in categorias" :key="cat.id" :value="cat.id">{{ cat.nombre }}</option>
              </select>
            </div>
            <div class="form-group">
              <label>Icono</label>
              <select v-model="formulario.icono">
                <option value="fas fa-cut">✂️ Corte</option>
                <option value="fas fa-user-tie">🤵 Barbero</option>
                <option value="fas fa-paint-brush">🎨 Color</option>
                <option value="fas fa-spa">💆 Masaje</option>
                <option value="fas fa-star">⭐ Premium</option>
              </select>
            </div>
            <div class="form-group checkbox">
              <input type="checkbox" id="activo" v-model="formulario.activo" />
              <label for="activo">Servicio Activo</label>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn-secondary" @click="mostrarModal = false">Cancelar</button>
            <button type="submit" class="btn-primary" :disabled="guardando">
              <i class="fas fa-save"></i> {{ guardando ? 'Guardando...' : 'Guardar' }}
            </button>
          </div>
        </form>
      </div>
    </div>

    <div v-if="cargando" class="loading-overlay">
      <div class="spinner"></div>
      <p>Cargando servicios...</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const cargando = ref(false);
const guardando = ref(false);
const mostrarModal = ref(false);
const editando = ref(false);
const servicios = ref([]);
const categorias = ref([]);

const formulario = ref({
  id: null,
  nombre: '',
  descripcion: '',
  precio: 0,
  duracion: 30,
  categoriaId: '',
  icono: 'fas fa-cut',
  activo: true
});

onMounted(async () => {
  await cargarServicios();
  await cargarCategorias();
});

async function cargarServicios() {
  cargando.value = true;
  try {
    const response = await fetch('/api/multibarbero/servicios?barberiaId=current', {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    });
    if (response.ok) {
      servicios.value = await response.json();
    }
  } catch (error) {
    console.error('Error cargando servicios:', error);
  } finally {
    cargando.value = false;
  }
}

async function cargarCategorias() {
  try {
    const response = await fetch('/api/multibarbero/categorias', {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    });
    if (response.ok) {
      categorias.value = await response.json();
    }
  } catch (error) {
    console.error('Error cargando categorías:', error);
  }
}

function abrirModal(servicio = null) {
  editando.value = !!servicio;
  if (servicio) {
    formulario.value = { ...servicio };
  } else {
    formulario.value = {
      id: null,
      nombre: '',
      descripcion: '',
      precio: 0,
      duracion: 30,
      categoriaId: '',
      icono: 'fas fa-cut',
      activo: true
    };
  }
  mostrarModal.value = true;
}

async function guardarServicio() {
  guardando.value = true;
  try {
    const url = editando.value 
      ? `/api/multibarbero/servicios/${formulario.value.id}`
      : '/api/multibarbero/servicios';
    
    const method = editando.value ? 'PUT' : 'POST';
    
    const response = await fetch(url, {
      method,
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify(formulario.value)
    });
    
    if (response.ok) {
      await cargarServicios();
      mostrarModal.value = false;
      alert(editando.value ? 'Servicio actualizado' : 'Servicio creado');
    }
  } catch (error) {
    console.error('Error guardando:', error);
    alert('Error al guardar el servicio');
  } finally {
    guardando.value = false;
  }
}

async function eliminarServicio(servicio) {
  if (!confirm(`¿Eliminar el servicio "${servicio.nombre}"?`)) return;
  
  try {
    const response = await fetch(`/api/multibarbero/servicios/${servicio.id}`, {
      method: 'DELETE',
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    });
    
    if (response.ok) {
      await cargarServicios();
      alert('Servicio eliminado');
    }
  } catch (error) {
    console.error('Error eliminando:', error);
  }
}
</script>

<style scoped>
.servicios-view { padding: 2rem; }
.page-header { margin-bottom: 2rem; }
.page-header h1 { font-size: 2rem; color: #1F2937; margin-bottom: 0.5rem; }
.actions-bar { margin-bottom: 1.5rem; }
.btn-primary {
  background: #4F46E5;
  color: white;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}
.btn-primary:hover { background: #4338CA; }
.btn-primary:disabled { opacity: 0.6; }
.btn-secondary {
  background: #F3F4F6;
  color: #374151;
  padding: 0.625rem 1.25rem;
  border: 1px solid #D1D5DB;
  border-radius: 6px;
  font-weight: 500;
  cursor: pointer;
}
.servicios-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}
.servicio-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
  overflow: hidden;
  transition: transform 0.2s;
}
.servicio-card:hover { transform: translateY(-2px); box-shadow: 0 4px 12px rgba(0,0,0,0.15); }
.servicio-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  background: linear-gradient(135deg, #4F46E5, #7C3AED);
  color: white;
}
.servicio-icon {
  width: 50px;
  height: 50px;
  background: rgba(255,255,255,0.2);
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
}
.servicio-actions { display: flex; gap: 0.5rem; }
.btn-icon {
  padding: 0.5rem;
  border: none;
  background: rgba(255,255,255,0.2);
  color: white;
  border-radius: 6px;
  cursor: pointer;
}
.btn-icon:hover { background: rgba(255,255,255,0.3); }
.btn-icon.danger:hover { background: rgba(239, 68, 68, 0.5); }
.servicio-body { padding: 1.25rem; }
.servicio-body h3 { font-size: 1.125rem; color: #1F2937; margin-bottom: 0.5rem; }
.descripcion { color: #6B7280; font-size: 0.875rem; margin-bottom: 1rem; min-height: 40px; }
.servicio-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}
.precio { font-size: 1.25rem; font-weight: 700; color: #10B981; }
.duracion { color: #6B7280; font-size: 0.875rem; }
.badge {
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
  font-size: 0.75rem;
  font-weight: 600;
}
.badge-activo { background: #D1FAE5; color: #065F46; }
.badge-inactivo { background: #F3F4F6; color: #6B7280; }
.modal-overlay {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}
.modal {
  background: white;
  border-radius: 12px;
  max-width: 500px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
}
.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.25rem;
  border-bottom: 1px solid #E5E7EB;
}
.modal-header h2 { font-size: 1.25rem; color: #1F2937; }
.btn-close {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: #6B7280;
}
.modal-body { padding: 1.25rem; }
.form-group { margin-bottom: 1rem; }
.form-group label {
  display: block;
  font-weight: 600;
  color: #4B5563;
  margin-bottom: 0.5rem;
  font-size: 0.875rem;
}
.form-group input, .form-group textarea, .form-group select {
  width: 100%;
  padding: 0.625rem;
  border: 1px solid #D1D5DB;
  border-radius: 6px;
  font-size: 0.875rem;
}
.form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; }
.checkbox { display: flex; align-items: center; gap: 0.5rem; }
.checkbox input { width: auto; }
.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
  padding: 1.25rem;
  border-top: 1px solid #E5E7EB;
}
.loading-overlay {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  color: white;
}
.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid rgba(255,255,255,0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}
@keyframes spin { to { transform: rotate(360deg); } }
@media (max-width: 768px) {
  .servicios-view { padding: 1rem; }
  .servicios-grid { grid-template-columns: 1fr; }
  .form-row { grid-template-columns: 1fr; }
}
</style>
