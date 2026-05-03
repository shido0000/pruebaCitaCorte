<template>
  <div class="view-container">
    <div class="dashboard-header">
      <h1>Dashboard Barbería</h1>
      <p class="subtitle">Gestiona tu negocio de manera eficiente</p>
    </div>

    <!-- Estadísticas Rápidas -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon bg-blue">
          <i class="fas fa-calendar-check"></i>
        </div>
        <div class="stat-content">
          <h3>{{ estadisticas.reservasHoy }}</h3>
          <p>Reservas Hoy</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon bg-green">
          <i class="fas fa-users"></i>
        </div>
        <div class="stat-content">
          <h3>{{ estadisticas.barberosActivos }}</h3>
          <p>Barberos Activos</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon bg-purple">
          <i class="fas fa-dollar-sign"></i>
        </div>
        <div class="stat-content">
          <h3>${{ estadisticas.ingresosMes }}</h3>
          <p>Ingresos este Mes</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon bg-orange">
          <i class="fas fa-star"></i>
        </div>
        <div class="stat-content">
          <h3>{{ estadisticas.calificacionPromedio }}</h3>
          <p>Calificación Promedio</p>
        </div>
      </div>
    </div>

    <!-- Gráfico de Reservas -->
    <div class="dashboard-sections">
      <div class="section-card">
        <div class="section-header">
          <h2>Reservas de la Semana</h2>
          <button class="btn-filter" @click="filtroFecha = 'semana'">
            <i class="fas fa-filter"></i> Filtrar
          </button>
        </div>
        <div class="chart-container">
          <canvas id="reservasChart"></canvas>
        </div>
      </div>

      <!-- Solicitudes Pendientes -->
      <div class="section-card">
        <div class="section-header">
          <h2>Solicitudes de Afiliación Pendientes</h2>
          <router-link to="/barberia/solicitudes" class="btn-link">
            Ver todas
          </router-link>
        </div>
        <div v-if="solicitudesPendientes.length > 0" class="solicitudes-list">
          <div v-for="solicitud in solicitudesPendientes" :key="solicitud.id" class="solicitud-item">
            <div class="barbero-info">
              <img :src="solicitud.fotoPerfil || '/default-avatar.png'" alt="Foto" class="avatar">
              <div>
                <h4>{{ solicitud.nombre }}</h4>
                <p class="especialidad">{{ solicitud.especialidad }}</p>
              </div>
            </div>
            <div class="actions">
              <button class="btn btn-success" @click="aprobarSolicitud(solicitud.id)">
                <i class="fas fa-check"></i> Aprobar
              </button>
              <button class="btn btn-danger" @click="rechazarSolicitud(solicitud.id)">
                <i class="fas fa-times"></i> Rechazar
              </button>
            </div>
          </div>
        </div>
        <div v-else class="empty-state">
          <i class="fas fa-check-circle"></i>
          <p>No hay solicitudes pendientes</p>
        </div>
      </div>

      <!-- Próximas Reservas -->
      <div class="section-card">
        <div class="section-header">
          <h2>Próximas Reservas</h2>
          <router-link to="/barberia/reservas" class="btn-link">
            Ver calendario
          </router-link>
        </div>
        <div v-if="proximasReservas.length > 0" class="reservas-list">
          <div v-for="reserva in proximasReservas" :key="reserva.id" class="reserva-item">
            <div class="time-badge">
              <span class="hour">{{ formatearHora(reserva.fechaHoraInicio) }}</span>
              <span class="date">{{ formatearFecha(reserva.fechaHoraInicio) }}</span>
            </div>
            <div class="reserva-info">
              <h4>{{ reserva.clienteNombre }}</h4>
              <p>{{ reserva.servicioNombre }} con {{ reserva.barberoNombre }}</p>
              <span :class="['status-badge', getStatusClass(reserva.estado)]">
                {{ reserva.estado }}
              </span>
            </div>
          </div>
        </div>
        <div v-else class="empty-state">
          <i class="fas fa-calendar-alt"></i>
          <p>No hay reservas próximas</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import Chart from 'chart.js/auto'

export default {
  name: 'BarberiaDashboard',
  setup() {
    const estadisticas = ref({
      reservasHoy: 0,
      barberosActivos: 0,
      ingresosMes: 0,
      calificacionPromedio: 0
    })

    const solicitudesPendientes = ref([])
    const proximasReservas = ref([])
    const filtroFecha = ref('semana')
    let reservasChart = null

    const cargarEstadisticas = async () => {
      try {
        const response = await axios.get('/api/estadistica/barberia/dashboard')
        estadisticas.value = response.data
      } catch (error) {
        console.error('Error cargando estadísticas:', error)
      }
    }

    const cargarSolicitudesPendientes = async () => {
      try {
        const response = await axios.get('/api/solicitudafiliacion/pendientes')
        solicitudesPendientes.value = response.data.slice(0, 5)
      } catch (error) {
        console.error('Error cargando solicitudes:', error)
      }
    }

    const cargarProximasReservas = async () => {
      try {
        const response = await axios.get('/api/reserva/proximas')
        proximasReservas.value = response.data.slice(0, 5)
      } catch (error) {
        console.error('Error cargando reservas:', error)
      }
    }

    const aprobarSolicitud = async (id) => {
      try {
        await axios.post(`/api/solicitudafiliacion/${id}/aprobar`)
        alert('Solicitud aprobada exitosamente')
        cargarSolicitudesPendientes()
      } catch (error) {
        alert('Error al aprobar la solicitud')
      }
    }

    const rechazarSolicitud = async (id) => {
      const motivo = prompt('Motivo del rechazo (opcional):')
      try {
        await axios.post(`/api/solicitudafiliacion/${id}/rechazar`, { motivo })
        alert('Solicitud rechazada')
        cargarSolicitudesPendientes()
      } catch (error) {
        alert('Error al rechazar la solicitud')
      }
    }

    const formatearHora = (fecha) => {
      return new Date(fecha).toLocaleTimeString('es-ES', { hour: '2-digit', minute: '2-digit' })
    }

    const formatearFecha = (fecha) => {
      return new Date(fecha).toLocaleDateString('es-ES', { day: 'numeric', month: 'short' })
    }

    const getStatusClass = (estado) => {
      const classes = {
        'Pendiente': 'status-pending',
        'Confirmada': 'status-confirmed',
        'EnProceso': 'status-process',
        'Completada': 'status-completed',
        'Cancelada': 'status-cancelled'
      }
      return classes[estado] || ''
    }

    const renderizarChart = () => {
      const ctx = document.getElementById('reservasChart')
      if (!ctx) return

      if (reservasChart) {
        reservasChart.destroy()
      }

      reservasChart = new Chart(ctx, {
        type: 'line',
        data: {
          labels: ['Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb', 'Dom'],
          datasets: [{
            label: 'Reservas',
            data: [12, 19, 15, 22, 18, 25, 14],
            borderColor: '#4F46E5',
            backgroundColor: 'rgba(79, 70, 229, 0.1)',
            tension: 0.4,
            fill: true
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              display: false
            }
          },
          scales: {
            y: {
              beginAtZero: true,
              grid: {
                color: 'rgba(0, 0, 0, 0.05)'
              }
            },
            x: {
              grid: {
                display: false
              }
            }
          }
        }
      })
    }

    onMounted(() => {
      cargarEstadisticas()
      cargarSolicitudesPendientes()
      cargarProximasReservas()
      setTimeout(renderizarChart, 100)
    })

    return {
      estadisticas,
      solicitudesPendientes,
      proximasReservas,
      filtroFecha,
      aprobarSolicitud,
      rechazarSolicitud,
      formatearHora,
      formatearFecha,
      getStatusClass
    }
  }
}
</script>

<style scoped>
.view-container {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

.dashboard-header {
  margin-bottom: 2rem;
}

.dashboard-header h1 {
  font-size: 2rem;
  color: #1a1a1a;
  margin-bottom: 0.5rem;
}

.subtitle {
  color: #666;
  font-size: 1rem;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

.stat-icon {
  width: 60px;
  height: 60px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: white;
}

.bg-blue { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); }
.bg-green { background: linear-gradient(135deg, #11998e 0%, #38ef7d 100%); }
.bg-purple { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); }
.bg-orange { background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); }

.stat-content h3 {
  font-size: 1.8rem;
  color: #1a1a1a;
  margin: 0;
}

.stat-content p {
  color: #666;
  margin: 0;
  font-size: 0.9rem;
}

.dashboard-sections {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 1.5rem;
}

.section-card {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.section-header h2 {
  font-size: 1.2rem;
  color: #1a1a1a;
  margin: 0;
}

.btn-link {
  color: #4F46E5;
  text-decoration: none;
  font-size: 0.9rem;
}

.btn-link:hover {
  text-decoration: underline;
}

.chart-container {
  height: 250px;
}

.solicitudes-list, .reservas-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.solicitud-item, .reserva-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 8px;
}

.barbero-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.avatar {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  object-fit: cover;
}

.barbero-info h4 {
  margin: 0;
  color: #1a1a1a;
}

.especialidad {
  color: #666;
  font-size: 0.9rem;
  margin: 0;
}

.actions {
  display: flex;
  gap: 0.5rem;
}

.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-size: 0.9rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-success {
  background: #10b981;
  color: white;
}

.btn-danger {
  background: #ef4444;
  color: white;
}

.time-badge {
  text-align: center;
  padding: 0.5rem 1rem;
  background: #4F46E5;
  color: white;
  border-radius: 8px;
  margin-right: 1rem;
}

.hour {
  display: block;
  font-weight: bold;
  font-size: 1.1rem;
}

.date {
  display: block;
  font-size: 0.8rem;
  opacity: 0.9;
}

.reserva-info h4 {
  margin: 0 0 0.25rem 0;
  color: #1a1a1a;
}

.reserva-info p {
  margin: 0 0 0.5rem 0;
  color: #666;
  font-size: 0.9rem;
}

.status-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 500;
}

.status-pending { background: #fef3c7; color: #92400e; }
.status-confirmed { background: #d1fae5; color: #065f46; }
.status-process { background: #dbeafe; color: #1e40af; }
.status-completed { background: #d1fae5; color: #065f46; }
.status-cancelled { background: #fee2e2; color: #991b1b; }

.empty-state {
  text-align: center;
  padding: 2rem;
  color: #666;
}

.empty-state i {
  font-size: 3rem;
  color: #ddd;
  margin-bottom: 1rem;
  display: block;
}

.btn-filter {
  background: #f3f4f6;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #374151;
}

@media (max-width: 768px) {
  .dashboard-sections {
    grid-template-columns: 1fr;
  }
  
  .stats-grid {
    grid-template-columns: 1fr;
  }
}
</style>
