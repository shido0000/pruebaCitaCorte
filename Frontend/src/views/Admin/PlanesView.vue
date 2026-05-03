<template>
  <div class="view-container">
    <div class="header-actions">
      <h1>Gestión de Planes de Suscripción</h1>
      <button @click="abrirModalCrear()" class="btn btn-primary">
        <i class="fas fa-plus"></i> Nuevo Plan
      </button>
    </div>

    <!-- Filtros -->
    <div class="filtros-card">
      <div class="filtro-group">
        <label>Tipo de Proveedor:</label>
        <select v-model="filtros.tipoProveedor" @change="cargarPlanes()">
          <option value="">Todos</option>
          <option value="Barbero">Barbero</option>
          <option value="Barberia">Barbería</option>
        </select>
      </div>
      <div class="filtro-group">
        <label>Estado:</label>
        <select v-model="filtros.estado" @change="cargarPlanes()">
          <option value="">Todos</option>
          <option value="Activo">Activo</option>
          <option value="Inactivo">Inactivo</option>
        </select>
      </div>
    </div>

    <!-- Tabla de Planes -->
    <div class="table-container" v-if="!cargando">
      <table class="tabla">
        <thead>
          <tr>
            <th>Nombre</th>
            <th>Tipo</th>
            <th>Precio</th>
            <th>Duración</th>
            <th>Límite Barberos</th>
            <th>Límite Reservas</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="plan in planes" :key="plan.id">
            <td>
              <strong>{{ plan.nombre }}</strong>
              <span v-if="plan.esPopular" class="badge badge-popular">Popular</span>
            </td>
            <td>{{ plan.tipoProveedor }}</td>
            <td>${{ plan.precio.toFixed(2) }}</td>
            <td>{{ plan.diasDuracion }} días</td>
            <td>{{ plan.limiteBarberos || '∞' }}</td>
            <td>{{ plan.limiteReservasDiarias || '∞' }}</td>
            <td>
              <span :class="['badge', plan.estado === 'Activo' ? 'badge-success' : 'badge-danger']">
                {{ plan.estado }}
              </span>
            </td>
            <td class="acciones">
              <button @click="verDetalles(plan)" class="btn-icon" title="Ver detalles">
                <i class="fas fa-eye"></i>
              </button>
              <button @click="editarPlan(plan)" class="btn-icon" title="Editar">
                <i class="fas fa-edit"></i>
              </button>
              <button @click="toggleEstado(plan)" class="btn-icon" title="Cambiar estado">
                <i class="fas" :class="plan.estado === 'Activo' ? 'fa-toggle-on' : 'fa-toggle-off'"></i>
              </button>
              <button @click="eliminarPlan(plan)" class="btn-icon btn-danger" title="Eliminar">
                <i class="fas fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <p v-if="planes.length === 0" class="no-data">No hay planes registrados</p>
    </div>

    <div v-else class="loading">
      <i class="fas fa-spinner fa-spin"></i> Cargando planes...
    </div>

    <!-- Modal Crear/Editar -->
    <Modal 
      v-if="mostrarModal"
      :titulo="modalEsEdicion ? 'Editar Plan' : 'Nuevo Plan'"
      @cerrar="cerrarModal()"
    >
      <form @submit.prevent="guardarPlan()" class="form">
        <div class="form-row">
          <div class="form-group">
            <label>Nombre del Plan *</label>
            <input v-model="formulario.nombre" type="text" required class="input" />
          </div>
          <div class="form-group">
            <label>Tipo de Proveedor *</label>
            <select v-model="formulario.tipoProveedor" required class="input">
              <option value="Barbero">Barbero</option>
              <option value="Barberia">Barbería</option>
            </select>
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Precio Mensual *</label>
            <input v-model.number="formulario.precio" type="number" step="0.01" min="0" required class="input" />
          </div>
          <div class="form-group">
            <label>Duración (días) *</label>
            <input v-model.number="formulario.diasDuracion" type="number" min="1" required class="input" />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Límite de Barberos</label>
            <input v-model.number="formulario.limiteBarberos" type="number" min="0" class="input" placeholder="Ilimitado" />
          </div>
          <div class="form-group">
            <label>Límite Reservas Diarias</label>
            <input v-model.number="formulario.limiteReservasDiarias" type="number" min="0" class="input" placeholder="Ilimitado" />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Límite de Productos</label>
            <input v-model.number="formulario.limiteProductos" type="number" min="0" class="input" placeholder="Ilimitado" />
          </div>
          <div class="form-group">
            <label>Límite de Servicios</label>
            <input v-model.number="formulario.limiteServicios" type="number" min="0" class="input" placeholder="Ilimitado" />
          </div>
        </div>

        <div class="form-group checkbox-group">
          <label>
            <input v-model="formulario.esPopular" type="checkbox" />
            Marcar como Popular
          </label>
        </div>

        <div class="form-group">
          <label>Descripción</label>
          <textarea v-model="formulario.descripcion" rows="3" class="input"></textarea>
        </div>

        <div class="form-actions">
          <button type="button" @click="cerrarModal()" class="btn btn-secondary">Cancelar</button>
          <button type="submit" :disabled="guardando" class="btn btn-primary">
            <i v-if="guardando" class="fas fa-spinner fa-spin"></i>
            {{ guardando ? 'Guardando...' : 'Guardar' }}
          </button>
        </div>
      </form>
    </Modal>

    <!-- Modal Detalles -->
    <Modal 
      v-if="mostrarModalDetalles"
      titulo="Detalles del Plan"
      @cerrar="mostrarModalDetalles = false"
    >
      <div v-if="planSeleccionado" class="detalles-plan">
        <h3>{{ planSeleccionado.nombre }}</h3>
        <span :class="['badge', planSeleccionado.esPopular ? 'badge-popular' : 'badge-info']">
          {{ planSeleccionado.tipoProveedor }}
        </span>
        <div class="info-grid">
          <div class="info-item">
            <label>Precio:</label>
            <span>${{ planSeleccionado.precio.toFixed(2) }}</span>
          </div>
          <div class="info-item">
            <label>Duración:</label>
            <span>{{ planSeleccionado.diasDuracion }} días</span>
          </div>
          <div class="info-item">
            <label>Estado:</label>
            <span :class="['badge', planSeleccionado.estado === 'Activo' ? 'badge-success' : 'badge-danger']">
              {{ planSeleccionado.estado }}
            </span>
          </div>
        </div>
        <h4>Límites</h4>
        <ul class="lista-limites">
          <li>Barberos: {{ planSeleccionado.limiteBarberos || 'Ilimitado' }}</li>
          <li>Reservas diarias: {{ planSeleccionado.limiteReservasDiarias || 'Ilimitado' }}</li>
          <li>Productos: {{ planSeleccionado.limiteProductos || 'Ilimitado' }}</li>
          <li>Servicios: {{ planSeleccionado.limiteServicios || 'Ilimitado' }}</li>
        </ul>
        <p v-if="planSeleccionado.descripcion" class="descripcion">
          {{ planSeleccionado.descripcion }}
        </p>
      </div>
    </Modal>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue';
import { usePlanStore } from '@/stores/planStore';
import Modal from '@/components/ui/Modal.vue';

const planStore = usePlanStore();

// Estado
const cargando = ref(false);
const guardando = ref(false);
const mostrarModal = ref(false);
const mostrarModalDetalles = ref(false);
const modalEsEdicion = ref(false);
const planSeleccionado = ref(null);

// Filtros
const filtros = reactive({
  tipoProveedor: '',
  estado: ''
});

// Formulario
const formulario = reactive({
  id: null,
  nombre: '',
  tipoProveedor: 'Barbero',
  precio: 0,
  diasDuracion: 30,
  limiteBarberos: null,
  limiteReservasDiarias: null,
  limiteProductos: null,
  limiteServicios: null,
  esPopular: false,
  descripcion: '',
  estado: 'Activo'
});

// Computed
const planes = ref([]);

// Methods
const cargarPlanes = async () => {
  cargando.value = true;
  try {
    const tipo = filtros.tipoProveedor || null;
    await planStore.cargarPlanes(tipo);
    let resultado = planStore.planes;
    
    if (filtros.estado) {
      resultado = resultado.filter(p => p.estado === filtros.estado);
    }
    
    planes.value = resultado;
  } catch (error) {
    alert('Error al cargar planes: ' + error.message);
  } finally {
    cargando.value = false;
  }
};

const abrirModalCrear = () => {
  modalEsEdicion.value = false;
  resetearFormulario();
  mostrarModal.value = true;
};

const editarPlan = (plan) => {
  modalEsEdicion.value = true;
  planSeleccionado.value = plan;
  
  formulario.id = plan.id;
  formulario.nombre = plan.nombre;
  formulario.tipoProveedor = plan.tipoProveedor;
  formulario.precio = plan.precio;
  formulario.diasDuracion = plan.diasDuracion;
  formulario.limiteBarberos = plan.limiteBarberos;
  formulario.limiteReservasDiarias = plan.limiteReservasDiarias;
  formulario.limiteProductos = plan.limiteProductos;
  formulario.limiteServicios = plan.limiteServicios;
  formulario.esPopular = plan.esPopular;
  formulario.descripcion = plan.descripcion || '';
  formulario.estado = plan.estado;
  
  mostrarModal.value = true;
};

const verDetalles = (plan) => {
  planSeleccionado.value = plan;
  mostrarModalDetalles.value = true;
};

const guardarPlan = async () => {
  guardando.value = true;
  try {
    const datos = { ...formulario };
    
    if (modalEsEdicion.value) {
      await planStore.actualizarPlan(formulario.id, datos);
      alert('Plan actualizado correctamente');
    } else {
      await planStore.crearPlan(datos);
      alert('Plan creado correctamente');
    }
    
    cerrarModal();
    cargarPlanes();
  } catch (error) {
    alert('Error al guardar: ' + error.message);
  } finally {
    guardando.value = false;
  }
};

const toggleEstado = async (plan) => {
  if (!confirm(`¿Está seguro de ${plan.estado === 'Activo' ? 'desactivar' : 'activar'} el plan "${plan.nombre}"?`)) {
    return;
  }
  
  try {
    const nuevoEstado = plan.estado === 'Activo' ? 'Inactivo' : 'Activo';
    await planStore.actualizarPlan(plan.id, { ...plan, estado: nuevoEstado });
    cargarPlanes();
    alert('Estado actualizado correctamente');
  } catch (error) {
    alert('Error al actualizar estado: ' + error.message);
  }
};

const eliminarPlan = async (plan) => {
  if (!confirm(`¿Está seguro de eliminar el plan "${plan.nombre}"? Esta acción no se puede deshacer.`)) {
    return;
  }
  
  try {
    await planStore.eliminarPlan(plan.id);
    cargarPlanes();
    alert('Plan eliminado correctamente');
  } catch (error) {
    alert('Error al eliminar: ' + error.message);
  }
};

const cerrarModal = () => {
  mostrarModal.value = false;
  planSeleccionado.value = null;
  resetearFormulario();
};

const resetearFormulario = () => {
  formulario.id = null;
  formulario.nombre = '';
  formulario.tipoProveedor = 'Barbero';
  formulario.precio = 0;
  formulario.diasDuracion = 30;
  formulario.limiteBarberos = null;
  formulario.limiteReservasDiarias = null;
  formulario.limiteProductos = null;
  formulario.limiteServicios = null;
  formulario.esPopular = false;
  formulario.descripcion = '';
  formulario.estado = 'Activo';
};

onMounted(() => {
  cargarPlanes();
});
</script>

<style scoped>
.header-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.filtros-card {
  background: #f8f9fa;
  padding: 15px;
  border-radius: 8px;
  margin-bottom: 20px;
  display: flex;
  gap: 20px;
}

.filtro-group {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.filtro-group label {
  font-weight: 600;
  font-size: 14px;
}

.table-container {
  overflow-x: auto;
}

.tabla {
  width: 100%;
  border-collapse: collapse;
  background: white;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  border-radius: 8px;
  overflow: hidden;
}

.tabla th,
.tabla td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid #e0e0e0;
}

.tabla th {
  background: #2c3e50;
  color: white;
  font-weight: 600;
}

.tabla tr:hover {
  background: #f8f9fa;
}

.acciones {
  display: flex;
  gap: 8px;
}

.btn-icon {
  background: none;
  border: none;
  cursor: pointer;
  padding: 5px 8px;
  border-radius: 4px;
  transition: background 0.2s;
}

.btn-icon:hover {
  background: #e0e0e0;
}

.btn-icon.btn-danger:hover {
  background: #ffebee;
  color: #dc3545;
}

.badge {
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 600;
}

.badge-success {
  background: #d4edda;
  color: #155724;
}

.badge-danger {
  background: #f8d7da;
  color: #721c24;
}

.badge-popular {
  background: #fff3cd;
  color: #856404;
  margin-left: 8px;
}

.badge-info {
  background: #d1ecf1;
  color: #0c5460;
}

.loading {
  text-align: center;
  padding: 40px;
  font-size: 16px;
  color: #666;
}

.no-data {
  text-align: center;
  padding: 40px;
  color: #666;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 15px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.form-group label {
  font-weight: 600;
  font-size: 14px;
}

.input {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.checkbox-group label {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}

.btn {
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 8px;
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

.btn-secondary:hover {
  background: #545b62;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.detalles-plan h3 {
  margin-bottom: 10px;
  color: #2c3e50;
}

.info-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 15px;
  margin: 20px 0;
  padding: 15px;
  background: #f8f9fa;
  border-radius: 8px;
}

.info-item {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.info-item label {
  font-weight: 600;
  font-size: 12px;
  color: #666;
}

.lista-limites {
  list-style: none;
  padding: 0;
  margin: 15px 0;
}

.lista-limites li {
  padding: 8px 0;
  border-bottom: 1px solid #e0e0e0;
}

.lista-limites li:last-child {
  border-bottom: none;
}

.descripcion {
  margin-top: 20px;
  padding: 15px;
  background: #f8f9fa;
  border-radius: 8px;
  font-style: italic;
}
</style>
