<template>
  <div class="view-container">
    <div class="header-actions">
      <h1>Gestión de Barberos</h1>
      <button @click="abrirModalCrear()" class="btn btn-primary">
        <i class="fas fa-plus"></i> Nuevo Barbero
      </button>
    </div>

    <!-- Filtros -->
    <div class="filtros-card">
      <div class="filtros-grid">
        <div class="filtro-item">
          <label>Buscar</label>
          <input 
            v-model="filtros.busqueda" 
            placeholder="Nombre, email o teléfono..." 
            @input="cargarBarberos"
          />
        </div>
        <div class="filtro-item">
          <label>Estado</label>
          <select v-model="filtros.estado" @change="cargarBarberos">
            <option value="">Todos</option>
            <option value="Activo">Activos</option>
            <option value="Inactivo">Inactivos</option>
            <option value="Suspendido">Suspendidos</option>
          </select>
        </div>
        <div class="filtro-item">
          <label>Plan</label>
          <select v-model="filtros.plan" @change="cargarBarberos">
            <option value="">Todos</option>
            <option value="Free">Free</option>
            <option value="Popular">Popular</option>
            <option value="Premium">Premium</option>
          </select>
        </div>
        <div class="filtro-item">
          <label>Ordenar por</label>
          <select v-model="filtros.orden" @change="cargarBarberos">
            <option value="nombre">Nombre</option>
            <option value="fechaRegistro">Fecha Registro</option>
            <option value="totalReservas">Total Reservas</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Tabla de Barberos -->
    <div class="tabla-card">
      <div class="tabla-responsive">
        <table>
          <thead>
            <tr>
              <th>Barbero</th>
              <th>Email</th>
              <th>Teléfono</th>
              <th>Plan</th>
              <th>Estado</th>
              <th>Reservas</th>
              <th>Calificación</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="barbero in barberos" :key="barbero.id">
              <td>
                <div class="usuario-info">
                  <div class="avatar">{{ obtenerIniciales(barbero.nombre) }}</div>
                  <div>
                    <div class="nombre">{{ barbero.nombre }}</div>
                    <small class="texto-suave">{{ barbero.usuario }}</small>
                  </div>
                </div>
              </td>
              <td>{{ barbero.email }}</td>
              <td>{{ barbero.telefono || 'N/A' }}</td>
              <td>
                <span :class="['badge', 'badge-' + obtenerClasePlan(barbero.planSuscripcion?.tipoPlan)]">
                  {{ barbero.planSuscripcion?.tipoPlan || 'Sin Plan' }}
                </span>
              </td>
              <td>
                <span :class="['badge', 'badge-' + (barbero.activo ? 'success' : 'danger')]">
                  {{ barbero.activo ? 'Activo' : 'Inactivo' }}
                </span>
              </td>
              <td>{{ barbero.totalReservas || 0 }}</td>
              <td>
                <div class="calificacion">
                  <i class="fas fa-star"></i>
                  <span>{{ barbero.calificacionPromedio?.toFixed(1) || '0.0' }}</span>
                </div>
              </td>
              <td>
                <div class="acciones">
                  <button @click="verDetalles(barbero)" class="btn-icon" title="Ver detalles">
                    <i class="fas fa-eye"></i>
                  </button>
                  <button @click="editarBarbero(barbero)" class="btn-icon" title="Editar">
                    <i class="fas fa-edit"></i>
                  </button>
                  <button @click="toggleEstado(barbero)" class="btn-icon" :title="barbero.activo ? 'Desactivar' : 'Activar'">
                    <i :class="barbero.activo ? 'fas fa-lock' : 'fas fa-unlock'"></i>
                  </button>
                </div>
              </td>
            </tr>
            <tr v-if="barberos.length === 0 && !cargando">
              <td colspan="8" class="sin-datos">No hay barberos registrados</td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Paginación -->
      <div class="paginacion" v-if="totalPaginas > 1">
        <button @click="cambiarPagina(pagina - 1)" :disabled="pagina === 1" class="btn btn-secondary">
          <i class="fas fa-chevron-left"></i>
        </button>
        <span class="pagina-info">Página {{ pagina }} de {{ totalPaginas }}</span>
        <button @click="cambiarPagina(pagina + 1)" :disabled="pagina === totalPaginas" class="btn btn-secondary">
          <i class="fas fa-chevron-right"></i>
        </button>
      </div>
    </div>

    <!-- Modal Crear/Editar -->
    <Modal 
      :visible="modalVisible" 
      :titulo="modoEdicion ? 'Editar Barbero' : 'Nuevo Barbero'"
      @cerrar="cerrarModal"
    >
      <form @submit.prevent="guardarBarbero" class="formulario">
        <div class="formulario-grid">
          <div class="formulario-item">
            <label>Nombre Completo *</label>
            <input v-model="formulario.nombre" required type="text" />
          </div>
          <div class="formulario-item">
            <label>Usuario *</label>
            <input v-model="formulario.usuario" required type="text" />
          </div>
          <div class="formulario-item">
            <label>Email *</label>
            <input v-model="formulario.email" required type="email" />
          </div>
          <div class="formulario-item">
            <label>Teléfono</label>
            <input v-model="formulario.telefono" type="tel" />
          </div>
          <div class="formulario-item" v-if="!modoEdicion">
            <label>Contraseña *</label>
            <input v-model="formulario.password" required type="password" />
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
      titulo="Detalles del Barbero"
      @cerrar="modalDetallesVisible = false"
    >
      <div v-if="barberoSeleccionado" class="detalles-contenido">
        <div class="detalle-header">
          <div class="avatar-grande">{{ obtenerIniciales(barberoSeleccionado.nombre) }}</div>
          <div>
            <h3>{{ barberoSeleccionado.nombre }}</h3>
            <p class="texto-suave">{{ barberoSeleccionado.usuario }}</p>
          </div>
        </div>
        
        <div class="detalle-grid">
          <div class="detalle-item">
            <label>Email</label>
            <p>{{ barberoSeleccionado.email }}</p>
          </div>
          <div class="detalle-item">
            <label>Teléfono</label>
            <p>{{ barberoSeleccionado.telefono || 'N/A' }}</p>
          </div>
          <div class="detalle-item">
            <label>Plan Actual</label>
            <p>{{ barberoSeleccionado.planSuscripcion?.nombre || 'Sin Plan' }}</p>
          </div>
          <div class="detalle-item">
            <label>Estado</label>
            <p><span :class="['badge', 'badge-' + (barberoSeleccionado.activo ? 'success' : 'danger')]">
              {{ barberoSeleccionado.activo ? 'Activo' : 'Inactivo' }}
            </span></p>
          </div>
          <div class="detalle-item">
            <label>Total Reservas</label>
            <p>{{ barberoSeleccionado.totalReservas || 0 }}</p>
          </div>
          <div class="detalle-item">
            <label>Calificación</label>
            <p><i class="fas fa-star"></i> {{ barberoSeleccionado.calificacionPromedio?.toFixed(1) || '0.0' }}</p>
          </div>
          <div class="detalle-item">
            <label>Fecha Registro</label>
            <p>{{ formatearFecha(barberoSeleccionado.fechaRegistro) }}</p>
          </div>
        </div>
      </div>
    </Modal>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useBarberoStore } from '@/stores/barberoStore';
import { usePlanStore } from '@/stores/planStore';
import Modal from '@/components/ui/Modal.vue';

const barberoStore = useBarberoStore();
const planStore = usePlanStore();

// Estado
const barberos = ref([]);
const planes = ref([]);
const cargando = ref(false);
const filtros = ref({
  busqueda: '',
  estado: '',
  plan: '',
  orden: 'nombre'
});
const pagina = ref(1);
const totalPaginas = ref(1);

// Modales
const modalVisible = ref(false);
const modalDetallesVisible = ref(false);
const modoEdicion = ref(false);
const barberoSeleccionado = ref(null);

// Formulario
const formulario = ref({
  nombre: '',
  usuario: '',
  email: '',
  telefono: '',
  password: '',
  planId: '',
  activo: true
});

// Computed
const totalRegistros = computed(() => barberos.value.length);

// Métodos
const cargarBarberos = async () => {
  cargando.value = true;
  try {
    await barberoStore.obtenerTodos(filtros.value);
    barberos.value = barberoStore.barberos;
    totalPaginas.value = Math.ceil(barberos.value.length / 10);
  } catch (error) {
    console.error('Error cargando barberos:', error);
  } finally {
    cargando.value = false;
  }
};

const cargarPlanes = async () => {
  try {
    await planStore.cargarPlanes({ tipo: 'Barbero' });
    planes.value = planStore.planes;
  } catch (error) {
    console.error('Error cargando planes:', error);
  }
};

const abrirModalCrear = () => {
  modoEdicion.value = false;
  formulario.value = {
    nombre: '',
    usuario: '',
    email: '',
    telefono: '',
    password: '',
    planId: '',
    activo: true
  };
  modalVisible.value = true;
};

const editarBarbero = (barbero) => {
  modoEdicion.value = true;
  barberoSeleccionado.value = barbero;
  formulario.value = {
    nombre: barbero.nombre,
    usuario: barbero.usuario,
    email: barbero.email,
    telefono: barbero.telefono || '',
    password: '',
    planId: barbero.planSuscripcionId || '',
    activo: barbero.activo
  };
  modalVisible.value = true;
};

const guardarBarbero = async () => {
  cargando.value = true;
  try {
    if (modoEdicion.value) {
      await barberoStore.actualizar(barberoSeleccionado.value.id, formulario.value);
    } else {
      await barberoStore.crear(formulario.value);
    }
    cerrarModal();
    await cargarBarberos();
  } catch (error) {
    console.error('Error guardando barbero:', error);
    alert(error.message || 'Error al guardar');
  } finally {
    cargando.value = false;
  }
};

const verDetalles = (barbero) => {
  barberoSeleccionado.value = barbero;
  modalDetallesVisible.value = true;
};

const toggleEstado = async (barbero) => {
  if (!confirm(`¿Está seguro de ${barbero.activo ? 'desactivar' : 'activar'} este barbero?`)) return;
  
  try {
    await barberoStore.actualizar(barbero.id, { activo: !barbero.activo });
    await cargarBarberos();
  } catch (error) {
    console.error('Error cambiando estado:', error);
  }
};

const cerrarModal = () => {
  modalVisible.value = false;
  barberoSeleccionado.value = null;
};

const cambiarPagina = (nuevaPagina) => {
  if (nuevaPagina >= 1 && nuevaPagina <= totalPaginas.value) {
    pagina.value = nuevaPagina;
    cargarBarberos();
  }
};

const obtenerIniciales = (nombre) => {
  if (!nombre) return '';
  const partes = nombre.split(' ');
  return partes.slice(0, 2).map(p => p[0]).join('').toUpperCase();
};

const obtenerClasePlan = (tipo) => {
  switch(tipo) {
    case 'Free': return 'info';
    case 'Popular': return 'warning';
    case 'Premium': return 'success';
    default: return 'secondary';
  }
};

const formatearFecha = (fecha) => {
  if (!fecha) return 'N/A';
  return new Date(fecha).toLocaleDateString('es-ES');
};

onMounted(() => {
  cargarBarberos();
  cargarPlanes();
});
</script>

<style scoped>
.view-container {
  padding: 2rem;
}

.header-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.filtros-card {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.filtros-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
}

.filtro-item label {
  display: block;
  font-size: 0.875rem;
  color: #666;
  margin-bottom: 0.5rem;
}

.filtro-item input,
.filtro-item select {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.tabla-card {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.tabla-responsive {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead th {
  text-align: left;
  padding: 1rem;
  background: #f8f9fa;
  font-weight: 600;
  color: #333;
  border-bottom: 2px solid #e9ecef;
}

tbody td {
  padding: 1rem;
  border-bottom: 1px solid #e9ecef;
}

.usuario-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: #007bff;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
}

.nombre {
  font-weight: 600;
  color: #333;
}

.texto-suave {
  color: #666;
  font-size: 0.875rem;
}

.badge {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.75rem;
  font-weight: 600;
}

.badge-success { background: #d4edda; color: #155724; }
.badge-danger { background: #f8d7da; color: #721c24; }
.badge-warning { background: #fff3cd; color: #856404; }
.badge-info { background: #d1ecf1; color: #0c5460; }
.badge-secondary { background: #e9ecef; color: #6c757d; }

.calificacion {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  color: #ffc107;
}

.acciones {
  display: flex;
  gap: 0.5rem;
}

.btn-icon {
  padding: 0.5rem;
  border: none;
  background: transparent;
  cursor: pointer;
  color: #666;
  border-radius: 4px;
  transition: all 0.2s;
}

.btn-icon:hover {
  background: #f8f9fa;
  color: #007bff;
}

.paginacion {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  margin-top: 1.5rem;
}

.pagina-info {
  color: #666;
}

.formulario {
  padding: 1rem 0;
}

.formulario-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.formulario-item label {
  display: block;
  font-size: 0.875rem;
  color: #666;
  margin-bottom: 0.5rem;
}

.formulario-item input,
.formulario-item select {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.formulario-acciones {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
}

.btn {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
}

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #0056b3;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.detalles-contenido {
  padding: 1rem 0;
}

.detalle-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 2rem;
  padding-bottom: 1.5rem;
  border-bottom: 1px solid #e9ecef;
}

.avatar-grande {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  background: #007bff;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  font-weight: bold;
}

.detalle-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1.5rem;
}

.detalle-item label {
  display: block;
  font-size: 0.875rem;
  color: #666;
  margin-bottom: 0.25rem;
}

.detalle-item p {
  margin: 0;
  font-weight: 500;
}

.sin-datos {
  text-align: center;
  padding: 3rem;
  color: #666;
}

@media (max-width: 768px) {
  .formulario-grid,
  .detalle-grid {
    grid-template-columns: 1fr;
  }
  
  .filtros-grid {
    grid-template-columns: 1fr;
  }
}
</style>
