<template>
  <div class="estadisticas-container">
    <div class="page-header">
      <h1><i class="fas fa-chart-line"></i> Estadísticas y Reportes</h1>
      <div class="actions">
        <select v-model="periodo" @change="cargarEstadisticas" class="period-selector">
          <option value="7">Últimos 7 días</option>
          <option value="30">Últimos 30 días</option>
          <option value="90">Últimos 3 meses</option>
          <option value="365">Último año</option>
        </select>
        <button class="btn btn-primary" @click="exportarReporte">
          <i class="fas fa-download"></i> Exportar
        </button>
      </div>
    </div>

    <!-- Tarjetas de Resumen -->
    <div class="resumen-grid">
      <div class="resumen-card">
        <div class="resumen-icon icon-primary">
          <i class="fas fa-calendar-check"></i>
        </div>
        <div class="resumen-info">
          <h3>{{ estadisticas.totalReservas }}</h3>
          <p>Total Reservas</p>
          <span class="variacion" :class="{ positiva: estadisticas.variacionReservas >= 0, negativa: estadisticas.variacionReservas < 0 }">
            <i :class="estadisticas.variacionReservas >= 0 ? 'fas fa-arrow-up' : 'fas fa-arrow-down'"></i>
            {{ Math.abs(estadisticas.variacionReservas) }}% vs periodo anterior
          </span>
        </div>
      </div>

      <div class="resumen-card">
        <div class="resumen-icon icon-success">
          <i class="fas fa-dollar-sign"></i>
        </div>
        <div class="resumen-info">
          <h3>${{ estadisticas.ingresosTotales?.toFixed(2) }}</h3>
          <p>Ingresos Totales</p>
          <span class="variacion" :class="{ positiva: estadisticas.variacionIngresos >= 0, negativa: estadisticas.variacionIngresos < 0 }">
            <i :class="estadisticas.variacionIngresos >= 0 ? 'fas fa-arrow-up' : 'fas fa-arrow-down'"></i>
            {{ Math.abs(estadisticas.variacionIngresos) }}% vs periodo anterior
          </span>
        </div>
      </div>

      <div class="resumen-card">
        <div class="resumen-icon icon-info">
          <i class="fas fa-users"></i>
        </div>
        <div class="resumen-info">
          <h3>{{ estadisticas.clientesUnicos }}</h3>
          <p>Clientes Únicos</p>
          <span class="variacion" :class="{ positiva: estadisticas.variacionClientes >= 0, negativa: estadisticas.variacionClientes < 0 }">
            <i :class="estadisticas.variacionClientes >= 0 ? 'fas fa-arrow-up' : 'fas fa-arrow-down'"></i>
            {{ Math.abs(estadisticas.variacionClientes) }}% vs periodo anterior
          </span>
        </div>
      </div>

      <div class="resumen-card">
        <div class="resumen-icon icon-warning">
          <i class="fas fa-star"></i>
        </div>
        <div class="resumen-info">
          <h3>{{ estadisticas.calificacionPromedio?.toFixed(1) }}</h3>
          <p>Calificación Promedio</p>
          <span class="variacion neutral">
            <i class="fas fa-minus"></i> Basado en {{ estadisticas.totalCalificaciones }} calificaciones
          </span>
        </div>
      </div>
    </div>

    <!-- Gráficos -->
    <div class="graficos-grid">
      <!-- Reservas por Día -->
      <div class="grafico-card">
        <div class="grafico-header">
          <h3><i class="fas fa-chart-bar"></i> Reservas por Día</h3>
        </div>
        <div class="grafico-body">
          <div class="chart-placeholder" v-if="!datosGraficoReservas.length">
            <i class="fas fa-chart-bar"></i>
            <p>No hay datos disponibles</p>
          </div>
          <div v-else class="bar-chart">
            <div 
              v-for="(dia, index) in datosGraficoReservas" 
              :key="index" 
              class="bar-item"
            >
              <div class="bar-label">{{ dia.dia }}</div>
              <div class="bar-container">
                <div 
                  class="bar-fill" 
                  :style="{ height: `${(dia.cantidad / maxReservasDia) * 100}%` }"
                ></div>
              </div>
              <div class="bar-value">{{ dia.cantidad }}</div>
            </div>
          </div>
        </div>
      </div>

      <!-- Ingresos por Día -->
      <div class="grafico-card">
        <div class="grafico-header">
          <h3><i class="fas fa-chart-line"></i> Ingresos por Día</h3>
        </div>
        <div class="grafico-body">
          <div class="chart-placeholder" v-if="!datosGraficoIngresos.length">
            <i class="fas fa-chart-line"></i>
            <p>No hay datos disponibles</p>
          </div>
          <div v-else class="line-chart">
            <svg viewBox="0 0 400 200" class="line-svg">
              <defs>
                <linearGradient id="gradientArea" x1="0%" y1="0%" x2="0%" y2="100%">
                  <stop offset="0%" style="stop-color:var(--success-color);stop-opacity:0.3" />
                  <stop offset="100%" style="stop-color:var(--success-color);stop-opacity:0" />
                </linearGradient>
              </defs>
              
              <!-- Área bajo la línea -->
              <path 
                :d="areaPath" 
                fill="url(#gradientArea)" 
                stroke="none"
              />
              
              <!-- Línea -->
              <path 
                :d="linePath" 
                fill="none" 
                :stroke="getComputedStyle(document.documentElement).getPropertyValue('--success-color')" 
                stroke-width="3"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
              
              <!-- Puntos -->
              <circle 
                v-for="(punto, index) in puntosGrafico" 
                :key="index"
                :cx="punto.x"
                :cy="punto.y"
                r="5"
                :fill="getComputedStyle(document.documentElement).getPropertyValue('--success-color')"
                class="chart-point"
              >
                <title>{{ punto.label }}: ${{ punto.valor.toFixed(2) }}</title>
              </circle>
            </svg>
            <div class="chart-labels">
              <span v-for="(dia, index) in datosGraficoIngresos" :key="index" class="label-x">
                {{ dia.dia }}
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- Servicios Más Populares -->
      <div class="grafico-card full-width">
        <div class="grafico-header">
          <h3><i class="fas fa-cut"></i> Servicios Más Populares</h3>
        </div>
        <div class="grafico-body">
          <div v-if="serviciosPopulares.length === 0" class="chart-placeholder">
            <i class="fas fa-cut"></i>
            <p>No hay datos de servicios</p>
          </div>
          <div v-else class="servicios-ranking">
            <div 
              v-for="(servicio, index) in serviciosPopulares" 
              :key="servicio.id"
              class="servicio-item"
            >
              <div class="ranking-position" :class="'pos-' + (index + 1)">
                {{ index + 1 }}
              </div>
              <div class="servicio-info">
                <h4>{{ servicio.nombre }}</h4>
                <p>{{ servicio.cantidad }} realizaciones</p>
              </div>
              <div class="servicio-stats">
                <div class="progress-bar">
                  <div 
                    class="progress-fill" 
                    :style="{ width: `${(servicio.cantidad / maxServicios) * 100}%` }"
                  ></div>
                </div>
                <span class="porcentaje">{{ ((servicio.cantidad / totalServicios) * 100).toFixed(1) }}%</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Tabla de Rendimiento -->
    <div class="tabla-card">
      <div class="tabla-header">
        <h3><i class="fas fa-table"></i> Rendimiento Detallado</h3>
      </div>
      <div class="tabla-body">
        <table class="data-table">
          <thead>
            <tr>
              <th>Métrica</th>
              <th>Valor Actual</th>
              <th>Periodo Anterior</th>
              <th>Variación</th>
              <th>Tendencia</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td><strong>Reservas Totales</strong></td>
              <td>{{ estadisticas.totalReservas }}</td>
              <td>{{ estadisticas.reservasPeriodoAnterior }}</td>
              <td :class="estadisticas.variacionReservas >= 0 ? 'positivo' : 'negativo'">
                {{ estadisticas.variacionReservas >= 0 ? '+' : '' }}{{ estadisticas.variacionReservas }}%
              </td>
              <td>
                <i v-if="estadisticas.variacionReservas > 5" class="fas fa-arrow-up trend-up"></i>
                <i v-else-if="estadisticas.variacionReservas < -5" class="fas fa-arrow-down trend-down"></i>
                <i v-else class="fas fa-minus trend-stable"></i>
              </td>
            </tr>
            <tr>
              <td><strong>Ingresos Totales</strong></td>
              <td>${{ estadisticas.ingresosTotales?.toFixed(2) }}</td>
              <td>${{ estadisticas.ingresosPeriodoAnterior?.toFixed(2) }}</td>
              <td :class="estadisticas.variacionIngresos >= 0 ? 'positivo' : 'negativo'">
                {{ estadisticas.variacionIngresos >= 0 ? '+' : '' }}{{ estadisticas.variacionIngresos }}%
              </td>
              <td>
                <i v-if="estadisticas.variacionIngresos > 5" class="fas fa-arrow-up trend-up"></i>
                <i v-else-if="estadisticas.variacionIngresos < -5" class="fas fa-arrow-down trend-down"></i>
                <i v-else class="fas fa-minus trend-stable"></i>
              </td>
            </tr>
            <tr>
              <td><strong>Clientes Únicos</strong></td>
              <td>{{ estadisticas.clientesUnicos }}</td>
              <td>{{ estadisticas.clientesPeriodoAnterior }}</td>
              <td :class="estadisticas.variacionClientes >= 0 ? 'positivo' : 'negativo'">
                {{ estadisticas.variacionClientes >= 0 ? '+' : '' }}{{ estadisticas.variacionClientes }}%
              </td>
              <td>
                <i v-if="estadisticas.variacionClientes > 5" class="fas fa-arrow-up trend-up"></i>
                <i v-else-if="estadisticas.variacionClientes < -5" class="fas fa-arrow-down trend-down"></i>
                <i v-else class="fas fa-minus trend-stable"></i>
              </td>
            </tr>
            <tr>
              <td><strong>Tasa de Cancelación</strong></td>
              <td>{{ estadisticas.tasaCancelacion?.toFixed(1) }}%</td>
              <td>{{ estadisticas.tasaCancelacionAnterior?.toFixed(1) }}%</td>
              <td :class="estadisticas.variacionCancelacion <= 0 ? 'positivo' : 'negativo'">
                {{ estadisticas.variacionCancelacion <= 0 ? '-' : '+' }}{{ Math.abs(estadisticas.variacionCancelacion) }}%
              </td>
              <td>
                <i v-if="estadisticas.variacionCancelacion < 0" class="fas fa-arrow-up trend-up"></i>
                <i v-else-if="estadisticas.variacionCancelacion > 0" class="fas fa-arrow-down trend-down"></i>
                <i v-else class="fas fa-minus trend-stable"></i>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';

// Estado
const loading = ref(false);
const periodo = ref('30');
const estadisticas = ref({
  totalReservas: 0,
  reservasPeriodoAnterior: 0,
  variacionReservas: 0,
  ingresosTotales: 0,
  ingresosPeriodoAnterior: 0,
  variacionIngresos: 0,
  clientesUnicos: 0,
  clientesPeriodoAnterior: 0,
  variacionClientes: 0,
  calificacionPromedio: 0,
  totalCalificaciones: 0,
  tasaCancelacion: 0,
  tasaCancelacionAnterior: 0,
  variacionCancelacion: 0
});

const datosGraficoReservas = ref([]);
const datosGraficoIngresos = ref([]);
const serviciosPopulares = ref([]);

// Computadas para gráficos
const maxReservasDia = computed(() => {
  if (!datosGraficoReservas.value.length) return 1;
  return Math.max(...datosGraficoReservas.value.map(d => d.cantidad));
});

const maxServicios = computed(() => {
  if (!serviciosPopulares.value.length) return 1;
  return Math.max(...serviciosPopulares.value.map(s => s.cantidad));
});

const totalServicios = computed(() => {
  return serviciosPopulares.value.reduce((acc, s) => acc + s.cantidad, 0);
});

const areaPath = computed(() => {
  if (datosGraficoIngresos.value.length === 0) return '';
  
  const points = datosGraficoIngresos.value.map((d, i) => {
    const x = (i / (datosGraficoIngresos.value.length - 1)) * 400;
    const maxY = Math.max(...datosGraficoIngresos.value.map(item => item.ingreso));
    const y = 200 - (d.ingreso / maxY) * 180;
    return `${x},${y}`;
  });
  
  return `M0,200 L${points.join(' L')} L400,200 Z`;
});

const linePath = computed(() => {
  if (datosGraficoIngresos.value.length === 0) return '';
  
  const points = datosGraficoIngresos.value.map((d, i) => {
    const x = (i / (datosGraficoIngresos.value.length - 1)) * 400;
    const maxY = Math.max(...datosGraficoIngresos.value.map(item => item.ingreso));
    const y = 200 - (d.ingreso / maxY) * 180;
    return `${x},${y}`;
  });
  
  return `M${points.join(' L')}`;
});

const puntosGrafico = computed(() => {
  if (datosGraficoIngresos.value.length === 0) return [];
  
  const maxY = Math.max(...datosGraficoIngresos.value.map(item => item.ingreso));
  
  return datosGraficoIngresos.value.map((d, i) => ({
    x: (i / (datosGraficoIngresos.value.length - 1)) * 400,
    y: 200 - (d.ingreso / maxY) * 180,
    label: d.dia,
    valor: d.ingreso
  }));
});

// Métodos
const cargarEstadisticas = async () => {
  loading.value = true;
  try {
    // Simulación de datos - Reemplazar con llamada real al servicio
    // const response = await estadisticaService.obtenerPorBarbero(periodo.value);
    
    await new Promise(resolve => setTimeout(resolve, 500)); // Simular delay
    
    // Datos mock para desarrollo
    estadisticas.value = {
      totalReservas: 145,
      reservasPeriodoAnterior: 120,
      variacionReservas: 20.8,
      ingresosTotales: 4350.00,
      ingresosPeriodoAnterior: 3800.00,
      variacionIngresos: 14.5,
      clientesUnicos: 89,
      clientesPeriodoAnterior: 75,
      variacionClientes: 18.7,
      calificacionPromedio: 4.7,
      totalCalificaciones: 67,
      tasaCancelacion: 3.2,
      tasaCancelacionAnterior: 4.5,
      variacionCancelacion: -1.3
    };

    // Datos para gráfico de barras (reservas por día)
    const dias = ['Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb', 'Dom'];
    datosGraficoReservas.value = dias.map(dia => ({
      dia,
      cantidad: Math.floor(Math.random() * 30) + 10
    }));

    // Datos para gráfico de línea (ingresos por día)
    datosGraficoIngresos.value = dias.map(dia => ({
      dia,
      ingreso: Math.floor(Math.random() * 800) + 200
    }));

    // Servicios populares
    serviciosPopulares.value = [
      { id: 1, nombre: 'Corte de Cabello Clásico', cantidad: 85 },
      { id: 2, nombre: 'Barba Completa', cantidad: 62 },
      { id: 3, nombre: 'Corte + Barba', cantidad: 48 },
      { id: 4, nombre: 'Afeitado Tradicional', cantidad: 35 },
      { id: 5, nombre: 'Peinado Infantil', cantidad: 28 }
    ];
  } catch (error) {
    console.error('Error al cargar estadísticas:', error);
    alert('Error al cargar las estadísticas. Intente nuevamente.');
  } finally {
    loading.value = false;
  }
};

const exportarReporte = () => {
  alert('Exportando reporte en PDF/Excel. Funcionalidad en desarrollo.');
  // Aquí se implementaría la lógica para generar y descargar el reporte
};

const getComputedStyle = (el) => {
  // Helper para obtener variables CSS
  return {
    getPropertyValue: (name) => {
      if (name === '--success-color') return '#28a745';
      if (name === '--primary-color') return '#007bff';
      return '';
    }
  };
};

onMounted(() => {
  cargarEstadisticas();
});
</script>

<style scoped>
.estadisticas-container {
  padding: 2rem;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.actions {
  display: flex;
  gap: 1rem;
  align-items: center;
}

.period-selector {
  padding: 0.6rem 1rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 0.9rem;
  background: white;
}

/* Resumen Grid */
.resumen-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.resumen-card {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  display: flex;
  align-items: center;
  gap: 1rem;
}

.resumen-icon {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  flex-shrink: 0;
}

.icon-primary { background: #e3f2fd; color: var(--primary-color); }
.icon-success { background: #e8f5e9; color: var(--success-color); }
.icon-info { background: #e0f7fa; color: var(--info-color); }
.icon-warning { background: #fff3e0; color: var(--warning-color); }

.resumen-info h3 {
  margin: 0 0 0.25rem 0;
  font-size: 1.8rem;
  color: #333;
}

.resumen-info p {
  margin: 0 0 0.5rem 0;
  color: #666;
  font-size: 0.9rem;
}

.variacion {
  font-size: 0.8rem;
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

.variacion.positiva { color: var(--success-color); }
.variacion.negativa { color: var(--danger-color); }
.variacion.neutral { color: #666; }

/* Gráficos Grid */
.graficos-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.grafico-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  overflow: hidden;
}

.grafico-card.full-width {
  grid-column: 1 / -1;
}

.grafico-header {
  padding: 1.5rem;
  border-bottom: 1px solid #eee;
}

.grafico-header h3 {
  margin: 0;
  color: #333;
  font-size: 1.1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.grafico-body {
  padding: 1.5rem;
}

.chart-placeholder {
  text-align: center;
  padding: 3rem;
  color: #999;
}

.chart-placeholder i {
  font-size: 3rem;
  margin-bottom: 1rem;
  display: block;
}

/* Bar Chart */
.bar-chart {
  display: flex;
  justify-content: space-around;
  align-items: flex-end;
  height: 200px;
  padding-top: 1rem;
}

.bar-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
  flex: 1;
  max-width: 60px;
}

.bar-label {
  font-size: 0.8rem;
  color: #666;
}

.bar-container {
  width: 100%;
  height: 150px;
  display: flex;
  align-items: flex-end;
  justify-content: center;
}

.bar-fill {
  width: 100%;
  max-width: 40px;
  background: linear-gradient(to top, var(--primary-color), #5dade2);
  border-radius: 4px 4px 0 0;
  transition: height 0.3s ease;
}

.bar-value {
  font-weight: 600;
  color: #333;
  font-size: 0.9rem;
}

/* Line Chart */
.line-chart {
  position: relative;
}

.line-svg {
  width: 100%;
  height: 200px;
}

.chart-point {
  cursor: pointer;
  transition: r 0.2s;
}

.chart-point:hover {
  r: 7;
}

.chart-labels {
  display: flex;
  justify-content: space-between;
  margin-top: 0.5rem;
  padding: 0 5px;
}

.label-x {
  font-size: 0.75rem;
  color: #666;
}

/* Servicios Ranking */
.servicios-ranking {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.servicio-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 8px;
}

.ranking-position {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  color: white;
  flex-shrink: 0;
}

.pos-1 { background: gold; }
.pos-2 { background: silver; }
.pos-3 { background: #cd7f32; }
.pos-4, .pos-5 { background: #6c757d; }

.servicio-info {
  flex: 1;
}

.servicio-info h4 {
  margin: 0 0 0.25rem 0;
  font-size: 1rem;
  color: #333;
}

.servicio-info p {
  margin: 0;
  font-size: 0.85rem;
  color: #666;
}

.servicio-stats {
  display: flex;
  align-items: center;
  gap: 1rem;
  min-width: 150px;
}

.progress-bar {
  flex: 1;
  height: 8px;
  background: #e9ecef;
  border-radius: 4px;
  overflow: hidden;
}

.progress-fill {
  height: 100%;
  background: var(--primary-color);
  transition: width 0.3s ease;
}

.porcentaje {
  font-weight: 600;
  color: #333;
  font-size: 0.9rem;
  min-width: 45px;
  text-align: right;
}

/* Tabla */
.tabla-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  overflow: hidden;
}

.tabla-header {
  padding: 1.5rem;
  border-bottom: 1px solid #eee;
}

.tabla-header h3 {
  margin: 0;
  color: #333;
  font-size: 1.1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
}

.data-table th, .data-table td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #eee;
}

.data-table th {
  background: #f8f9fa;
  font-weight: 600;
  color: #333;
}

.data-table tr:last-child td {
  border-bottom: none;
}

.data-table tr:hover {
  background: #f8f9fa;
}

.positivo {
  color: var(--success-color);
  font-weight: 600;
}

.negativo {
  color: var(--danger-color);
  font-weight: 600;
}

.trend-up { color: var(--success-color); }
.trend-down { color: var(--danger-color); }
.trend-stable { color: #666; }

/* Buttons */
.btn {
  padding: 0.6rem 1.2rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 500;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  transition: opacity 0.2s;
}

.btn-primary { background: var(--primary-color); color: white; }

@media (max-width: 1024px) {
  .graficos-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 768px) {
  .resumen-grid {
    grid-template-columns: 1fr;
  }
  
  .page-header {
    flex-direction: column;
    gap: 1rem;
    align-items: stretch;
  }
  
  .actions {
    flex-direction: column;
  }
  
  .data-table {
    font-size: 0.85rem;
  }
  
  .data-table th, .data-table td {
    padding: 0.75rem 0.5rem;
  }
}
</style>
