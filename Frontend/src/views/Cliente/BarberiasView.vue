<template>
  <div class="barberias-view">
    <div class="page-header">
      <h1><i class="fas fa-store"></i> Barberías Disponibles</h1>
      <p>Explora y encuentra la barbería perfecta para tu estilo</p>
    </div>
    <div class="grid-container">
      <div v-for="b in barberias" :key="b.id" class="card">
        <h3>{{ b.nombre }}</h3>
        <p>{{ b.direccion }}</p>
        <button @click="reservar(b)">Reservar</button>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref } from 'vue';
const barberias = ref([
  { id: 1, nombre: "The Gentleman's Cut", direccion: "Av. Principal 123" },
  { id: 2, nombre: "Urban Cuts", direccion: "Calle Moderno 45" }
]);
const reservar = (b) => console.log('Reservar en', b.nombre);
</script>
<style scoped>
.barberias-view { padding: 2rem; }
.page-header { margin-bottom: 2rem; }
.grid-container { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 1.5rem; }
.card { background: white; padding: 1.5rem; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.card h3 { margin-bottom: 0.5rem; color: #1a1a1a; }
.card p { color: #666; margin-bottom: 1rem; }
.card button { background: #3b82f6; color: white; border: none; padding: 0.75rem 1.5rem; border-radius: 8px; cursor: pointer; }
</style>
