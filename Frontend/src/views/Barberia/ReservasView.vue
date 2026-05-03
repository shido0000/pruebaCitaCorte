<template>
  <div class="reservas-view">
    <div class="page-header">
      <h1><i class="fas fa-calendar-alt"></i> Reservas</h1>
      <p>Gestiona todas las reservas de tu barbería</p>
    </div>

    <!-- Filtros y Búsqueda -->
    <div class="filters-bar">
      <div class="search-box">
        <i class="fas fa-search"></i>
        <input type="text" v-model="busqueda" placeholder="Buscar por cliente o servicio..." />
      </div>
      
      <div class="filter-group">
        <select v-model="filtroEstado">
          <option value="">Todos los estados</option>
          <option value="Pendiente">Pendiente</option>
          <option value="Confirmada">Confirmada</option>
          <option value="EnProgreso">En Progreso</option>
          <option value="Completada">Completada</option>
          <option value="Cancelada">Cancelada</option>
        </select>
      </div>

      <div class="filter-group">
        <select v-model="filtroBarbero">
          <option value="">Todos los barberos</option>
          <option v-for="b in barberos" :key="b.id" :value="b.id">{{ b.nombre }} {{ b.apellido }}</option>
        </select>
      </div>

      <div class="view-toggle">
        <button :class="{ active: vistaActual === 'lista' }" @click="vistaActual = 'lista'">
          <i class="fas fa-list"></i>
        </button>
        <button :class="{ active: vistaActual === 'calendario' }" @click="vistaActual = 'calendario'">
          <i class="fas fa-calendar"></i>
        </button>
      </div>
    </div>

    <!-- Vista Lista -->
    <div v-if="vistaActual === 'lista'" class="reservas-lista">
      <div class="table-card">
        <table>
          <thead>
            <tr>
              <th>Fecha/Hora</th>
              <th>Cliente</th>
              <th>Servicio</th>
              <th>Barbero</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="reserva in reservasFiltradas" :key="reserva.id">
              <td>{{ formatearFecha(reserva.fecha) }} {{ reserva.hora }}</td>
              <td>{{ reserva.clienteNombre }}</td>
              <td>{{ reserva.servicioNombre }}</td>
              <td>{{ reserva.barberoNombre }}</td>
              <td>
                <span class="badge" :class="'badge-' + reserva.estado.toLowerCase()">{{ reserva.estado }}</span>
              </td>
              <td>
                <div class="acciones">
                  <button class="btn-icon" @click="verDetalle(reserva)" title="Ver">
                    <i class="fas fa-eye"></i>
                  </button>
                  <button v-if="reserva.estado === 'Pendiente'" class="btn-icon success" @click="cambiarEstado(reserva, 'Confirmada')" title="Confirmar">
                    <i class="fas fa-check"></i>
                  </button>
                  <button v-if="reserva.estado === 'Confirmada'" class="btn-icon info" @click="cambiarEstado(reserva, 'EnProgreso')" title="Iniciar">
                    <i class="fas fa-play"></i>
                  </button>
                  <button v-if="reserva.estado === 'EnProgreso'" class="btn-icon success" @click="cambiarEstado(reserva, 'Completada')" title="Completar">
                    <i class="fas fa-flag-checkered"></i>
                  </button>
                  <button class="btn-icon danger" @click="cancelarReserva(reserva)" title="Cancelar">
                    <i class="fas fa-times"></i>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Modal Detalle -->
    <div v-if="mostrarModal" class="modal-overlay" @click.self="mostrarModal = false">
      <div class="modal">
        <div class="modal-header">
          <h2>Detalle de Reserva</h2>
          <button class="btn-close" @click="mostrarModal = false"><i class="fas fa-times"></i></button>
        </div>
        <div class="modal-body" v-if="reservaSeleccionada">
          <div class="detalle-grid">
            <div class="detalle-item">
              <label>Cliente:</label>
              <p>{{ reservaSeleccionada.clienteNombre }}</p>
            </div>
            <div class="detalle-item">
              <label>Teléfono:</label>
              <p>{{ reservaSeleccionada.clienteTelefono || 'N/A' }}</p>
            </div>
            <div class="detalle-item">
              <label>Fecha:</label>
              <p>{{ formatearFecha(reservaSeleccionada.fecha) }}</p>
            </div>
            <div class="detalle-item">
              <label>Hora:</label>
              <p>{{ reservaSeleccionada.hora }}</p>
            </div>
            <div class="detalle-item">
              <label>Servicio:</label>
              <p>{{ reservaSeleccionada.servicioNombre }}</p>
            </div>
            <div class="detalle-item">
              <label>Duración:</label>
              <p>{{ reservaSeleccionada.duracion }} min</p>
            </div>
            <div class="detalle-item">
              <label>Barbero:</label>
              <p>{{ reservaSeleccionada.barberoNombre }}</p>
            </div>
            <div class="detalle-item">
              <label>Estado:</label>
              <p><span class="badge" :class="'badge-' + reservaSeleccionada.estado.toLowerCase()">{{ reservaSeleccionada.estado }}</span></p>
            </div>
            <div class="detalle-item full" v-if="reservaSeleccionada.notas">
              <label>Notas:</label>
              <p>{{ reservaSeleccionada.notas }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="cargando" class="loading-overlay">
      <div class="spinner"></div>
      <p>Cargando reservas...</p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';

const cargando = ref(false);
const busqueda = ref('');
const filtroEstado = ref('');
const filtroBarbero = ref('');
const vistaActual = ref('lista');
const mostrarModal = ref(false);
const reservaSeleccionada = ref(null);
const reservas = ref([]);
const barberos = ref([]);

const reservasFiltradas = computed(() => {
  return reservas.value.filter(r => {
    const matchBusqueda = !busqueda.value || 
      r.clienteNombre.toLowerCase().includes(busqueda.value.toLowerCase()) ||
      r.servicioNombre.toLowerCase().includes(busqueda.value.toLowerCase());
    const matchEstado = !filtroEstado.value || r.estado === filtroEstado.value;
    const matchBarbero = !filtroBarbero.value || r.barberoId === filtroBarbero.value;
    return matchBusqueda && matchEstado && matchBarbero;
  });
});

onMounted(async () => {
  await cargarReservas();
  await cargarBarberos();
});

async function cargarReservas() {
  cargando.value = true;
  try {
    const response = await fetch('/api/multibarbero/reservas?barberiaId=current', {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    });
    if (response.ok) {
      reservas.value = await response.json();
    }
  } catch (error) {
    console.error('Error cargando reservas:', error);
  } finally {
    cargando.value = false;
  }
}

async function cargarBarberos() {
  try {
    const response = await fetch('/api/multibarbero/barberos?barberiaId=current', {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    });
    if (response.ok) {
      barberos.value = await response.json();
    }
  } catch (error) {
    console.error('Error cargando barberos:', error);
  }
}

function formatearFecha(fechaStr) {
  const fecha = new Date(fechaStr);
  return fecha.toLocaleDateString('es-ES', { day: '2-digit', month: 'short', year: 'numeric' });
}

function verDetalle(reserva) {
  reservaSeleccionada.value = reserva;
  mostrarModal.value = true;
}

async function cambiarEstado(reserva, nuevoEstado) {
  try {
    const response = await fetch(`/api/multibarbero/reservas/${reserva.id}/estado`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify({ estado: nuevoEstado })
    });
    if (response.ok) {
      reserva.estado = nuevoEstado;
      alert(`Reserva ${nuevoEstado.toLowerCase()} correctamente`);
    }
  } catch (error) {
    console.error('Error actualizando estado:', error);
  }
}

async function cancelarReserva(reserva) {
  if (!confirm('¿Estás seguro de cancelar esta reserva?')) return;
  
  try {
    await cambiarEstado(reserva, 'Cancelada');
  } catch (error) {
    console.error('Error cancelando:', error);
  }
}
</script>

<style scoped>
.reservas-view { padding: 2rem; }
.page-header { margin-bottom: 2rem; }
.page-header h1 { font-size: 2rem; color: #1F2937; margin-bottom: 0.5rem; }
.filters-bar {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
  align-items: center;
  background: white;
  padding: 1rem;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
  margin-bottom: 1.5rem;
}
.search-box {
  flex: 1;
  min-width: 250px;
  position: relative;
}
.search-box i {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #9CA3AF;
}
.search-box input {
  width: 100%;
  padding: 0.625rem 1rem 0.625rem 2.5rem;
  border: 1px solid #D1D5DB;
  border-radius: 6px;
}
.filter-group select {
  padding: 0.625rem 1rem;
  border: 1px solid #D1D5DB;
  border-radius: 6px;
  background: white;
}
.view-toggle {
  display: flex;
  gap: 0.5rem;
}
.view-toggle button {
  padding: 0.625rem;
  border: 1px solid #D1D5DB;
  background: white;
  border-radius: 6px;
  cursor: pointer;
}
.view-toggle button.active {
  background: #4F46E5;
  color: white;
  border-color: #4F46E5;
}
.table-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
  overflow: hidden;
}
table { width: 100%; border-collapse: collapse; }
th, td { padding: 1rem; text-align: left; border-bottom: 1px solid #E5E7EB; }
th { background: #F9FAFB; font-weight: 600; color: #6B7280; font-size: 0.875rem; }
.badge {
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
  font-size: 0.75rem;
  font-weight: 600;
}
.badge-pendiente { background: #FEF3C7; color: #92400E; }
.badge-confirmada { background: #DBEAFE; color: #1E40AF; }
.badge-enprogreso { background: #FEF3C7; color: #B45309; }
.badge-completada { background: #D1FAE5; color: #065F46; }
.badge-cancelada { background: #FEE2E2; color: #991B1B; }
.acciones { display: flex; gap: 0.5rem; }
.btn-icon {
  padding: 0.375rem;
  border: none;
  background: #F3F4F6;
  border-radius: 4px;
  cursor: pointer;
  color: #6B7280;
}
.btn-icon:hover { background: #E5E7EB; }
.btn-icon.success { color: #10B981; }
.btn-icon.info { color: #3B82F6; }
.btn-icon.danger { color: #EF4444; }
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
  max-width: 600px;
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
.detalle-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
}
.detalle-item label {
  display: block;
  font-size: 0.75rem;
  color: #6B7280;
  margin-bottom: 0.25rem;
}
.detalle-item.full { grid-column: 1 / -1; }
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
  .reservas-view { padding: 1rem; }
  .detalle-grid { grid-template-columns: 1fr; }
}
</style>
