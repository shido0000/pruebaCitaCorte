<template>
  <div class="estadisticas-view">
    <div class="page-header">
      <h1><i class="fas fa-chart-line"></i> Estadísticas</h1>
      <p>Análisis detallado del rendimiento de tu barbería</p>
    </div>

    <!-- Filtros -->
    <div class="filters-card">
      <div class="filter-group">
        <label for="periodo">Período:</label>
        <select id="periodo" v-model="filtros.periodo">
          <option value="7">Últimos 7 días</option>
          <option value="30">Últimos 30 días</option>
          <option value="90">Últimos 90 días</option>
          <option value="365">Último año</option>
        </select>
      </div>
      <div class="filter-group">
        <label for="barbero">Barbero:</label>
        <select id="barbero" v-model="filtros.barberoId">
          <option value="">Todos los barberos</option>
          <option v-for="barbero in barberos" :key="barbero.id" :value="barbero.id">
            {{ barbero.nombre }} {{ barbero.apellido }}
          </option>
        </select>
      </div>
      <button @click="cargarEstadisticas" class="btn-primary">
        <i class="fas fa-sync"></i> Actualizar
      </button>
    </div>

    <!-- Tarjetas de Resumen -->
    <div class="stats-grid">
      <div class="stat-card primary">
        <div class="stat-icon"><i class="fas fa-dollar-sign"></i></div>
        <div class="stat-content">
          <h3>Ingresos Totales</h3>
          <p class="stat-value">${{ estadisticas.ingresosTotales?.toFixed(2) || '0.00' }}</p>
          <span class="stat-trend" :class="estadisticas.tendenciaIngresos >= 0 ? 'positive' : 'negative'">
            <i :class="estadisticas.tendenciaIngresos >= 0 ? 'fas fa-arrow-up' : 'fas fa-arrow-down'"></i>
            {{ Math.abs(estadisticas.tendenciaIngresos) }}% vs período anterior
          </span>
        </div>
      </div>

      <div class="stat-card success">
        <div class="stat-icon"><i class="fas fa-cut"></i></div>
        <div class="stat-content">
          <h3>Total Reservas</h3>
          <p class="stat-value">{{ estadisticas.totalReservas || 0 }}</p>
          <span class="stat-trend" :class="estadisticas.tendenciaReservas >= 0 ? 'positive' : 'negative'">
            <i :class="estadisticas.tendenciaReservas >= 0 ? 'fas fa-arrow-up' : 'fas fa-arrow-down'"></i>
            {{ Math.abs(estadisticas.tendenciaReservas) }}% vs período anterior
          </span>
        </div>
      </div>

      <div class="stat-card info">
        <div class="stat-icon"><i class="fas fa-users"></i></div>
        <div class="stat-content">
          <h3>Clientes Nuevos</h3>
          <p class="stat-value">{{ estadisticas.clientesNuevos || 0 }}</p>
        </div>
      </div>

      <div class="stat-card warning">
        <div class="stat-icon"><i class="fas fa-star"></i></div>
        <div class="stat-content">
          <h3>Ticket Promedio</h3>
          <p class="stat-value">${{ estadisticas.ticketPromedio?.toFixed(2) || '0.00' }}</p>
        </div>
      </div>
    </div>

    <!-- Gráficos -->
    <div class="charts-grid">
      <div class="chart-card">
        <div class="chart-header">
          <h3><i class="fas fa-chart-bar"></i> Ingresos por Día</h3>
        </div>
        <div class="chart-container">
          <canvas ref="chartIngresos"></canvas>
        </div>
      </div>

      <div class="chart-card">
        <div class="chart-header">
          <h3><i class="fas fa-chart-pie"></i> Reservas por Estado</h3>
        </div>
        <div class="chart-container">
          <canvas ref="chartReservasEstado"></canvas>
        </div>
      </div>

      <div class="chart-card full-width">
        <div class="chart-header">
          <h3><i class="fas fa-chart-column"></i> Rendimiento por Barbero</h3>
        </div>
        <div class="chart-container">
          <canvas ref="chartRendimientoBarberos"></canvas>
        </div>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="cargando" class="loading-overlay">
      <div class="spinner"></div>
      <p>Cargando estadísticas...</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, nextTick } from 'vue';
import Chart from 'chart.js/auto';

const cargando = ref(false);
const barberos = ref([]);
const charts = ref({});

const filtros = ref({
  periodo: '30',
  barberoId: ''
});

const estadisticas = ref({
  ingresosTotales: 0,
  totalReservas: 0,
  clientesNuevos: 0,
  ticketPromedio: 0,
  tendenciaIngresos: 0,
  tendenciaReservas: 0,
  ingresosPorDia: [],
  reservasPorEstado: [],
  rendimientoBarberos: []
});

onMounted(async () => {
  await cargarBarberos();
  await cargarEstadisticas();
});

async function cargarBarberos() {
  try {
    const response = await fetch('/api/multibarbero/barberos?barberiaId=current', {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    });
    if (response.ok) {
      const data = await response.json();
      barberos.value = data;
    }
  } catch (error) {
    console.error('Error cargando barberos:', error);
  }
}

async function cargarEstadisticas() {
  cargando.value = true;
  try {
    const params = new URLSearchParams({
      dias: filtros.value.periodo,
      ...(filtros.value.barberoId && { barberoId: filtros.value.barberoId })
    });

    const response = await fetch(`/api/multibarbero/estadisticas/barberia?${params}`, {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    });

    if (response.ok) {
      estadisticas.value = await response.json();
      await nextTick();
      renderizarGraficos();
    }
  } catch (error) {
    console.error('Error cargando estadísticas:', error);
  } finally {
    cargando.value = false;
  }
}

function renderizarGraficos() {
  destruirGraficos();
  
  const ctx1 = document.querySelector('.chart-card:nth-child(1) canvas');
  const ctx2 = document.querySelector('.chart-card:nth-child(2) canvas');
  const ctx3 = document.querySelector('.chart-card:nth-child(3) canvas');

  if (ctx1 && estadisticas.value.ingresosPorDia?.length) {
    charts.value.ingresos = new Chart(ctx1, {
      type: 'line',
      data: {
        labels: estadisticas.value.ingresosPorDia.map(d => d.fecha),
        datasets: [{
          label: 'Ingresos ($)',
          data: estadisticas.value.ingresosPorDia.map(d => d.monto),
          borderColor: '#4F46E5',
          backgroundColor: 'rgba(79, 70, 229, 0.1)',
          fill: true,
          tension: 0.4
        }]
      },
      options: { responsive: true, maintainAspectRatio: false }
    });
  }

  if (ctx2 && estadisticas.value.reservasPorEstado?.length) {
    charts.value.estado = new Chart(ctx2, {
      type: 'doughnut',
      data: {
        labels: estadisticas.value.reservasPorEstado.map(e => e.estado),
        datasets: [{
          data: estadisticas.value.reservasPorEstado.map(e => e.cantidad),
          backgroundColor: ['#10B981', '#F59E0B', '#EF4444', '#6B7280', '#3B82F6']
        }]
      },
      options: { responsive: true, maintainAspectRatio: false }
    });
  }

  if (ctx3 && estadisticas.value.rendimientoBarberos?.length) {
    charts.value.barteros = new Chart(ctx3, {
      type: 'bar',
      data: {
        labels: estadisticas.value.rendimientoBarberos.map(b => `${b.nombre} ${b.apellido}`),
        datasets: [{
          label: 'Ingresos Generados ($)',
          data: estadisticas.value.rendimientoBarberos.map(b => b.ingresosGenerados),
          backgroundColor: '#06B6D4'
        }]
      },
      options: { responsive: true, maintainAspectRatio: false }
    });
  }
}

function destruirGraficos() {
  Object.values(charts.value).forEach(chart => {
    if (chart) chart.destroy();
  });
  charts.value = {};
}
</script>

<style scoped>
.estadisticas-view { padding: 2rem; }
.page-header { margin-bottom: 2rem; }
.page-header h1 { font-size: 2rem; color: #1F2937; margin-bottom: 0.5rem; }
.filters-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
  align-items: flex-end;
  margin-bottom: 2rem;
}
.filter-group { display: flex; flex-direction: column; gap: 0.5rem; }
.filter-group label { font-weight: 600; color: #4B5563; font-size: 0.875rem; }
.filter-group select { padding: 0.5rem 1rem; border: 1px solid #D1D5DB; border-radius: 6px; min-width: 180px; }
.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}
.stat-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
  display: flex;
  align-items: center;
  gap: 1rem;
  border-left: 4px solid;
}
.stat-card.primary { border-left-color: #4F46E5; }
.stat-card.success { border-left-color: #10B981; }
.stat-card.info { border-left-color: #3B82F6; }
.stat-card.warning { border-left-color: #F59E0B; }
.stat-icon {
  width: 50px;
  height: 50px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: white;
}
.stat-card.primary .stat-icon { background: #4F46E5; }
.stat-card.success .stat-icon { background: #10B981; }
.stat-card.info .stat-icon { background: #3B82F6; }
.stat-card.warning .stat-icon { background: #F59E0B; }
.stat-content h3 { font-size: 0.875rem; color: #6B7280; margin-bottom: 0.25rem; }
.stat-value { font-size: 1.5rem; font-weight: 700; color: #1F2937; margin-bottom: 0.25rem; }
.stat-trend { font-size: 0.75rem; display: flex; align-items: center; gap: 0.25rem; }
.stat-trend.positive { color: #10B981; }
.stat-trend.negative { color: #EF4444; }
.charts-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}
.chart-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}
.chart-card.full-width { grid-column: 1 / -1; }
.chart-header { margin-bottom: 1rem; }
.chart-header h3 { font-size: 1rem; color: #1F2937; }
.chart-container { height: 250px; }
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
  .estadisticas-view { padding: 1rem; }
  .charts-grid { grid-template-columns: 1fr; }
}
</style>
