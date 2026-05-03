<template>
  <div class="dashboard-container">
    <div class="dashboard-header">
      <h1>Dashboard Comercial</h1>
      <p class="subtitle">Panel de control y gestión del sistema</p>
    </div>

    <!-- Tarjetas de estadísticas -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon barberos">✂️</div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.barberosTotales }}</span>
          <span class="stat-label">Barberos Registrados</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon barberias">🏪</div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.barberiasActivas }}</span>
          <span class="stat-label">Barberías Activas</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon pendientes">⏳</div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.pendientesValidacion }}</span>
          <span class="stat-label">Pendientes Validación</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon crecimiento">📈</div>
        <div class="stat-info">
          <span class="stat-value">{{ stats.crecimientoMes }}%</span>
          <span class="stat-label">Crecimiento este Mes</span>
        </div>
      </div>
    </div>

    <!-- Barberos pendientes de validación -->
    <div class="section">
      <div class="section-header">
        <h2>Barberos Pendientes de Validación</h2>
        <router-link to="/comercial/barberos" class="btn-link">Ver todos</router-link>
      </div>

      <div v-if="loadingBarberos" class="loading-state">
        <div class="spinner"></div>
        <p>Cargando barberos...</p>
      </div>

      <div v-else-if="barberosPendientes.length === 0" class="empty-state">
        <p>No hay barberos pendientes de validación</p>
      </div>

      <div v-else class="barberos-list">
        <div 
          v-for="barbero in barberosPendientes" 
          :key="barbero.id" 
          class="barbero-item"
        >
          <div class="barbero-info">
            <div class="barbero-avatar">
              {{ obtenerIniciales(barbero.nombre) }}
            </div>
            <div class="barbero-detalles">
              <h3>{{ barbero.nombre }}</h3>
              <p>{{ barbero.email }}</p>
              <span class="fecha-registro">📅 {{ formatearFecha(barbero.fechaRegistro) }}</span>
            </div>
          </div>
          <div class="barbero-actions">
            <button 
              @click="validarBarbero(barbero.id)"
              class="btn btn-success"
            >
              ✅ Validar
            </button>
            <button 
              @click="verDetalleBarbero(barbero.id)"
              class="btn btn-secondary"
            >
              👁️ Ver detalle
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Barberías pendientes de aprobación -->
    <div class="section">
      <div class="section-header">
        <h2>Barberías Pendientes de Aprobación</h2>
        <router-link to="/comercial/barberias" class="btn-link">Ver todas</router-link>
      </div>

      <div v-if="loadingBarberias" class="loading-state">
        <div class="spinner"></div>
        <p>Cargando barberías...</p>
      </div>

      <div v-else-if="barberiasPendientes.length === 0" class="empty-state">
        <p>No hay barberías pendientes de aprobación</p>
      </div>

      <div v-else class="barberias-list">
        <div 
          v-for="barberia in barberiasPendientes" 
          :key="barberia.id" 
          class="barberia-item"
        >
          <div class="barberia-info">
            <div class="barberia-logo">
              🏪
            </div>
            <div class="barberia-detalles">
              <h3>{{ barberia.nombre }}</h3>
              <p>{{ barberia.direccion || 'Sin dirección' }}</p>
              <span class="fecha-solicitud">📅 {{ formatearFecha(barberia.fechaSolicitud) }}</span>
            </div>
          </div>
          <div class="barberia-actions">
            <button 
              @click="aprobarBarberia(barberia.id)"
              class="btn btn-success"
            >
              ✅ Aprobar
            </button>
            <button 
              @click="rechazarBarberia(barberia.id)"
              class="btn btn-danger"
            >
              ❌ Rechazar
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import { useAuthStore } from '../../stores/authStore'
import dashboardComercialService from '../../services/dashboardComercialService'
import barberoService from '../../services/barberoService'
import barberiaService from '../../services/barberiaService'

const authStore = useAuthStore()

const loadingBarberos = ref(false)
const loadingBarberias = ref(false)
const stats = reactive({
  barberosTotales: 0,
  barberiasActivas: 0,
  pendientesValidacion: 0,
  crecimientoMes: 0
})

const barberosPendientes = ref([])
const barberiasPendientes = ref([])

onMounted(async () => {
  await cargarEstadisticas()
  await cargarBarberosPendientes()
  await cargarBarberiasPendientes()
})

async function cargarEstadisticas() {
  try {
    const dashboardData = await dashboardComercialService.obtenerDashboard()
    
    if (dashboardData) {
      stats.barberosTotales = dashboardData.barberosTotales || 0
      stats.barberiasActivas = dashboardData.barberiasActivas || 0
      stats.pendientesValidacion = dashboardData.pendientesValidacion || 0
      stats.crecimientoMes = dashboardData.crecimientoPorcentaje || 0
    }
  } catch (error) {
    console.error('Error al cargar estadísticas:', error)
  }
}

async function cargarBarberosPendientes() {
  loadingBarberos.value = true
  try {
    const response = await dashboardComercialService.obtenerBarberosPendientesValidacion()
    barberosPendientes.value = response.items || response.data || response || []
  } catch (error) {
    console.error('Error al cargar barberos pendientes:', error)
  } finally {
    loadingBarberos.value = false
  }
}

async function cargarBarberiasPendientes() {
  loadingBarberias.value = true
  try {
    const response = await dashboardComercialService.obtenerBarberiasPendientesAprobacion()
    barberiasPendientes.value = response.items || response.data || response || []
  } catch (error) {
    console.error('Error al cargar barberías pendientes:', error)
  } finally {
    loadingBarberias.value = false
  }
}

async function validarBarbero(id) {
  if (!confirm('¿Validar documentos de este barbero?')) return
  
  try {
    await barberoService.validarDocumentos(id, true)
    alert('Barbero validado exitosamente')
    await cargarBarberosPendientes()
    await cargarEstadisticas()
  } catch (error) {
    console.error('Error al validar barbero:', error)
    alert(error.response?.data?.message || 'Error al validar el barbero')
  }
}

async function aprobarBarberia(id) {
  if (!confirm('¿Aprobar esta barbería?')) return
  
  try {
    await barberiaService.aprobarSolicitud(id, 'Aprobada por comercial')
    alert('Barbería aprobada exitosamente')
    await cargarBarberiasPendientes()
    await cargarEstadisticas()
  } catch (error) {
    console.error('Error al aprobar barbería:', error)
    alert(error.response?.data?.message || 'Error al aprobar la barbería')
  }
}

async function rechazarBarberia(id) {
  const motivo = prompt('Motivo del rechazo:')
  if (!motivo) return
  
  try {
    await barberiaService.rechazarSolicitud(id, motivo)
    alert('Solicitud de barbería rechazada')
    await cargarBarberiasPendientes()
    await cargarEstadisticas()
  } catch (error) {
    console.error('Error al rechazar barbería:', error)
    alert(error.response?.data?.message || 'Error al rechazar la barbería')
  }
}

function verDetalleBarbero(id) {
  // Navegar a la vista de detalle o mostrar modal
  alert(`Ver detalle del barbero ${id}`)
}

function obtenerIniciales(nombre) {
  if (!nombre) return '?'
  return nombre.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2)
}

function formatearFecha(fecha) {
  if (!fecha) return ''
  return new Date(fecha).toLocaleDateString('es-ES', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  })
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

.stat-icon.barberos {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.stat-icon.barberias {
  background: linear-gradient(135deg, #0ba360 0%, #3cba92 100%);
}

.stat-icon.pendientes {
  background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
}

.stat-icon.crecimiento {
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

.barberos-list,
.barberias-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.barbero-item,
.barberia-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 0.5rem;
  transition: background-color 0.2s;
}

.barbero-item:hover,
.barberia-item:hover {
  background-color: #f9fafb;
}

.barbero-info,
.barberia-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.barbero-avatar,
.barberia-logo {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 1.25rem;
}

.barberia-logo {
  background: linear-gradient(135deg, #0ba360 0%, #3cba92 100%);
}

.barbero-detalles h3,
.barberia-detalles h3 {
  margin: 0 0 0.25rem 0;
  color: #1a1a2e;
  font-size: 1rem;
}

.barbero-detalles p,
.barberia-detalles p {
  margin: 0 0 0.25rem 0;
  color: #6b7280;
  font-size: 0.875rem;
}

.fecha-registro,
.fecha-solicitud {
  font-size: 0.75rem;
  color: #9ca3af;
}

.barbero-actions,
.barberia-actions {
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

.btn-success {
  background-color: #0ba360;
  color: white;
}

.btn-danger {
  background-color: #ef4444;
  color: white;
}

.btn-secondary {
  background-color: #6b7280;
  color: white;
}

@media (max-width: 768px) {
  .dashboard-container {
    padding: 1rem;
  }

  .stats-grid {
    grid-template-columns: 1fr;
  }

  .barbero-item,
  .barberia-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .barbero-actions,
  .barberia-actions {
    width: 100%;
  }

  .barbero-actions .btn,
  .barberia-actions .btn {
    flex: 1;
  }
}
</style>

