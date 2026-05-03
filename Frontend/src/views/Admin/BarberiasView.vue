<template>
  <div class="view-container">
    <div class="header-actions">
      <h1>Gestión de Barberías</h1>
      <button @click="abrirModalCrear()" class="btn btn-primary">
        <i class="fas fa-plus"></i> Nueva Barbería
      </button>
    </div>

    <!-- Filtros -->
    <div class="filtros-card">
      <div class="filtros-grid">
        <div class="filtro-item">
          <label>Buscar</label>
          <input 
            v-model="filtros.busqueda" 
            placeholder="Nombre, ciudad..." 
            @input="cargarBarberias"
          />
        </div>
        <div class="filtro-item">
          <label>Estado</label>
          <select v-model="filtros.estado" @change="cargarBarberias">
            <option value="">Todos</option>
            <option value="Activo">Activos</option>
            <option value="Inactivo">Inactivos</option>
          </select>
        </div>
        <div class="filtro-item">
          <label>Plan</label>
          <select v-model="filtros.plan" @change="cargarBarberias">
            <option value="">Todos</option>
            <option value="Basico">Básico</option>
            <option value="Estandar">Estándar</option>
            <option value="Enterprise">Enterprise</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Grid de Barberías -->
    <div class="grid-barberias">
      <div v-for="barberia in barberias" :key="barberia.id" class="card-barberia">
        <div class="card-header">
          <div class="avatar">{{ obtenerIniciales(barberia.nombreComercial) }}</div>
          <div class="estado">
            <span :class="['badge', 'badge-' + (barberia.activo ? 'success' : 'danger')]">
              {{ barberia.activo ? 'Activo' : 'Inactivo' }}
            </span>
          </div>
        </div>
        
        <div class="card-body">
          <h3>{{ barberia.nombreComercial }}</h3>
          <p class="texto-suave"><i class="fas fa-map-marker-alt"></i> {{ barberia.ciudad || 'N/A' }}</p>
          
          <div class="info-grid">
            <div class="info-item">
              <label>Barberos</label>
              <span>{{ barberia.totalBarberos || 0 }}/{{ barberia.capacidadMaxima || 10 }}</span>
            </div>
            <div class="info-item">
              <label>Calificación</label>
              <span><i class="fas fa-star"></i> {{ barberia.calificacionPromedio?.toFixed(1) || '0.0' }}</span>
            </div>
          </div>
          
          <div class="plan-info">
            <span :class="['badge', 'badge-plan']">
              {{ barberia.planSuscripcion?.tipoPlan || 'Sin Plan' }}
            </span>
          </div>
        </div>
        
        <div class="card-actions">
          <button @click="verDetalles(barberia)" class="btn-icon" title="Ver detalles">
            <i class="fas fa-eye"></i>
          </button>
          <button @click="editarBarberia(barberia)" class="btn-icon" title="Editar">
            <i class="fas fa-edit"></i>
          </button>
          <button @click="toggleEstado(barberia)" class="btn-icon" :title="barberia.activo ? 'Desactivar' : 'Activar'">
            <i :class="barberia.activo ? 'fas fa-lock' : 'fas fa-unlock'"></i>
          </button>
        </div>
      </div>
      
      <div v-if="barberias.length === 0 && !cargando" class="sin-datos">
        <i class="fas fa-store-slash"></i>
        <p>No hay barberías registradas</p>
      </div>
    </div>

    <!-- Modal Crear/Editar -->
    <Modal 
      :visible="modalVisible" 
      :titulo="modoEdicion ? 'Editar Barbería' : 'Nueva Barbería'"
      @cerrar="cerrarModal"
    >
      <form @submit.prevent="guardarBarberia" class="formulario">
        <div class="formulario-grid">
          <div class="formulario-item full-width">
            <label>Nombre Comercial *</label>
            <input v-model="formulario.nombreComercial" required type="text" />
          </div>
          <div class="formulario-item">
            <label>Email *</label>
            <input v-model="formulario.email" required type="email" />
          </div>
          <div class="formulario-item">
            <label>Teléfono</label>
            <input v-model="formulario.telefono" type="tel" />
          </div>
          <div class="formulario-item">
            <label>Ciudad</label>
            <input v-model="formulario.ciudad" type="text" />
          </div>
          <div class="formulario-item">
            <label>Dirección</label>
            <input v-model="formulario.direccion" type="text" />
          </div>
          <div class="formulario-item">
            <label>Capacidad Máxima</label>
            <input v-model="formulario.capacidadMaxima" type="number" min="1" />
          </div>
          <div class="formulario-item">
            <label>Plan de Suscripción</label>
            <select v-model="formulario.planId">
              <option value="">Seleccionar plan</option>
              <option v-for="plan in planes" :key="plan.id" :value="plan.id">
                {{ plan.nombre }} - {{ plan.tipoPlan }}
              </option>
            </select>
          </div>
          <div class="formulario-item">
            <label>Estado</label>
            <select v-model="formulario.activo">
              <option :value="true">Activo</option>
              <option :value="false">Inactivo</option>
            </select>
          </div>
        </div>

        <div class="formulario-acciones">
          <button type="button" @click="cerrarModal" class="btn btn-secondary">Cancelar</button>
          <button type="submit" :disabled="cargando" class="btn btn-primary">
            {{ cargando ? 'Guardando...' : (modoEdicion ? 'Actualizar' : 'Crear') }}
          </button>
        </div>
      </form>
    </Modal>

    <!-- Modal Detalles -->
    <Modal 
      :visible="modalDetallesVisible" 
      titulo="Detalles de la Barbería"
      @cerrar="modalDetallesVisible = false"
    >
      <div v-if="barberiaSeleccionada" class="detalles-contenido">
        <div class="detalle-header">
          <div class="avatar-grande">{{ obtenerIniciales(barberiaSeleccionada.nombreComercial) }}</div>
          <div>
            <h3>{{ barberiaSeleccionada.nombreComercial }}</h3>
            <p class="texto-suave"><i class="fas fa-map-marker-alt"></i> {{ barberiaSeleccionada.ciudad || 'N/A' }}</p>
          </div>
        </div>
        
        <div class="detalle-grid">
          <div class="detalle-item">
            <label>Email</label>
            <p>{{ barberiaSeleccionada.email }}</p>
          </div>
          <div class="detalle-item">
            <label>Teléfono</label>
            <p>{{ barberiaSeleccionada.telefono || 'N/A' }}</p>
          </div>
          <div class="detalle-item">
            <label>Dirección</label>
            <p>{{ barberiaSeleccionada.direccion || 'N/A' }}</p>
          </div>
          <div class="detalle-item">
            <label>Plan Actual</label>
            <p>{{ barberiaSeleccionada.planSuscripcion?.nombre || 'Sin Plan' }}</p>
          </div>
          <div class="detalle-item">
            <label>Capacidad</label>
            <p>{{ barberiaSeleccionada.totalBarberos || 0 }}/{{ barberiaSeleccionada.capacidadMaxima || 10 }} barberos</p>
          </div>
          <div class="detalle-item">
            <label>Calificación</label>
            <p><i class="fas fa-star"></i> {{ barberiaSeleccionada.calificacionPromedio?.toFixed(1) || '0.0' }}</p>
          </div>
        </div>
      </div>
    </Modal>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useBarberiaStore } from '@/stores/barberiaStore';
import { usePlanStore } from '@/stores/planStore';
import Modal from '@/components/ui/Modal.vue';

const barberiaStore = useBarberiaStore();
const planStore = usePlanStore();

const barberias = ref([]);
const planes = ref([]);
const cargando = ref(false);
const filtros = ref({ busqueda: '', estado: '', plan: '' });
const modalVisible = ref(false);
const modalDetallesVisible = ref(false);
const modoEdicion = ref(false);
const barberiaSeleccionada = ref(null);

const formulario = ref({
  nombreComercial: '',
  email: '',
  telefono: '',
  ciudad: '',
  direccion: '',
  capacidadMaxima: 10,
  planId: '',
  activo: true
});

const cargarBarberias = async () => {
  cargando.value = true;
  try {
    await barberiaStore.obtenerTodos(filtros.value);
    barberias.value = barberiaStore.barberias;
  } catch (error) {
    console.error('Error cargando barberías:', error);
  } finally {
    cargando.value = false;
  }
};

const cargarPlanes = async () => {
  try {
    await planStore.cargarPlanes({ tipo: 'Barberia' });
    planes.value = planStore.planes;
  } catch (error) {
    console.error('Error cargando planes:', error);
  }
};

const abrirModalCrear = () => {
  modoEdicion.value = false;
  formulario.value = {
    nombreComercial: '',
    email: '',
    telefono: '',
    ciudad: '',
    direccion: '',
    capacidadMaxima: 10,
    planId: '',
    activo: true
  };
  modalVisible.value = true;
};

const editarBarberia = (barberia) => {
  modoEdicion.value = true;
  barberiaSeleccionada.value = barberia;
  formulario.value = {
    nombreComercial: barberia.nombreComercial,
    email: barberia.email,
    telefono: barberia.telefono || '',
    ciudad: barberia.ciudad || '',
    direccion: barberia.direccion || '',
    capacidadMaxima: barberia.capacidadMaxima || 10,
    planId: barberia.planSuscripcionId || '',
    activo: barberia.activo
  };
  modalVisible.value = true;
};

const guardarBarberia = async () => {
  cargando.value = true;
  try {
    if (modoEdicion.value) {
      await barberiaStore.actualizar(barberiaSeleccionada.value.id, formulario.value);
    } else {
      await barberiaStore.crear(formulario.value);
    }
    cerrarModal();
    await cargarBarberias();
  } catch (error) {
    console.error('Error guardando barbería:', error);
    alert(error.message || 'Error al guardar');
  } finally {
    cargando.value = false;
  }
};

const verDetalles = (barberia) => {
  barberiaSeleccionada.value = barberia;
  modalDetallesVisible.value = true;
};

const toggleEstado = async (barberia) => {
  if (!confirm(`¿Está seguro de ${barberia.activo ? 'desactivar' : 'activar'} esta barbería?`)) return;
  
  try {
    await barberiaStore.actualizar(barberia.id, { activo: !barberia.activo });
    await cargarBarberias();
  } catch (error) {
    console.error('Error cambiando estado:', error);
  }
};

const cerrarModal = () => {
  modalVisible.value = false;
  barberiaSeleccionada.value = null;
};

const obtenerIniciales = (nombre) => {
  if (!nombre) return '';
  const partes = nombre.split(' ');
  return partes.slice(0, 2).map(p => p[0]).join('').toUpperCase();
};

onMounted(() => {
  cargarBarberias();
  cargarPlanes();
});
</script>

<style scoped>
.view-container { padding: 2rem; }
.header-actions { display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem; }
.filtros-card { background: white; border-radius: 8px; padding: 1.5rem; margin-bottom: 1.5rem; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
.filtros-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1rem; }
.filtro-item label { display: block; font-size: 0.875rem; color: #666; margin-bottom: 0.5rem; }
.filtro-item input, .filtro-item select { width: 100%; padding: 0.5rem; border: 1px solid #ddd; border-radius: 4px; }

.grid-barberias { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.5rem; }
.card-barberia { background: white; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 4px rgba(0,0,0,0.1); transition: transform 0.2s; }
.card-barberia:hover { transform: translateY(-4px); box-shadow: 0 8px 16px rgba(0,0,0,0.1); }
.card-header { display: flex; justify-content: space-between; align-items: center; padding: 1.5rem; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); }
.avatar { width: 60px; height: 60px; border-radius: 50%; background: white; color: #667eea; display: flex; align-items: center; justify-content: center; font-weight: bold; font-size: 1.5rem; }
.card-body { padding: 1.5rem; }
.card-body h3 { margin: 0 0 0.5rem 0; color: #333; }
.texto-suave { color: #666; font-size: 0.875rem; margin: 0 0 1rem 0; }
.info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; margin-bottom: 1rem; }
.info-item { text-align: center; padding: 0.75rem; background: #f8f9fa; border-radius: 4px; }
.info-item label { display: block; font-size: 0.75rem; color: #666; margin-bottom: 0.25rem; }
.info-item span { font-weight: 600; color: #333; }
.plan-info { text-align: center; }
.card-actions { display: flex; justify-content: center; gap: 0.5rem; padding: 1rem; background: #f8f9fa; border-top: 1px solid #e9ecef; }

.badge { padding: 0.25rem 0.75rem; border-radius: 20px; font-size: 0.75rem; font-weight: 600; }
.badge-success { background: #d4edda; color: #155724; }
.badge-danger { background: #f8d7da; color: #721c24; }
.badge-plan { background: #667eea; color: white; }

.btn-icon { padding: 0.5rem; border: none; background: transparent; cursor: pointer; color: #666; border-radius: 4px; transition: all 0.2s; }
.btn-icon:hover { background: white; color: #667eea; }

.formulario { padding: 1rem 0; }
.formulario-grid { display: grid; grid-template-columns: repeat(2, 1fr); gap: 1rem; margin-bottom: 1.5rem; }
.formulario-item.full-width { grid-column: 1 / -1; }
.formulario-item label { display: block; font-size: 0.875rem; color: #666; margin-bottom: 0.5rem; }
.formulario-item input, .formulario-item select { width: 100%; padding: 0.75rem; border: 1px solid #ddd; border-radius: 4px; }
.formulario-acciones { display: flex; justify-content: flex-end; gap: 1rem; }

.btn { padding: 0.75rem 1.5rem; border: none; border-radius: 4px; cursor: pointer; font-weight: 600; transition: all 0.2s; }
.btn-primary { background: #667eea; color: white; }
.btn-primary:hover:not(:disabled) { background: #5a6fd6; }
.btn-secondary { background: #6c757d; color: white; }
.btn:disabled { opacity: 0.6; cursor: not-allowed; }

.detalles-contenido { padding: 1rem 0; }
.detalle-header { display: flex; align-items: center; gap: 1rem; margin-bottom: 2rem; padding-bottom: 1.5rem; border-bottom: 1px solid #e9ecef; }
.avatar-grande { width: 80px; height: 80px; border-radius: 50%; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; display: flex; align-items: center; justify-content: center; font-size: 2rem; font-weight: bold; }
.detalle-grid { display: grid; grid-template-columns: repeat(2, 1fr); gap: 1.5rem; }
.detalle-item label { display: block; font-size: 0.875rem; color: #666; margin-bottom: 0.25rem; }
.detalle-item p { margin: 0; font-weight: 500; }

.sin-datos { text-align: center; padding: 4rem; color: #666; }
.sin-datos i { font-size: 4rem; margin-bottom: 1rem; color: #ddd; }

@media (max-width: 768px) {
  .formulario-grid, .detalle-grid { grid-template-columns: 1fr; }
  .filtros-grid { grid-template-columns: 1fr; }
}
</style>
