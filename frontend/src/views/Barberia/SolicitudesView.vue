<template>
  <div class="solicitudes-view">
    <div class="page-header">
      <h1><i class="fas fa-user-plus"></i> Solicitudes de Afiliación</h1>
      <p>Gestiona las solicitudes de barberos que quieren unirse</p>
    </div>

    <div class="tabs">
      <button :class="{ active: filtro === 'pendientes' }" @click="filtro = 'pendientes'">
        Pendientes ({{ solicitudesPendientes.length }})
      </button>
      <button :class="{ active: filtro === 'aprobadas' }" @click="filtro = 'aprobadas'">
        Aprobadas
      </button>
      <button :class="{ active: filtro === 'rechazadas' }" @click="filtro = 'rechazadas'">
        Rechazadas
      </button>
    </div>

    <div class="solicitudes-lista">
      <div v-for="solicitud in solicitudesFiltradas" :key="solicitud.id" class="solicitud-card">
        <div class="solicitud-header">
          <img :src="solicitud.fotoUrl || '/default-avatar.png'" alt="Foto" class="avatar" />
          <div class="solicitud-info">
            <h3>{{ solicitud.nombre }} {{ solicitud.apellido }}</h3>
            <p class="email">{{ solicitud.email }}</p>
            <p class="telefono">{{ solicitud.telefono }}</p>
          </div>
          <span class="badge" :class="'badge-' + solicitud.estado.toLowerCase()">
            {{ solicitud.estado }}
          </span>
        </div>
        <div class="solicitud-body">
          <div class="detalle-row">
            <label>Experiencia:</label>
            <span>{{ solicitud.aniosExperiencia }} años</span>
          </div>
          <div class="detalle-row" v-if="solicitud.especialidades?.length">
            <label>Especialidades:</label>
            <span>{{ solicitud.especialidades.join(', ') }}</span>
          </div>
          <div class="detalle-row">
            <label>Fecha solicitud:</label>
            <span>{{ formatearFecha(solicitud.fechaSolicitud) }}</span>
          </div>
          <div class="detalle-row full" v-if="solicitud.mensaje">
            <label>Mensaje:</label>
            <p>{{ solicitud.mensaje }}</p>
          </div>
        </div>
        <div v-if="solicitud.estado === 'Pendiente'" class="solicitud-actions">
          <button class="btn-success" @click="responderSolicitud(solicitud, true)">
            <i class="fas fa-check"></i> Aprobar
          </button>
          <button class="btn-danger" @click="mostrarModalRechazo(solicitud)">
            <i class="fas fa-times"></i> Rechazar
          </button>
        </div>
        <div v-else-if="solicitud.estado === 'Aprobada'" class="solicitud-actions">
          <button class="btn-secondary" @click="verPerfilBarbero(solicitud)">
            <i class="fas fa-eye"></i> Ver Perfil
          </button>
        </div>
      </div>

      <div v-if="solicitudesFiltradas.length === 0" class="empty-state">
        <i class="fas fa-inbox"></i>
        <p>No hay solicitudes {{ filtro }}s</p>
      </div>
    </div>

    <!-- Modal Rechazo -->
    <div v-if="mostrarModal" class="modal-overlay" @click.self="mostrarModal = false">
      <div class="modal">
        <div class="modal-header">
          <h2>Rechazar Solicitud</h2>
          <button class="btn-close" @click="mostrarModal = false"><i class="fas fa-times"></i></button>
        </div>
        <div class="modal-body">
          <div class="form-group">
            <label>Motivo del rechazo (opcional)</label>
            <textarea v-model="motivoRechazo" rows="4" placeholder="Explica el motivo del rechazo..."></textarea>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn-secondary" @click="mostrarModal = false">Cancelar</button>
          <button class="btn-danger" @click="confirmarRechazo">
            <i class="fas fa-times"></i> Confirmar Rechazo
          </button>
        </div>
      </div>
    </div>

    <div v-if="cargando" class="loading-overlay">
      <div class="spinner"></div>
      <p>Cargando solicitudes...</p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';

const cargando = ref(false);
const filtro = ref('pendientes');
const mostrarModal = ref(false);
const solicitudSeleccionada = ref(null);
const motivoRechazo = ref('');
const solicitudes = ref([]);

const solicitudesPendientes = computed(() => 
  solicitudes.value.filter(s => s.estado === 'Pendiente')
);

const solicitudesFiltradas = computed(() => {
  if (filtro.value === 'pendientes') return solicitudesPendientes.value;
  return solicitudes.value.filter(s => s.estado.toLowerCase() === filtro.value);
});

onMounted(async () => {
  await cargarSolicitudes();
});

async function cargarSolicitudes() {
  cargando.value = true;
  try {
    const response = await fetch('/api/multibarbero/solicitudes?barberiaId=current', {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    });
    if (response.ok) {
      solicitudes.value = await response.json();
    }
  } catch (error) {
    console.error('Error cargando solicitudes:', error);
  } finally {
    cargando.value = false;
  }
}

async function responderSolicitud(solicitud, aprobada) {
  try {
    const response = await fetch(`/api/multibarbero/solicitudes/${solicitud.id}/responder`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify({ 
        aprobada,
        motivoRechazo: aprobada ? null : motivoRechazo.value 
      })
    });
    
    if (response.ok) {
      await cargarSolicitudes();
      alert(aprobada ? 'Solicitud aprobada' : 'Solicitud rechazada');
      mostrarModal.value = false;
      motivoRechazo.value = '';
    }
  } catch (error) {
    console.error('Error respondiendo:', error);
  }
}

function mostrarModalRechazo(solicitud) {
  solicitudSeleccionada.value = solicitud;
  mostrarModal.value = true;
}

async function confirmarRechazo() {
  if (solicitudSeleccionada.value) {
    await responderSolicitud(solicitudSeleccionada.value, false);
  }
}

function formatearFecha(fechaStr) {
  return new Date(fechaStr).toLocaleDateString('es-ES');
}

function verPerfilBarbero(solicitud) {
  // Navegar al perfil del barbero
  alert(`Ver perfil de ${solicitud.nombre} ${solicitud.apellido}`);
}
</script>

<style scoped>
.solicitudes-view { padding: 2rem; }
.page-header { margin-bottom: 2rem; }
.page-header h1 { font-size: 2rem; color: #1F2937; margin-bottom: 0.5rem; }
.tabs {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1.5rem;
}
.tabs button {
  padding: 0.75rem 1.5rem;
  border: none;
  background: white;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  color: #6B7280;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}
.tabs button.active {
  background: #4F46E5;
  color: white;
}
.solicitudes-lista {
  display: grid;
  gap: 1rem;
}
.solicitud-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
  overflow: hidden;
}
.solicitud-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1.25rem;
  background: #F9FAFB;
  border-bottom: 1px solid #E5E7EB;
}
.avatar {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  object-fit: cover;
}
.solicitud-info h3 { font-size: 1.125rem; color: #1F2937; margin-bottom: 0.25rem; }
.solicitud-info p { font-size: 0.875rem; color: #6B7280; }
.badge {
  margin-left: auto;
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
  font-size: 0.75rem;
  font-weight: 600;
}
.badge-pendiente { background: #FEF3C7; color: #92400E; }
.badge-aprobada { background: #D1FAE5; color: #065F46; }
.badge-rechazada { background: #FEE2E2; color: #991B1B; }
.solicitud-body { padding: 1.25rem; }
.detalle-row {
  display: flex;
  gap: 1rem;
  margin-bottom: 0.75rem;
  font-size: 0.875rem;
}
.detalle-row label { font-weight: 600; color: #4B5563; min-width: 120px; }
.detalle-row.full { flex-direction: column; }
.detalle-row.full p { margin-top: 0.5rem; color: #6B7280; }
.solicitud-actions {
  display: flex;
  gap: 0.75rem;
  padding: 1.25rem;
  border-top: 1px solid #E5E7EB;
}
.btn-success {
  background: #10B981;
  color: white;
  padding: 0.625rem 1.25rem;
  border: none;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}
.btn-success:hover { background: #059669; }
.btn-danger {
  background: #EF4444;
  color: white;
  padding: 0.625rem 1.25rem;
  border: none;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}
.btn-danger:hover { background: #DC2626; }
.btn-secondary {
  background: #F3F4F6;
  color: #374151;
  padding: 0.625rem 1.25rem;
  border: none;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}
.empty-state {
  text-align: center;
  padding: 3rem;
  color: #9CA3AF;
}
.empty-state i { font-size: 3rem; margin-bottom: 1rem; }
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
.form-group label {
  display: block;
  font-weight: 600;
  color: #4B5563;
  margin-bottom: 0.5rem;
}
.form-group textarea {
  width: 100%;
  padding: 0.625rem;
  border: 1px solid #D1D5DB;
  border-radius: 6px;
  font-size: 0.875rem;
}
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
  .solicitudes-view { padding: 1rem; }
  .solicitud-header { flex-wrap: wrap; }
  .badge { margin-left: 0; }
}
</style>
