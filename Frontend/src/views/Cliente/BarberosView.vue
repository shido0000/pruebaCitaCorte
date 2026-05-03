<template>
  <div class="barberos-view">
    <div class="page-header">
      <h1><i class="fas fa-user-tie"></i> Barberos</h1>
      <p>Elige tu barbero de confianza</p>
    </div>
    <div class="grid-container">
      <div v-for="barbero in barberos" :key="barbero.id" class="card">
        <div class="avatar">{{ barbero.nombre.charAt(0) }}</div>
        <h3>{{ barbero.nombre }}</h3>
        <div class="rating">
          <i class="fas fa-star"></i> {{ barbero.calificacion }}
        </div>
        <p class="especialidad">{{ barbero.especialidad }}</p>
        <button @click="reservar(barbero)">Reservar con {{ barbero.nombre.split(' ')[0] }}</button>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref } from 'vue';
const barberos = ref([
  { id: 1, nombre: "Carlos García", calificacion: 4.8, especialidad: "Cortes Clásicos" },
  { id: 2, nombre: "Miguel Rodríguez", calificacion: 4.9, especialidad: "Barbas y Afeitados" },
  { id: 3, nombre: "Luis Martínez", calificacion: 4.7, especialidad: "Estilos Modernos" }
]);
const reservar = (b) => console.log('Reservar con', b.nombre);
</script>
<style scoped>
.barberos-view { padding: 2rem; }
.page-header { margin-bottom: 2rem; }
.grid-container { display: grid; grid-template-columns: repeat(auto-fill, minmax(250px, 1fr)); gap: 1.5rem; }
.card { background: white; padding: 2rem; border-radius: 12px; text-align: center; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.avatar { width: 80px; height: 80px; background: #3b82f6; color: white; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-size: 2rem; margin: 0 auto 1rem; }
.card h3 { margin-bottom: 0.5rem; }
.rating { color: #fbbf24; margin-bottom: 0.5rem; }
.especialidad { color: #666; margin-bottom: 1rem; }
.card button { background: #3b82f6; color: white; border: none; padding: 0.75rem 1.5rem; border-radius: 8px; cursor: pointer; width: 100%; }
</style>
