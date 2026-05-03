<template>
  <div class="view-container reservas-view">
    <div class="view-header">
      <h1>Gestión de Reservas</h1>
      <button @click="cargarReservas" class="btn btn-refresh" :disabled="loading">
        🔄 Actualizar
      </button>
    </div>

    <!-- Filtros y búsqueda -->
    <div class="filters-section">
      <div class="search-box">
        <input 
          v-model="filtros.busqueda" 
          @input="aplicarFiltros"
          placeholder="Buscar por cliente o servicio..." 
          class="form-control"
        />
      </div>
      
      <div class="filters-grid">
        <select v-model="filtros.estado" @change="aplicarFiltros" class="form-control">
          <option value="">Todos los estados</option>
          <option value="Pendiente">Pendiente</option>
          <option value="Confirmada">Confirmada</option>
          <option value="En Proceso">En Proceso</option>
          <option value="Completada">Completada</option>
          <option value="Cancelada">Cancelada</option>
          <option value="Rechazada">Rechazada</option>
        </select>

        <input 
          type="date" 
          v-model="filtros.fechaDesde" 
          @change="aplicarFiltros"
          class="form-control"
          placeholder="Fecha desde"
        />

        <input 
          type="date" 
          v-model="filtros.fechaHasta" 
          @change="aplicarFiltros"
          class="form-control"
          placeholder="Fecha hasta"
        />
      </div>
    </div>

    <!-- Estado de carga -->
    <div v-if="loading" class="loading-state">
      <div class="spinner"></div>
      <p>Cargando reservas...</p>
    </div>

    <!-- Mensaje sin datos -->
    <div v-else-if="reservas.length === 0" class="empty-state">
      <div class="empty-icon">📅</div>
      <h3>No hay reservas</h3>
      <p>No se encontraron reservas con los filtros seleccionados</p>
      <button @click="limpiarFiltros" class="btn btn-primary">Limpiar filtros</button>
    </div>

    <!-- Lista de reservas -->
    <div v-else class="reservas-list">
      <div class="reservas-header">
        <span class="total-reservas">{{ reservas.length }} reserva(s) encontrada(s)</span>
      </div>

      <div class="reservas-grid">
        <div 
          v-for="reserva in reservas" 
          :key="reserva.id"
          class="reserva-card"
          :class="['estado-' + reserva.estado.toLowerCase().replace(' ', '-')]"
        >
          <div class="reserva-header">
            <div class="cliente-info">
              <div class="cliente-avatar">
                {{ obtenerIniciales(reserva.cliente?.nombre || 'C') }}
              </div>
              <div class="cliente-detalles">
                <h3>{{ reserva.cliente?.nombre || 'Cliente' }}</h3>
                <p>{{ reserva.cliente?.email || '' }}</p>
              </div>
            </div>
            <span class="badge estado" :class="'badge-' + getEstadoClass(reserva.estado)">
              {{ reserva.estado }}
            </span>
          </div>

          <div class="reserva-body">
            <div class="servicio-info">
              <h4>{{ reserva.servicio?.nombre || 'Servicio' }}</h4>
              <p class="descripcion">{{ reserva.servicio?.descripcion || '' }}</p>
            </div>

            <div class="fecha-hora">
              <div class="info-item">
                <span class="icon">📅</span>
                <span>{{ formatearFecha(reserva.fechaHoraInicio) }}</span>
              </div>
              <div class="info-item">
                <span class="icon">⏰</span>
                <span>{{ formatearHora(reserva.fechaHoraInicio) }} - {{ formatearHora(reserva.fechaHoraFin) }}</span>
              </div>
            </div>

            <div class="precio-info">
              <span class="label">Precio:</span>
              <span class="precio">${{ (reserva.servicio?.precio || 0).toFixed(2) }}</span>
            </div>

            <div v-if="reserva.notas" class="notas">
              <span class="icon">📝</span>
              <span>{{ reserva.notas }}</span>
            </div>
          </div>

          <div class="reserva-actions">
            <button 
              v-if="reserva.estado === 'Pendiente'"
              @click="confirmarReserva(reserva)"
              class="btn btn-success btn-sm"
              :disabled="accionando"
            >
              ✅ Confirmar
            </button>
            
            <button 
              v-if="reserva.estado === 'Pendiente'"
              @click="mostrarModalRechazo(reserva)"
              class="btn btn-danger btn-sm"
              :disabled="accionando"
            >
              ❌ Rechazar
            </button>

            <button 
              v-if="reserva.estado === 'Confirmada'"
              @click="iniciarReserva(reserva)"
              class="btn btn-primary btn-sm"
              :disabled="accionando"
            >
              ▶️ Iniciar
            </button>

            <button 
              v-if="reserva.estado === 'En Proceso'"
              @click="completarReserva(reserva)"
              class="btn btn-success btn-sm"
              :disabled="accionando"
            >
              ✔️ Completar
            </button>

            <button 
              v-if="['Pendiente', 'Confirmada'].includes(reserva.estado)"
              @click="cancelarReserva(reserva)"
              class="btn btn-warning btn-sm"
              :disabled="accionando"
            >
              🚫 Cancelar
            </button>

            <button 
              @click="verDetalleReserva(reserva)"
              class="btn btn-info btn-sm"
            >
              👁️ Ver detalle
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal de Rechazo -->
    <div v-if="modalRechazo.visible" class="modal-overlay" @click.self="cerrarModalRechazo">
      <div class="modal-content">
        <h3>Rechazar Reserva</h3>
        <p>Indique el motivo del rechazo:</p>
        <textarea 
          v-model="motivoRechazo" 
          class="form-control"
          rows="4"
          placeholder="Escriba el motivo..."
        ></textarea>
        <div class="modal-actions">
          <button @click="cerrarModalRechazo" class="btn btn-secondary">Cancelar</button>
          <button @click="ejecutarRechazo" class="btn btn-danger" :disabled="!motivoRechazo.trim()">
            Rechazar Reserva
          </button>
        </div>
      </div>
    </div>

    <!-- Modal de Completar -->
    <div v-if="modalCompletar.visible" class="modal-overlay" @click.self="cerrarModalCompletar">
      <div class="modal-content">
        <h3>Completar Reserva</h3>
        <p>¿El cliente desea dejar una calificación?</p>
        <div class="calificacion-section">
          <label>Calificación:</label>
          <div class="estrellas">
            <span 
              v-for="n in 5" 
              :key="n"
              @click="calificacion = n"
              class="estrella"
              :class="{ activa: n <= calificacion }"
            >
              ⭐
            </span>
          </div>
        </div>
        <textarea 
          v-model="comentarioCompletar" 
          class="form-control"
          rows="3"
          placeholder="Comentario opcional del cliente..."
        ></textarea>
        <div class="modal-actions">
          <button @click="cerrarModalCompletar" class="btn btn-secondary">Cancelar</button>
          <button @click="ejecutarCompletar" class="btn btn-success">
            Completar Reserva
          </button>
        </div>
      </div>
    </div>

    <!-- Modal de Detalle -->
    <div v-if="modalDetalle.visible" class="modal-overlay" @click.self="cerrarModalDetalle">
      <div class="modal-content modal-large">
        <h3>Detalle de Reserva</h3>
        <div v-if="reservaSeleccionada" class="detalle-content">
          <div class="detalle-row">
            <span class="label">ID:</span>
            <span>{{ reservaSeleccionada.id }}</span>
          </div>
          <div class="detalle-row">
            <span class="label">Cliente:</span>
            <span>{{ reservaSeleccionada.cliente?.nombre }}</span>
          </div>
          <div class="detalle-row">
            <span class="label">Email:</span>
            <span>{{ reservaSeleccionada.cliente?.email }}</span>
          </div>
          <div class="detalle-row">
            <span class="label">Teléfono:</span>
            <span>{{ reservaSeleccionada.cliente?.telefono || 'No especificado' }}</span>
          </div>
          <div class="detalle-row">
            <span class="label">Servicio:</span>
            <span>{{ reservaSeleccionada.servicio?.nombre }}</span>
          </div>
          <div class="detalle-row">
            <span class="label">Descripción:</span>
            <span>{{ reservaSeleccionada.servicio?.descripcion || 'N/A' }}</span>
          </div>
          <div class="detalle-row">
            <span class="label">Precio:</span>
            <span>${{ (reservaSeleccionada.servicio?.precio || 0).toFixed(2) }}</span>
          </div>
          <div class="detalle-row">
            <span class="label">Fecha y Hora:</span>
            <span>{{ formatearFechaHora(reservaSeleccionada.fechaHoraInicio) }} - {{ formatearHora(reservaSeleccionada.fechaHoraFin) }}</span>
          </div>
          <div class="detalle-row">
            <span class="label">Estado:</span>
            <span class="badge" :class="'badge-' + getEstadoClass(reservaSeleccionada.estado)">
              {{ reservaSeleccionada.estado }}
            </span>
          </div>
          <div v-if="reservaSeleccionada.notas" class="detalle-row">
            <span class="label">Notas:</span>
            <span>{{ reservaSeleccionada.notas }}</span>
          </div>
          <div v-if="reservaSeleccionada.motivoCancelacion" class="detalle-row">
            <span class="label">Motivo Cancelación:</span>
            <span>{{ reservaSeleccionada.motivoCancelacion }}</span>
          </div>
          <div v-if="reservaSeleccionada.calificacion" class="detalle-row">
            <span class="label">Calificación:</span>
            <span>{{ '⭐'.repeat(reservaSeleccionada.calificacion) }}</span>
          </div>
        </div>
        <div class="modal-actions">
          <button @click="cerrarModalDetalle" class="btn btn-primary">Cerrar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import { useAuthStore } from '@/stores/authStore'
import reservaService from '@/services/reservaService'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

const authStore = useAuthStore()

// Estado
const loading = ref(false)
const accionando = ref(false)
const reservas = ref([])
const reservaSeleccionada = ref(null)

// Filtros
const filtros = reactive({
  busqueda: '',
  estado: '',
  fechaDesde: '',
  fechaHasta: ''
})

// Modales
const modalRechazo = reactive({ visible: false })
const modalCompletar = reactive({ visible: false })
const modalDetalle = reactive({ visible: false })

// Datos de formularios
const motivoRechazo = ref('')
const calificacion = ref(0)
const comentarioCompletar = ref('')

// Cargar reservas
async function cargarReservas() {
  loading.value = true
  try {
    const params = {}
    if (filtros.estado) params.estado = filtros.estado
    if (filtros.fechaDesde) params.fechaDesde = filtros.fechaDesde
    if (filtros.fechaHasta) params.fechaHasta = filtros.fechaHasta
    
    const data = await reservaService.obtenerReservas(params)
    reservas.value = Array.isArray(data) ? data : []
    
    // Filtrar por búsqueda
    if (filtros.busqueda) {
      aplicarFiltroBusqueda()
    }
  } catch (error) {
    console.error('Error al cargar reservas:', error)
    toast.error('Error al cargar las reservas')
    reservas.value = []
  } finally {
    loading.value = false
  }
}

// Aplicar filtros
function aplicarFiltros() {
  cargarReservas()
}

function aplicarFiltroBusqueda() {
  if (!filtros.busqueda) return
  
  const termino = filtros.busqueda.toLowerCase()
  reservas.value = reservas.value.filter(r => {
    const nombreCliente = r.cliente?.nombre?.toLowerCase() || ''
    const emailCliente = r.cliente?.email?.toLowerCase() || ''
    const nombreServicio = r.servicio?.nombre?.toLowerCase() || ''
    
    return nombreCliente.includes(termino) || 
           emailCliente.includes(termino) || 
           nombreServicio.includes(termino)
  })
}

function limpiarFiltros() {
  filtros.busqueda = ''
  filtros.estado = ''
  filtros.fechaDesde = ''
  filtros.fechaHasta = ''
  cargarReservas()
}

// Acciones con reservas
async function confirmarReserva(reserva) {
  if (!confirm('¿Confirmar esta reserva?')) return
  
  accionando.value = true
  try {
    await reservaService.confirmarReserva(reserva.id)
    toast.success('Reserva confirmada exitosamente')
    cargarReservas()
  } catch (error) {
    console.error('Error al confirmar:', error)
    toast.error(error.response?.data?.message || 'Error al confirmar la reserva')
  } finally {
    accionando.value = false
  }
}

function mostrarModalRechazo(reserva) {
  reservaSeleccionada.value = reserva
  motivoRechazo.value = ''
  modalRechazo.visible = true
}

function cerrarModalRechazo() {
  modalRechazo.visible = false
  reservaSeleccionada.value = null
  motivoRechazo.value = ''
}

async function ejecutarRechazo() {
  if (!motivoRechazo.value.trim()) {
    toast.error('Debe indicar un motivo para el rechazo')
    return
  }
  
  accionando.value = true
  try {
    await reservaService.rechazarReserva(reservaSeleccionada.value.id, motivoRechazo.value)
    toast.success('Reserva rechazada')
    cerrarModalRechazo()
    cargarReservas()
  } catch (error) {
    console.error('Error al rechazar:', error)
    toast.error(error.response?.data?.message || 'Error al rechazar la reserva')
  } finally {
    accionando.value = false
  }
}

async function cancelarReserva(reserva) {
  const motivo = prompt('Indique el motivo de la cancelación:')
  if (!motivo) return
  
  accionando.value = true
  try {
    await reservaService.cancelarReserva(reserva.id, motivo)
    toast.success('Reserva cancelada')
    cargarReservas()
  } catch (error) {
    console.error('Error al cancelar:', error)
    toast.error(error.response?.data?.message || 'Error al cancelar la reserva')
  } finally {
    accionando.value = false
  }
}

async function iniciarReserva(reserva) {
  if (!confirm('¿Iniciar atención de esta reserva?')) return
  
  accionando.value = true
  try {
    // Simulamos inicio cambiando a "En Proceso"
    await reservaService.actualizarReserva(reserva.id, { estado: 'En Proceso' })
    toast.success('Reserva iniciada')
    cargarReservas()
  } catch (error) {
    console.error('Error al iniciar:', error)
    toast.error('Error al iniciar la reserva')
  } finally {
    accionando.value = false
  }
}

function mostrarModalCompletar(reserva) {
  reservaSeleccionada.value = reserva
  calificacion.value = 0
  comentarioCompletar.value = ''
  modalCompletar.visible = true
}

function completarReserva(reserva) {
  mostrarModalCompletar(reserva)
}

function cerrarModalCompletar() {
  modalCompletar.visible = false
  reservaSeleccionada.value = null
  calificacion.value = 0
  comentarioCompletar.value = ''
}

async function ejecutarCompletar() {
  accionando.value = true
  try {
    await reservaService.completarReserva(
      reservaSeleccionada.value.id,
      calificacion.value > 0 ? calificacion.value : null,
      comentarioCompletar.value || null
    )
    toast.success('Reserva completada exitosamente')
    cerrarModalCompletar()
    cargarReservas()
  } catch (error) {
    console.error('Error al completar:', error)
    toast.error('Error al completar la reserva')
  } finally {
    accionando.value = false
  }
}

function verDetalleReserva(reserva) {
  reservaSeleccionada.value = reserva
  modalDetalle.visible = true
}

function cerrarModalDetalle() {
  modalDetalle.visible = false
  reservaSeleccionada.value = null
}

// Utilidades
function obtenerIniciales(nombre) {
  if (!nombre) return 'C'
  const partes = nombre.split(' ')
  if (partes.length >= 2) {
    return (partes[0][0] + partes[1][0]).toUpperCase()
  }
  return nombre.substring(0, 2).toUpperCase()
}

function getEstadoClass(estado) {
  const clases = {
    'Pendiente': 'warning',
    'Confirmada': 'info',
    'En Proceso': 'primary',
    'Completada': 'success',
    'Cancelada': 'danger',
    'Rechazada': 'dark'
  }
  return clases[estado] || 'secondary'
}

function formatearFecha(fechaString) {
  if (!fechaString) return ''
  const fecha = new Date(fechaString)
  return fecha.toLocaleDateString('es-ES', { 
    weekday: 'short', 
    year: 'numeric', 
    month: 'short', 
    day: 'numeric' 
  })
}

function formatearHora(fechaString) {
  if (!fechaString) return ''
  const fecha = new Date(fechaString)
  return fecha.toLocaleTimeString('es-ES', { 
    hour: '2-digit', 
    minute: '2-digit' 
  })
}

function formatearFechaHora(fechaString) {
  if (!fechaString) return ''
  const fecha = new Date(fechaString)
  return fecha.toLocaleDateString('es-ES', { 
    weekday: 'long', 
    year: 'numeric', 
    month: 'long', 
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

onMounted(() => {
  cargarReservas()
})
</script>

<style scoped>
.reservas-view {
  max-width: 1400px;
  margin: 0 auto;
}

.view-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.btn-refresh {
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  border: none;
  background: var(--color-primary);
  color: white;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn-refresh:hover:not(:disabled) {
  background: var(--color-primary-dark);
  transform: translateY(-2px);
}

.btn-refresh:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Filtros */
.filters-section {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  margin-bottom: 2rem;
}

.search-box {
  margin-bottom: 1rem;
}

.search-box input {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 1rem;
  transition: border-color 0.3s ease;
}

.search-box input:focus {
  outline: none;
  border-color: var(--color-primary);
}

.filters-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
}

.filters-grid select,
.filters-grid input {
  padding: 0.75rem 1rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 0.95rem;
  background: white;
}

.filters-grid select:focus,
.filters-grid input:focus {
  outline: none;
  border-color: var(--color-primary);
}

/* Loading y Empty states */
.loading-state,
.empty-state {
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

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
}

/* Lista de reservas */
.reservas-list {
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.reservas-header {
  padding: 1.5rem;
  background: #f8f9fa;
  border-bottom: 2px solid #e0e0e0;
}

.total-reservas {
  font-weight: 600;
  color: #666;
}

.reservas-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
  gap: 1.5rem;
  padding: 1.5rem;
}

.reserva-card {
  background: white;
  border: 2px solid #e0e0e0;
  border-radius: 12px;
  overflow: hidden;
  transition: all 0.3s ease;
}

.reserva-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
}

.reserva-card.estado-pendiente {
  border-left: 4px solid #ffc107;
}

.reserva-card.estado-confirmada {
  border-left: 4px solid #17a2b8;
}

.reserva-card.estado-en-proceso {
  border-left: 4px solid #007bff;
}

.reserva-card.estado-completada {
  border-left: 4px solid #28a745;
}

.reserva-card.estado-cancelada,
.reserva-card.estado-rechazada {
  border-left: 4px solid #dc3545;
  opacity: 0.8;
}

.reserva-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  background: #f8f9fa;
  border-bottom: 1px solid #e0e0e0;
}

.cliente-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.cliente-avatar {
  width: 45px;
  height: 45px;
  border-radius: 50%;
  background: var(--color-primary);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 1.1rem;
}

.cliente-detalles h3 {
  margin: 0;
  font-size: 1rem;
  color: #333;
}

.cliente-detalles p {
  margin: 0;
  font-size: 0.85rem;
  color: #666;
}

.badge {
  padding: 0.35rem 0.75rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
}

.badge-warning {
  background: #fff3cd;
  color: #856404;
}

.badge-info {
  background: #d1ecf1;
  color: #0c5460;
}

.badge-primary {
  background: #cce5ff;
  color: #004085;
}

.badge-success {
  background: #d4edda;
  color: #155724;
}

.badge-danger {
  background: #f8d7da;
  color: #721c24;
}

.badge-dark {
  background: #d6d8d9;
  color: #1b1e21;
}

.reserva-body {
  padding: 1.25rem;
}

.servicio-info h4 {
  margin: 0 0 0.5rem 0;
  color: #333;
  font-size: 1.1rem;
}

.descripcion {
  margin: 0;
  color: #666;
  font-size: 0.9rem;
  line-height: 1.4;
}

.fecha-hora {
  margin-top: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.info-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #555;
  font-size: 0.9rem;
}

.icon {
  font-size: 1.1rem;
}

.precio-info {
  margin-top: 1rem;
  padding-top: 1rem;
  border-top: 1px solid #e0e0e0;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.precio-info .label {
  color: #666;
  font-weight: 500;
}

.precio {
  font-size: 1.25rem;
  font-weight: 700;
  color: var(--color-primary);
}

.notas {
  margin-top: 1rem;
  padding: 0.75rem;
  background: #fff3cd;
  border-radius: 8px;
  display: flex;
  align-items: flex-start;
  gap: 0.5rem;
  font-size: 0.9rem;
  color: #856404;
}

.reserva-actions {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  padding: 1rem 1.25rem;
  background: #f8f9fa;
  border-top: 1px solid #e0e0e0;
}

.btn-sm {
  padding: 0.5rem 0.75rem;
  font-size: 0.85rem;
  border-radius: 6px;
  border: none;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.2s ease;
}

.btn-sm:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-success {
  background: #28a745;
  color: white;
}

.btn-success:hover:not(:disabled) {
  background: #218838;
}

.btn-danger {
  background: #dc3545;
  color: white;
}

.btn-danger:hover:not(:disabled) {
  background: #c82333;
}

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #0069d9;
}

.btn-warning {
  background: #ffc107;
  color: #333;
}

.btn-warning:hover:not(:disabled) {
  background: #e0a800;
}

.btn-info {
  background: #17a2b8;
  color: white;
}

.btn-info:hover:not(:disabled) {
  background: #138496;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover:not(:disabled) {
  background: #5a6268;
}

/* Modales */
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
  border-radius: 12px;
  padding: 2rem;
  max-width: 500px;
  width: 100%;
  max-height: 90vh;
  overflow-y: auto;
}

.modal-large {
  max-width: 700px;
}

.modal-content h3 {
  margin: 0 0 1.5rem 0;
  color: #333;
}

.modal-content textarea {
  width: 100%;
  padding: 0.75rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 1rem;
  resize: vertical;
  margin-bottom: 1.5rem;
}

.modal-content textarea:focus {
  outline: none;
  border-color: var(--color-primary);
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
}

.calificacion-section {
  margin-bottom: 1.5rem;
}

.calificacion-section label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 600;
  color: #333;
}

.estrellas {
  display: flex;
  gap: 0.5rem;
}

.estrella {
  font-size: 2rem;
  cursor: pointer;
  opacity: 0.3;
  transition: all 0.2s ease;
}

.estrella:hover,
.estrella.activa {
  opacity: 1;
  transform: scale(1.1);
}

.detalle-content {
  margin-bottom: 1.5rem;
}

.detalle-row {
  display: flex;
  justify-content: space-between;
  padding: 0.75rem 0;
  border-bottom: 1px solid #e0e0e0;
}

.detalle-row:last-child {
  border-bottom: none;
}

.detalle-row .label {
  font-weight: 600;
  color: #666;
}

/* Responsive */
@media (max-width: 768px) {
  .reservas-grid {
    grid-template-columns: 1fr;
  }
  
  .view-header {
    flex-direction: column;
    align-items: stretch;
  }
  
  .btn-refresh {
    width: 100%;
  }
  
  .filters-grid {
    grid-template-columns: 1fr;
  }
  
  .reserva-actions {
    flex-direction: column;
  }
  
  .reserva-actions .btn-sm {
    width: 100%;
  }
}
</style>
