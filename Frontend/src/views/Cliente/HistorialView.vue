<template>
  <div class="historial-view">
    <h1><i class="fas fa-history"></i> Historial de Reservas</h1>
    <div class="tabs">
      <button :class="{ active: tab === 'pasadas' }" @click="tab = 'pasadas'">Pasadas</button>
      <button :class="{ active: tab === 'canceladas' }" @click="tab = 'canceladas'">Canceladas</button>
    </div>
    <div class="lista-reservas">
      <div v-for="r in reservasFiltradas" :key="r.id" class="reserva-card">
        <div class="reserva-header">
          <h3>{{ r.servicio }}</h3>
          <span class="estado" :class="r.estado">{{ r.estado }}</span>
        </div>
        <p><strong>Barbería:</strong> {{ r.barberia }}</p>
        <p><strong>Barbero:</strong> {{ r.barbero }}</p>
        <p><strong>Fecha:</strong> {{ r.fecha }} a las {{ r.hora }}</p>
        <p class="precio">Total: ${{ r.precio }}</p>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref, computed } from 'vue';
const tab = ref('pasadas');
const reservas = ref([
  { id: 1, servicio: "Corte Clásico", barberia: "The Gentleman's Cut", barbero: "Carlos García", fecha: "2024-01-15", hora: "10:00", precio: 25, estado: "completada" },
  { id: 2, servicio: "Barba Completa", barberia: "Urban Cuts", barbero: "Miguel Rodríguez", fecha: "2024-01-10", hora: "15:30", precio: 15, estado: "completada" },
  { id: 3, servicio: "Corte Fade", barberia: "Classic Shave", barbero: "Luis Martínez", fecha: "2024-01-05", hora: "12:00", precio: 30, estado: "cancelada" }
]);
const reservasFiltradas = computed(() => {
  if (tab.value === 'pasadas') return reservas.value.filter(r => r.estado === 'completada');
  return reservas.value.filter(r => r.estado === 'cancelada');
});
</script>
<style scoped>
.historial-view { padding: 2rem; }
.tabs { display: flex; gap: 1rem; margin-bottom: 2rem; }
.tabs button { padding: 0.75rem 1.5rem; border: none; background: #f3f4f6; border-radius: 8px; cursor: pointer; }
.tabs button.active { background: #3b82f6; color: white; }
.lista-reservas { display: flex; flex-direction: column; gap: 1rem; }
.reserva-card { background: white; padding: 1.5rem; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.reserva-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; }
.estado { padding: 0.4rem 0.8rem; border-radius: 20px; font-size: 0.85rem; font-weight: 600; }
.estado.completada { background: #d1fae5; color: #065f46; }
.estado.cancelada { background: #fee2e2; color: #991b1b; }
.precio { font-size: 1.2rem; font-weight: 700; color: #10b981; margin-top: 1rem; }
</style>
