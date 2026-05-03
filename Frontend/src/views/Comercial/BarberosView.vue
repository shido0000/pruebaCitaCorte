<template>
  <div class="view-container">
    <h1>Mis Barberos Asignados</h1>
    
    <!-- Estadísticas -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon"><i class="fas fa-users"></i></div>
        <div class="stat-info">
          <h3>{{ totalBarberos }}</h3>
          <p>Total Barberos</p>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon success"><i class="fas fa-check-circle"></i></div>
        <div class="stat-info">
          <h3>{{ activosCount }}</h3>
          <p>Activos</p>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon warning"><i class="fas fa-star"></i></div>
        <div class="stat-info">
          <h3>{{ promedioCalificacion }}</h3>
          <p>Calificación Prom.</p>
        </div>
      </div>
    </div>

    <!-- Lista -->
    <div class="lista-barberos">
      <div v-for="barbero in barberos" :key="barbero.id" class="card-barbero">
        <div class="card-header">
          <div class="avatar">{{ obtenerIniciales(barbero.nombre) }}</div>
          <div>
            <h3>{{ barbero.nombre }}</h3>
            <p class="texto-suave">{{ barbero.email }}</p>
          </div>
        </div>
        <div class="card-body">
          <div class="info-row">
            <span><i class="fas fa-calendar"></i> {{ barbero.totalReservas || 0 }} reservas</span>
            <span><i class="fas fa-star"></i> {{ barbero.calificacionPromedio?.toFixed(1) || '0.0' }}</span>
          </div>
          <div class="plan-badge">{{ barbero.planSuscripcion?.tipoPlan || 'Sin Plan' }}</div>
        </div>
        <div class="card-footer">
          <button @click="verDetalle(barbero)" class="btn btn-primary btn-sm">Ver Detalles</button>
        </div>
      </div>
      
      <div v-if="barberos.length === 0 && !cargando" class="sin-datos">
        <i class="fas fa-user-slash"></i>
        <p>No tienes barberos asignados</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useBarberoStore } from '@/stores/barberoStore';

const barberoStore = useBarberoStore();
const barberos = ref([]);
const cargando = ref(false);

const totalBarberos = computed(() => barberos.value.length);
const activosCount = computed(() => barberos.value.filter(b => b.activo).length);
const promedioCalificacion = computed(() => {
  const suma = barberos.value.reduce((acc, b) => acc + (b.calificacionPromedio || 0), 0);
  return barberos.value.length ? (suma / barberos.value.length).toFixed(1) : '0.0';
});

const cargarBarberos = async () => {
  cargando.value = true;
  try {
    await barberoStore.obtenerTodos({});
    barberos.value = barberoStore.barberos;
  } catch (error) {
    console.error('Error:', error);
  } finally {
    cargando.value = false;
  }
};

const verDetalle = (barbero) => {
  alert(`Detalles de ${barbero.nombre}`);
};

const obtenerIniciales = (nombre) => nombre ? nombre.split(' ').slice(0,2).map(p=>p[0]).join('').toUpperCase() : '';

onMounted(() => { cargarBarberos(); });
</script>

<style scoped>
.view-container { padding: 2rem; }
h1 { margin-bottom: 2rem; color: #333; }

.stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1.5rem; margin-bottom: 2rem; }
.stat-card { background: white; border-radius: 8px; padding: 1.5rem; display: flex; align-items: center; gap: 1rem; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
.stat-icon { width: 50px; height: 50px; border-radius: 50%; background: #667eea; color: white; display: flex; align-items: center; justify-content: center; font-size: 1.5rem; }
.stat-icon.success { background: #28a745; }
.stat-icon.warning { background: #ffc107; }
.stat-info h3 { margin: 0; font-size: 1.75rem; color: #333; }
.stat-info p { margin: 0; color: #666; font-size: 0.875rem; }

.lista-barberos { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.5rem; }
.card-barbero { background: white; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
.card-header { display: flex; align-items: center; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); }
.avatar { width: 50px; height: 50px; border-radius: 50%; background: white; color: #667eea; display: flex; align-items: center; justify-content: center; font-weight: bold; }
.card-header h3 { margin: 0; color: white; font-size: 1.1rem; }
.card-header p { margin: 0; color: rgba(255,255,255,0.8); font-size: 0.875rem; }
.card-body { padding: 1.5rem; }
.info-row { display: flex; justify-content: space-between; margin-bottom: 1rem; color: #666; font-size: 0.875rem; }
.plan-badge { display: inline-block; padding: 0.25rem 0.75rem; background: #e9ecef; border-radius: 20px; font-size: 0.75rem; font-weight: 600; }
.card-footer { padding: 1rem 1.5rem; background: #f8f9fa; border-top: 1px solid #e9ecef; text-align: center; }

.btn { padding: 0.5rem 1rem; border: none; border-radius: 4px; cursor: pointer; font-weight: 600; }
.btn-sm { padding: 0.25rem 0.5rem; font-size: 0.875rem; }
.btn-primary { background: #667eea; color: white; }

.sin-datos { text-align: center; padding: 4rem; color: #666; }
.sin-datos i { font-size: 4rem; margin-bottom: 1rem; color: #ddd; }
.texto-suave { color: #666; font-size: 0.875rem; }
</style>
