<template>
  <div class="solicitudes-view">
    <h1><i class="fas fa-user-plus"></i> Solicitudes de Afiliación</h1>
    <div class="tabs">
      <button :class="{ active: tab === 'pendientes' }" @click="tab = 'pendientes'">Pendientes</button>
      <button :class="{ active: tab === 'aprobadas' }" @click="tab = 'aprobadas'">Aprobadas</button>
      <button :class="{ active: tab === 'rechazadas' }" @click="tab = 'rechazadas'">Rechazadas</button>
    </div>
    <div class="lista-solicitudes">
      <div v-for="s in solicitudesFiltradas" :key="s.id" class="solicitud-card">
        <div class="solicitud-header">
          <h3>{{ s.nombre }}</h3>
          <span class="estado" :class="s.estado">{{ s.estado }}</span>
        </div>
        <p><strong>Email:</strong> {{ s.email }}</p>
        <p><strong>Experiencia:</strong> {{ s.experiencia }} años</p>
        <p><strong>Especialidad:</strong> {{ s.especialidad }}</p>
        <div class="acciones" v-if="s.estado === 'pendiente'">
          <button class="btn-aprobar" @click="aprobar(s)">Aprobar</button>
          <button class="btn-rechazar" @click="rechazar(s)">Rechazar</button>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref, computed } from 'vue';
const tab = ref('pendientes');
const solicitudes = ref([
  { id: 1, nombre: "Carlos García", email: "carlos@email.com", experiencia: 5, especialidad: "Cortes Clásicos", estado: "pendiente" },
  { id: 2, nombre: "Miguel Rodríguez", email: "miguel@email.com", experiencia: 3, especialidad: "Barbas", estado: "pendiente" },
  { id: 3, nombre: "Luis Martínez", email: "luis@email.com", experiencia: 7, especialidad: "Estilos Modernos", estado: "aprobada" }
]);
const solicitudesFiltradas = computed(() => solicitudes.value.filter(s => s.estado === tab.value));
const aprobar = (s) => { s.estado = 'aprobada'; alert(`Solicitud de ${s.nombre} aprobada`); };
const rechazar = (s) => { s.estado = 'rechazada'; alert(`Solicitud de ${s.nombre} rechazada`); };
</script>
<style scoped>
.solicitudes-view { padding: 2rem; }
.tabs { display: flex; gap: 1rem; margin-bottom: 2rem; }
.tabs button { padding: 0.75rem 1.5rem; border: none; background: #f3f4f6; border-radius: 8px; cursor: pointer; }
.tabs button.active { background: #3b82f6; color: white; }
.lista-solicitudes { display: flex; flex-direction: column; gap: 1rem; }
.solicitud-card { background: white; padding: 1.5rem; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.solicitud-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; }
.estado { padding: 0.4rem 0.8rem; border-radius: 20px; font-size: 0.85rem; font-weight: 600; }
.estado.pendiente { background: #fef3c7; color: #92400e; }
.estado.aprobada { background: #d1fae5; color: #065f46; }
.estado.rechazada { background: #fee2e2; color: #991b1b; }
.acciones { display: flex; gap: 0.75rem; margin-top: 1rem; }
.btn-aprobar { background: #10b981; color: white; border: none; padding: 0.5rem 1rem; border-radius: 6px; cursor: pointer; }
.btn-rechazar { background: #ef4444; color: white; border: none; padding: 0.5rem 1rem; border-radius: 6px; cursor: pointer; }
</style>
