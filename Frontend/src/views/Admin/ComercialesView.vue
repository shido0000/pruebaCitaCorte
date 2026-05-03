<template>
  <div class="view-container">
    <div class="header-actions">
      <h1>Gestión de Comerciales</h1>
      <button @click="abrirModalCrear()" class="btn btn-primary">
        <i class="fas fa-plus"></i> Nuevo Comercial
      </button>
    </div>

    <!-- Filtros -->
    <div class="filtros-card">
      <div class="filtros-grid">
        <div class="filtro-item">
          <label>Buscar</label>
          <input v-model="filtros.busqueda" placeholder="Nombre, email..." @input="cargarComerciales" />
        </div>
        <div class="filtro-item">
          <label>Estado</label>
          <select v-model="filtros.estado" @change="cargarComerciales">
            <option value="">Todos</option>
            <option value="Activo">Activos</option>
            <option value="Inactivo">Inactivos</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Tabla -->
    <div class="tabla-card">
      <table>
        <thead>
          <tr>
            <th>Comercial</th>
            <th>Email</th>
            <th>Teléfono</th>
            <th>Barberos Asignados</th>
            <th>Barberías Asignadas</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="comercial in comerciales" :key="comercial.id">
            <td>
              <div class="usuario-info">
                <div class="avatar">{{ obtenerIniciales(comercial.nombre) }}</div>
                <div>
                  <div class="nombre">{{ comercial.nombre }}</div>
                  <small>{{ comercial.usuario }}</small>
                </div>
              </div>
            </td>
            <td>{{ comercial.email }}</td>
            <td>{{ comercial.telefono || 'N/A' }}</td>
            <td>{{ comercial.totalBarberos || 0 }}</td>
            <td>{{ comercial.totalBarberias || 0 }}</td>
            <td>
              <span :class="['badge', 'badge-' + (comercial.activo ? 'success' : 'danger')]">
                {{ comercial.activo ? 'Activo' : 'Inactivo' }}
              </span>
            </td>
            <td>
              <div class="acciones">
                <button @click="verDetalles(comercial)" class="btn-icon"><i class="fas fa-eye"></i></button>
                <button @click="editarComercial(comercial)" class="btn-icon"><i class="fas fa-edit"></i></button>
                <button @click="toggleEstado(comercial)" class="btn-icon"><i :class="comercial.activo ? 'fas fa-lock' : 'fas fa-unlock'"></i></button>
              </div>
            </td>
          </tr>
          <tr v-if="comerciales.length === 0 && !cargando">
            <td colspan="7" class="sin-datos">No hay comerciales registrados</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal -->
    <Modal :visible="modalVisible" :titulo="modoEdicion ? 'Editar Comercial' : 'Nuevo Comercial'" @cerrar="cerrarModal">
      <form @submit.prevent="guardarComercial" class="formulario">
        <div class="formulario-grid">
          <div class="formulario-item"><label>Nombre *</label><input v-model="formulario.nombre" required /></div>
          <div class="formulario-item"><label>Usuario *</label><input v-model="formulario.usuario" required /></div>
          <div class="formulario-item"><label>Email *</label><input v-model="formulario.email" required type="email" /></div>
          <div class="formulario-item"><label>Teléfono</label><input v-model="formulario.telefono" /></div>
          <div class="formulario-item" v-if="!modoEdicion"><label>Contraseña *</label><input v-model="formulario.password" required type="password" /></div>
          <div class="formulario-item"><label>Estado</label><select v-model="formulario.activo"><option :value="true">Activo</option><option :value="false">Inactivo</option></select></div>
        </div>
        <div class="formulario-acciones">
          <button type="button" @click="cerrarModal" class="btn btn-secondary">Cancelar</button>
          <button type="submit" :disabled="cargando" class="btn btn-primary">{{ cargando ? 'Guardando...' : (modoEdicion ? 'Actualizar' : 'Crear') }}</button>
        </div>
      </form>
    </Modal>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import Modal from '@/components/ui/Modal.vue';

const comerciales = ref([]);
const cargando = ref(false);
const filtros = ref({ busqueda: '', estado: '' });
const modalVisible = ref(false);
const modoEdicion = ref(false);
const comercialSeleccionado = ref(null);
const formulario = ref({ nombre: '', usuario: '', email: '', telefono: '', password: '', activo: true });

const cargarComerciales = async () => {
  cargando.value = true;
  try {
    // Simulación - reemplazar con llamada real al store
    comerciales.value = [];
  } catch (error) {
    console.error('Error:', error);
  } finally {
    cargando.value = false;
  }
};

const abrirModalCrear = () => {
  modoEdicion.value = false;
  formulario.value = { nombre: '', usuario: '', email: '', telefono: '', password: '', activo: true };
  modalVisible.value = true;
};

const editarComercial = (comercial) => {
  modoEdicion.value = true;
  comercialSeleccionado.value = comercial;
  formulario.value = { ...comercial, password: '' };
  modalVisible.value = true;
};

const guardarComercial = async () => {
  cargando.value = true;
  try {
    await new Promise(r => setTimeout(r, 500));
    cerrarModal();
    await cargarComerciales();
  } catch (error) {
    alert('Error al guardar');
  } finally {
    cargando.value = false;
  }
};

const verDetalles = (comercial) => {
  comercialSeleccionado.value = comercial;
  alert(`Detalles de ${comercial.nombre}`);
};

const toggleEstado = async (comercial) => {
  if (!confirm(`¿${comercial.activo ? 'Desactivar' : 'Activar'}?`)) return;
  await cargarComerciales();
};

const cerrarModal = () => { modalVisible.value = false; comercialSeleccionado.value = null; };
const obtenerIniciales = (nombre) => nombre ? nombre.split(' ').slice(0,2).map(p=>p[0]).join('').toUpperCase() : '';

onMounted(() => { cargarComerciales(); });
</script>

<style scoped>
.view-container { padding: 2rem; }
.header-actions { display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem; }
.filtros-card { background: white; border-radius: 8px; padding: 1.5rem; margin-bottom: 1.5rem; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
.filtros-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1rem; }
.filtro-item label { display: block; font-size: 0.875rem; color: #666; margin-bottom: 0.5rem; }
.filtro-item input, .filtro-item select { width: 100%; padding: 0.5rem; border: 1px solid #ddd; border-radius: 4px; }
.tabla-card { background: white; border-radius: 8px; padding: 1.5rem; box-shadow: 0 2px 4px rgba(0,0,0,0.1); overflow-x: auto; }
table { width: 100%; border-collapse: collapse; }
thead th { text-align: left; padding: 1rem; background: #f8f9fa; font-weight: 600; border-bottom: 2px solid #e9ecef; }
tbody td { padding: 1rem; border-bottom: 1px solid #e9ecef; }
.usuario-info { display: flex; align-items: center; gap: 0.75rem; }
.avatar { width: 40px; height: 40px; border-radius: 50%; background: #28a745; color: white; display: flex; align-items: center; justify-content: center; font-weight: bold; }
.nombre { font-weight: 600; }
.badge { padding: 0.25rem 0.75rem; border-radius: 20px; font-size: 0.75rem; font-weight: 600; }
.badge-success { background: #d4edda; color: #155724; }
.badge-danger { background: #f8d7da; color: #721c24; }
.acciones { display: flex; gap: 0.5rem; }
.btn-icon { padding: 0.5rem; border: none; background: transparent; cursor: pointer; color: #666; border-radius: 4px; }
.btn-icon:hover { background: #f8f9fa; color: #28a745; }
.formulario { padding: 1rem 0; }
.formulario-grid { display: grid; grid-template-columns: repeat(2, 1fr); gap: 1rem; margin-bottom: 1.5rem; }
.formulario-item label { display: block; font-size: 0.875rem; color: #666; margin-bottom: 0.5rem; }
.formulario-item input, .formulario-item select { width: 100%; padding: 0.75rem; border: 1px solid #ddd; border-radius: 4px; }
.formulario-acciones { display: flex; justify-content: flex-end; gap: 1rem; }
.btn { padding: 0.75rem 1.5rem; border: none; border-radius: 4px; cursor: pointer; font-weight: 600; }
.btn-primary { background: #28a745; color: white; }
.btn-secondary { background: #6c757d; color: white; }
.sin-datos { text-align: center; padding: 3rem; color: #666; }
</style>
