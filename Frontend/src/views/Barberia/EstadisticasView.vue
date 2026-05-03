<template>
  <div class="estadisticas-view">
    <h1><i class="fas fa-chart-line"></i> Estadísticas</h1>
    <div class="stats-grid">
      <div class="stat-card">
        <h3>Ingresos del Mes</h3>
        <p class="valor">$4,250</p>
        <span class="variacion positive">+12% vs mes anterior</span>
      </div>
      <div class="stat-card">
        <h3>Reservas Totales</h3>
        <p class="valor">156</p>
        <span class="variacion positive">+8% vs mes anterior</span>
      </div>
      <div class="stat-card">
        <h3>Clientes Nuevos</h3>
        <p class="valor">42</p>
        <span class="variacion positive">+15% vs mes anterior</span>
      </div>
      <div class="stat-card">
        <h3>Calificación Promedio</h3>
        <p class="valor">4.8 ⭐</p>
        <span class="variacion neutral">Sin cambios</span>
      </div>
    </div>
    <div class="charts-section">
      <div class="chart-card">
        <h3>Reservas por Día</h3>
        <div class="chart-placeholder">Gráfico de barras - Implementar con Chart.js</div>
      </div>
      <div class="chart-card">
        <h3>Ingresos por Servicio</h3>
        <div class="chart-placeholder">Gráfico circular - Implementar con Chart.js</div>
      </div>
    </div>
  </div>
</template>
<script setup>
// Vista de estadísticas con métricas clave
</script>
<style scoped>
.estadisticas-view { padding: 2rem; }
.stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 1.5rem; margin-bottom: 2rem; }
.stat-card { background: white; padding: 1.5rem; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.stat-card h3 { color: #666; font-size: 0.9rem; margin-bottom: 0.5rem; }
.stat-card .valor { font-size: 2rem; font-weight: 700; color: #1a1a1a; }
.variacion { font-size: 0.85rem; }
.variacion.positive { color: #10b981; }
.variacion.negative { color: #ef4444; }
.variacion.neutral { color: #666; }
.charts-section { display: grid; grid-template-columns: repeat(auto-fit, minmax(400px, 1fr)); gap: 1.5rem; }
.chart-card { background: white; padding: 1.5rem; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.chart-placeholder { height: 250px; background: #f3f4f6; border-radius: 8px; display: flex; align-items: center; justify-content: center; color: #999; }
</style>
