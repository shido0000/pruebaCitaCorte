<template>
  <div class="view-container">
    <h1>Gestión de Solicitudes de Afiliación</h1>
    
    <!-- Filtros -->
    <div class="filtros-card">
      <div class="filtros-grid">
        <div class="filtro-item">
          <label>Buscar</label>
          <input v-model="filtros.busqueda" placeholder="Barbero o barbería..." @input="cargarSolicitudes" />
        </div>
        <div class="filtro-item">
          <label>Estado</label>
          <select v-model="filtros.estado" @change="cargarSolicitudes">
            <option value="">Todos</option>
            <option value="Pendiente">Pendientes</option>
            <option value="Aprobada">Aprobadas</option>
            <option value="Rechazada">Rechazadas</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Tabla -->
    <div class="tabla-card">
      <table>
        <thead>
          <tr>
            <th>Barbero</th>
            <th>Barbería</th>
            <th>Fecha Solicitud</th>
            <th>Estado</th>
            <th>Motivo</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="solicitud in solicitudes" :key="solicitud.id">
            <td>{{ solicitud.barberoNombre }}</td>
            <td>{{ solicitud.barberiaNombre }}</td>
            <td>{{ formatearFecha(solicitud.fechaSolicitud) }}</td>
            <td>
              <span :class="['badge', getBadgeClass(solicitud.estado)]">{{ solicitud.estado }}</span>
            </td>
            <td>{{ solicitud.motivo || '-' }}</td>
            <td>
              <div class="acciones" v-if="solicitud.estado === 'Pendiente'">
                <button @click="aprobar(solicitud)" class="btn btn-success btn-sm"><i class="fas fa-check"></i> Aprobar</button>
                <button @click="rechazar(solicitud)" class="btn btn-danger btn-sm"><i class="fas fa-times"></i> Rechazar</button>
              </div>
              <span v-else class="texto-suave">Procesada</span>
            </td>
          </tr>
          <tr v-if="solicitudes.length === 0 && !cargando">
            <td colspan="6" class="sin-datos">No hay solicitudes</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal Rechazo -->
    <Modal :visible="modalRechazoVisible" titulo="Rechazar Solicitud" @cerrar="modalRechazoVisible = false">
      <form @submit.prevent="confirmarRechazo">
        <div class="formulario-item">
          <label>Motivo del rechazo *</label>
          <textarea v-model="motivoRechazo" rows="4" required placeholder="Explique el motivo..."></textarea>
        </div>
        <div class="formulario-acciones">
          <button type="button" @click="modalRechazoVisible = false" class="btn btn-secondary">Cancelar</button>
          <button type="submit" class="btn btn-danger">Confirmar Rechazo</button>
        </div>
      </form>
    </Modal>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useAfiliacionStore } from '@/stores/afiliacionStore';
import Modal from '@/components/ui/Modal.vue';

const afiliacionStore = useAfiliacionStore();
const solicitudes = ref([]);
const cargando = ref(false);
const filtros = ref({ busqueda: '', estado: 'Pendiente' });
const modalRechazoVisible = ref(false);
const solicitudSeleccionada = ref(null);
const motivoRechazo = ref('');

const cargarSolicitudes = async () => {
  cargando.value = true;
  try {
    await afiliacionStore.obtenerSolicitudes(filtros.value);
    solicitudes.value = afiliacionStore.solicitudes;
  } catch (error) {
    console.error('Error:', error);
  } finally {
    cargando.value = false;
  }
};

const aprobar = async (solicitud) => {
  if (!confirm('¿Aprobar esta solicitud?')) return;
  try {
    await afiliacionStore.aprobarSolicitud(solicitud.id);
    await cargarSolicitudes();
  } catch (error) {
    alert('Error al aprobar');
  }
};

const rechazar = (solicitud) => {
  solicitudSeleccionada.value = solicitud;
  motivoRechazo.value = '';
  modalRechazoVisible.value = true;
};

const confirmarRechazo = async () => {
  try {
    await afiliacionStore.rechazarSolicitud(solicitudSeleccionada.value.id, motivoRechazo.value);
    modalRechazoVisible.value = false;
    await cargarSolicitudes();
  } catch (error) {
    alert('Error al rechazar');
  }
};

const getBadgeClass = (estado) => {
  const clases = { Pendiente: 'warning', Aprobada: 'success', Rechazada: 'danger' };
  return clases[estado] || 'secondary';
};

const formatearFecha = (fecha) => new Date(fecha).toLocaleDateString('es-ES');

onMounted(() => { cargarSolicitudes(); });
</script>

<style scoped>
.view-container { padding: 2rem; }
.filtros-card { background: white; border-radius: 8px; padding: 1.5rem; margin-bottom: 1.5rem; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
.filtros-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1rem; }
.filtro-item label { display: block; font-size: 0.875rem; color: #666; margin-bottom: 0.5rem; }
.filtro-item input, .filtro-item select { width: 100%; padding: 0.5rem; border: 1px solid #ddd; border-radius: 4px; }
.tabla-card { background: white; border-radius: 8px; padding: 1.5rem; box-shadow: 0 2px 4px rgba(0,0,0,0.1); overflow-x: auto; }
table { width: 100%; border-collapse: collapse; }
thead th { text-align: left; padding: 1rem; background: #f8f9fa; font-weight: 600; border-bottom: 2px solid #e9ecef; }
tbody td { padding: 1rem; border-bottom: 1px solid #e9ecef; }
.badge { padding: 0.25rem 0.75rem; border-radius: 20px; font-size: 0.75rem; font-weight: 600; }
.badge-warning { background: #fff3cd; color: #856404; }
.badge-success { background: #d4edda; color: #155724; }
.badge-danger { background: #f8d7da; color: #721c24; }
.acciones { display: flex; gap: 0.5rem; }
.btn { padding: 0.5rem 1rem; border: none; border-radius: 4px; cursor: pointer; font-weight: 600; }
.btn-sm { padding: 0.25rem 0.5rem; font-size: 0.875rem; }
.btn-success { background: #28a745; color: white; }
.btn-danger { background: #dc3545; color: white; }
.btn-secondary { background: #6c757d; color: white; }
.formulario-item label { display: block; font-size: 0.875rem; color: #666; margin-bottom: 0.5rem; }
.formulario-item textarea { width: 100%; padding: 0.75rem; border: 1px solid #ddd; border-radius: 4px; resize: vertical; }
.formulario-acciones { display: flex; justify-content: flex-end; gap: 1rem; margin-top: 1rem; }
.sin-datos { text-align: center; padding: 3rem; color: #666; }
.texto-suave { color: #999; }
</style>
