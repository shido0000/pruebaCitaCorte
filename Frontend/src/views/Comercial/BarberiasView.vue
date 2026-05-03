<template>
  <div class="view-container">
    <h1>Mis Barberías Asignadas</h1>
    
    <!-- Estadísticas -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon"><i class="fas fa-store"></i></div>
        <div class="stat-info">
          <h3>{{ totalBarberias }}</h3>
          <p>Total Barberías</p>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon success"><i class="fas fa-users"></i></div>
        <div class="stat-info">
          <h3>{{ totalBarberos }}</h3>
          <p>Barberos Totales</p>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon warning"><i class="fas fa-chart-line"></i></div>
        <div class="stat-info">
          <h3>{{ crecimiento }}%</h3>
          <p>Crecimiento</p>
        </div>
      </div>
    </div>

    <!-- Grid -->
    <div class="grid-barberias">
      <div v-for="barberia in barberias" :key="barberia.id" class="card-barberia">
        <div class="card-header">
          <div class="avatar">{{ obtenerIniciales(barberia.nombreComercial) }}</div>
          <span :class="['badge', barberia.activo ? 'badge-success' : 'badge-danger']">
            {{ barberia.activo ? 'Activo' : 'Inactivo' }}
          </span>
        </div>
        <div class="card-body">
          <h3>{{ barberia.nombreComercial }}</h3>
          <p class="ubicacion"><i class="fas fa-map-marker-alt"></i> {{ barberia.ciudad || 'N/A' }}</p>
          <div class="metricas">
            <div class="metrica">
              <label>Barberos</label>
              <span>{{ barberia.totalBarberos || 0 }}</span>
            </div>
            <div class="metrica">
              <label>Calificación</label>
              <span><i class="fas fa-star"></i> {{ barberia.calificacionPromedio?.toFixed(1) || '0.0' }}</span>
            </div>
          </div>
        </div>
        <div class="card-footer">
          <button @click="verDetalle(barberia)" class="btn btn-primary btn-sm">Ver Detalles</button>
        </div>
      </div>
      
      <div v-if="barberias.length === 0 && !cargando" class="sin-datos">
        <i class="fas fa-store-slash"></i>
        <p>No tienes barberías asignadas</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useBarberiaStore } from '@/stores/barberiaStore';

const barberiaStore = useBarberiaStore();
const barberias = ref([]);
const cargando = ref(false);

const totalBarberias = computed(() => barberias.value.length);
const totalBarberos = computed(() => barberias.value.reduce((acc, b) => acc + (b.totalBarberos || 0), 0));
const crecimiento = ref(15);

const cargarBarberias = async () => {
  cargando.value = true;
  try {
    await barberiaStore.obtenerTodos({});
    barberias.value = barberiaStore.barberias;
  } catch (error) {
    console.error('Error:', error);
  } finally {
    cargando.value = false;
  }
};

const verDetalle = (barberia) => {
  alert(`Detalles de ${barberia.nombreComercial}`);
};

const obtenerIniciales = (nombre) => nombre ? nombre.split(' ').slice(0,2).map(p=>p[0]).join('').toUpperCase() : '';

onMounted(() => { cargarBarberias(); });
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

.grid-barberias { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.5rem; }
.card-barberia { background: white; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
.card-header { display: flex; justify-content: space-between; align-items: center; padding: 1.5rem; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); }
.avatar { width: 50px; height: 50px; border-radius: 50%; background: white; color: #667eea; display: flex; align-items: center; justify-content: center; font-weight: bold; }
.badge { padding: 0.25rem 0.75rem; border-radius: 20px; font-size: 0.75rem; font-weight: 600; }
.badge-success { background: #d4edda; color: #155724; }
.badge-danger { background: #f8d7da; color: #721c24; }
.card-body { padding: 1.5rem; }
.card-body h3 { margin: 0 0 0.5rem 0; color: #333; }
.ubicacion { margin: 0 0 1rem 0; color: #666; font-size: 0.875rem; }
.metricas { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; }
.metrica { text-align: center; padding: 0.75rem; background: #f8f9fa; border-radius: 4px; }
.metrica label { display: block; font-size: 0.75rem; color: #666; margin-bottom: 0.25rem; }
.metrica span { font-weight: 600; color: #333; }
.card-footer { padding: 1rem 1.5rem; background: #f8f9fa; border-top: 1px solid #e9ecef; text-align: center; }

.btn { padding: 0.5rem 1rem; border: none; border-radius: 4px; cursor: pointer; font-weight: 600; }
.btn-sm { padding: 0.25rem 0.5rem; font-size: 0.875rem; }
.btn-primary { background: #667eea; color: white; }

.sin-datos { text-align: center; padding: 4rem; color: #666; }
.sin-datos i { font-size: 4rem; margin-bottom: 1rem; color: #ddd; }
</style>
