<template>
  <div class="reservas-view">
    <h1><i class="fas fa-calendar-alt"></i> Mis Reservas</h1>
    <div class="tabs">
      <button :class="{ active: tab === 'pendientes' }" @click="tab = 'pendientes'">Pendientes</button>
      <button :class="{ active: tab === 'confirmadas' }" @click="tab = 'confirmadas'">Confirmadas</button>
      <button :class="{ active: tab === 'completadas' }" @click="tab = 'completadas'">Completadas</button>
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
        <div class="reserva-actions">
          <button class="btn-cancelar" v-if="r.estado === 'pendiente'">Cancelar</button>
          <button class="btn-ver">Ver Detalles</button>
        </div>
      </div>
    </div>
    <div v-if="reservasFiltradas.length === 0" class="empty-state">
      <p>No hay reservas en esta categoría</p>
    </div>
  </div>
</template>
<script setup>
import { ref, computed } from 'vue';
const tab = ref('pendientes');
const reservas = ref([
  { id: 1, servicio: "Corte Clásico", barberia: "The Gentleman's Cut", barbero: "Carlos García", fecha: "2024-02-15", hora: "10:00", estado: "pendiente" },
  { id: 2, servicio: "Barba Completa", barberia: "Urban Cuts", barbero: "Miguel Rodríguez", fecha: "2024-02-20", hora: "15:30", estado: "confirmada" },
  { id: 3, servicio: "Corte Fade", barberia: "Classic Shave", barbero: "Luis Martínez", fecha: "2024-01-05", hora: "12:00", estado: "completada" }
]);
const reservasFiltradas = computed(() => reservas.value.filter(r => r.estado === tab.value));
</script>
<style scoped>
.reservas-view { padding: 2rem; }
.tabs { display: flex; gap: 1rem; margin-bottom: 2rem; }
.tabs button { padding: 0.75rem 1.5rem; border: none; background: #f3f4f6; border-radius: 8px; cursor: pointer; }
.tabs button.active { background: #3b82f6; color: white; }
.lista-reservas { display: flex; flex-direction: column; gap: 1rem; }
.reserva-card { background: white; padding: 1.5rem; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.reserva-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; }
.estado { padding: 0.4rem 0.8rem; border-radius: 20px; font-size: 0.85rem; font-weight: 600; }
.estado.pendiente { background: #fef3c7; color: #92400e; }
.estado.confirmada { background: #dbeafe; color: #1e40af; }
.estado.completada { background: #d1fae5; color: #065f46; }
.reserva-actions { display: flex; gap: 0.75rem; margin-top: 1rem; }
.btn-cancelar { background: #ef4444; color: white; border: none; padding: 0.5rem 1rem; border-radius: 6px; cursor: pointer; }
.btn-ver { background: #3b82f6; color: white; border: none; padding: 0.5rem 1rem; border-radius: 6px; cursor: pointer; }
.empty-state { text-align: center; padding: 3rem; color: #999; }
</style>
