<template>
  <div class="dashboard-container">
    <div class="dashboard-header">
      <h1>Dashboard</h1>
      <p class="subtitle">Bienvenido, {{ authStore.userName }}</p>
    </div>

    <!-- Tarjetas de estadísticas -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon reservations">📅</div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.reservasPendientes }}</span>
          <span class="stat-label">Reservas Pendientes</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon confirmed">✅</div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.reservasConfirmadas }}</span>
          <span class="stat-label">Reservas Confirmadas</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon clients">👥</div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.clientesTotales }}</span>
          <span class="stat-label">Clientes Atendidos</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon earnings">💰</div>
        <div class="stat-info">
          <span class="stat-value">${{ stats.ingresosMes }}</span>
          <span class="stat-label">Ingresos este Mes</span>
        </div>
      </div>
    </div>

    <!-- Reservas recientes -->
    <div class="section">
      <div class="section-header">
        <h2>Reservas Próximas</h2>
        <router-link to="/barbero/reservas" class="btn-link">Ver todas</router-link>
      </div>

      <div v-if="loading" class="loading-state">
        <div class="spinner"></div>
        <p>Cargando reservas...</p>
      </div>

      <div v-else-if="reservas.length === 0" class="empty-state">
        <p>No hay reservas próximas</p>
      </div>

      <div v-else class="reservas-list">
        <div 
          v-for="reserva in reservas" 
          :key="reserva.id" 
          class="reserva-item"
        >
          <div class="reserva-info">
            <div class="reserva-header">
              <span class="cliente-nombre">{{ reserva.clienteNombre }}</span>
              <span :class="['estado-badge', `estado-${reserva.estado.toLowerCase()}`]">
                {{ reserva.estado }}
              </span>
            </div>
            <div class="reserva-details">
              <span class="servicio">{{ reserva.servicioNombre }}</span>
              <span class="fecha">📅 {{ formatearFecha(reserva.fecha) }}</span>
              <span class="hora">⏰ {{ reserva.hora }}</span>
            </div>
          </div>
          <div class="reserva-actions">
            <button 
              v-if="reserva.estado === 'Pendiente'"
              @click="confirmarReserva(reserva.id)"
              class="btn btn-confirm"
            >
              Confirmar
            </button>
            <button 
              v-if="reserva.estado === 'Pendiente'"
              @click="rechazarReserva(reserva.id)"
              class="btn btn-reject"
            >
              Rechazar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Notificaciones recientes -->
    <div class="section">
      <div class="section-header">
        <h2>Notificaciones Recientes</h2>
        <router-link to="/notificaciones" class="btn-link">Ver todas</router-link>
      </div>

      <div v-if="notificaciones.length === 0" class="empty-state">
        <p>No tienes notificaciones nuevas</p>
      </div>

      <div v-else class="notificaciones-list">
        <div 
          v-for="notificacion in notificaciones" 
          :key="notificacion.id" 
          :class="['notificacion-item', { leida: notificacion.leida }]"
        >
          <div class="notificacion-icon">
            {{ obtenerIconoNotificacion(notificacion.tipo) }}
          </div>
          <div class="notificacion-content">
            <p class="notificacion-mensaje">{{ notificacion.mensaje }}</p>
            <span class="notificacion-fecha">{{ formatearFecha(notificacion.fechaCreacion) }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import { useAuthStore } from '../../stores/authStore'
import { useNotificacionStore } from '../../stores/notificacionStore'
import reservaService from '../../services/reservaService'
import dashboardBarberoService from '../../services/dashboardBarberoService'

const authStore = useAuthStore()
const notificacionStore = useNotificacionStore()

const loading = ref(false)
const stats = reactive({
  reservasPendientes: 0,
  reservasConfirmadas: 0,
  clientesTotales: 0,
  ingresosMes: 0
})

const reservas = ref([])
const notificaciones = ref([])

onMounted(async () => {
  await cargarEstadisticas()
  await cargarReservas()
  await cargarNotificaciones()
})

async function cargarEstadisticas() {
  try {
    const dashboardData = await dashboardBarberoService.obtenerDashboard()
    
    if (dashboardData) {
      stats.reservasPendientes = dashboardData.reservasPendientes || 0
      stats.reservasConfirmadas = dashboardData.reservasConfirmadas || 0
      stats.clientesTotales = dashboardData.clientesAtendidos || 0
      stats.ingresosMes = dashboardData.ingresosMes || 0
    }
  } catch (error) {
    console.error('Error al cargar estadísticas:', error)
    // Fallback: intentar obtener datos básicos de reservas
    try {
      const response = await reservaService.obtenerReservas({ estado: 'Pendiente' })
      stats.reservasPendientes = response.items?.length || response.length || 0
      
      const responseConfirmadas = await reservaService.obtenerReservas({ estado: 'Confirmada' })
      stats.reservasConfirmadas = responseConfirmadas.items?.length || responseConfirmadas.length || 0
    } catch (fallbackError) {
      console.error('Error en fallback de estadísticas:', fallbackError)
    }
  }
}

async function cargarReservas() {
  loading.value = true
  try {
    const response = await reservaService.obtenerReservas({ 
      limite: 5,
      ordenarPor: 'fecha',
      ascendente: true
    })
    reservas.value = response.items || response.data || response || []
  } catch (error) {
    console.error('Error al cargar reservas:', error)
  } finally {
    loading.value = false
  }
}

async function cargarNotificaciones() {
  try {
    await notificacionStore.cargarNotificaciones(1, 5)
    notificaciones.value = notificacionStore.notificaciones
  } catch (error) {
    console.error('Error al cargar notificaciones:', error)
  }
}

async function confirmarReserva(id) {
  try {
    await reservaService.confirmarReserva(id)
    await cargarReservas()
    await cargarEstadisticas()
  } catch (error) {
    console.error('Error al confirmar reserva:', error)
  }
}

async function rechazarReserva(id) {
  const motivo = prompt('Motivo del rechazo:')
  if (!motivo) return
  
  try {
    await reservaService.rechazarReserva(id, motivo)
    await cargarReservas()
    await cargarEstadisticas()
  } catch (error) {
    console.error('Error al rechazar reserva:', error)
  }
}

function formatearFecha(fecha) {
  if (!fecha) return ''
  return new Date(fecha).toLocaleDateString('es-ES', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  })
}

function obtenerIconoNotificacion(tipo) {
  const iconos = {
    'Reserva': '📅',
    'Mensaje': '💬',
    'Sistema': 'ℹ️',
    'Recordatorio': '⏰'
  }
  return iconos[tipo] || '🔔'
}
</script>

<style scoped>
.dashboard-container {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

.dashboard-header {
  margin-bottom: 2rem;
}

.dashboard-header h1 {
  color: #1a1a2e;
  margin: 0 0 0.5rem 0;
  font-size: 2rem;
}

.subtitle {
  color: #6b7280;
  margin: 0;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: white;
  border-radius: 1rem;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s, box-shadow 0.2s;
}

.stat-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.stat-icon {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.75rem;
}

.stat-icon.reservations {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.stat-icon.confirmed {
  background: linear-gradient(135deg, #0ba360 0%, #3cba92 100%);
}

.stat-icon.clients {
  background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
}

.stat-icon.earnings {
  background: linear-gradient(135deg, #ffd700 0%, #ffed4e 100%);
}

.stat-info {
  display: flex;
  flex-direction: column;
}

.stat-value {
  font-size: 1.75rem;
  font-weight: 700;
  color: #1a1a2e;
}

.stat-label {
  font-size: 0.875rem;
  color: #6b7280;
}

.section {
  background: white;
  border-radius: 1rem;
  padding: 1.5rem;
  margin-bottom: 2rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.section-header h2 {
  margin: 0;
  color: #1a1a2e;
  font-size: 1.25rem;
}

.btn-link {
  color: #e94560;
  text-decoration: none;
  font-weight: 500;
  font-size: 0.875rem;
  transition: color 0.2s;
}

.btn-link:hover {
  color: #d63850;
}

.loading-state,
.empty-state {
  text-align: center;
  padding: 2rem;
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

.reservas-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.reserva-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 0.5rem;
  transition: background-color 0.2s;
}

.reserva-item:hover {
  background-color: #f9fafb;
}

.reserva-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}

.cliente-nombre {
  font-weight: 600;
  color: #1a1a2e;
}

.estado-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 9999px;
  font-size: 0.75rem;
  font-weight: 500;
}

.estado-pendiente {
  background-color: #fef3c7;
  color: #92400e;
}

.estado-confirmada {
  background-color: #d1fae5;
  color: #065f46;
}

.estado-cancelada {
  background-color: #fee2e2;
  color: #991b1b;
}

.estado-completada {
  background-color: #dbeafe;
  color: #1e40af;
}

.reserva-details {
  display: flex;
  gap: 1rem;
  font-size: 0.875rem;
  color: #6b7280;
}

.reserva-actions {
  display: flex;
  gap: 0.5rem;
}

.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 0.5rem;
  font-weight: 500;
  cursor: pointer;
  transition: opacity 0.2s;
}

.btn:hover {
  opacity: 0.9;
}

.btn-confirm {
  background-color: #0ba360;
  color: white;
}

.btn-reject {
  background-color: #ef4444;
  color: white;
}

.notificaciones-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.notificacion-item {
  display: flex;
  align-items: flex-start;
  gap: 1rem;
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 0.5rem;
  transition: background-color 0.2s;
}

.notificacion-item.leida {
  opacity: 0.7;
  background-color: #f9fafb;
}

.notificacion-item:hover {
  background-color: #f3f4f6;
}

.notificacion-icon {
  font-size: 1.5rem;
}

.notificacion-content {
  flex: 1;
}

.notificacion-mensaje {
  margin: 0 0 0.25rem 0;
  color: #1a1a2e;
  font-size: 0.875rem;
}

.notificacion-fecha {
  font-size: 0.75rem;
  color: #9ca3af;
}

@media (max-width: 768px) {
  .dashboard-container {
    padding: 1rem;
  }

  .stats-grid {
    grid-template-columns: 1fr;
  }

  .reserva-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .reserva-actions {
    width: 100%;
  }

  .reserva-actions .btn {
    flex: 1;
  }
}
</style>
