<template>
  <div class="notificaciones-container">
    <div class="header">
      <h1>Notificaciones</h1>
      <button 
        @click="marcarTodasLeidas" 
        class="btn-marcar-todas"
        :disabled="noLeidasCount === 0 || loading"
      >
        Marcar todas como leídas
      </button>
    </div>

    <div v-if="loading && notificaciones.length === 0" class="loading">
      Cargando notificaciones...
    </div>

    <div v-else-if="notificaciones.length === 0" class="empty-state">
      <span class="empty-icon">🔔</span>
      <p>No tienes notificaciones</p>
    </div>

    <div v-else class="notificaciones-list">
      <div 
        v-for="notificacion in notificaciones" 
        :key="notificacion.id"
        class="notificacion-item"
        :class="{ 'leida': notificacion.leida }"
      >
        <div class="notificacion-content">
          <div class="notificacion-icon">{{ getIconoTipo(notificacion.tipo) }}</div>
          <div class="notificacion-info">
            <h3>{{ notificacion.titulo }}</h3>
            <p>{{ notificacion.mensaje }}</p>
            <span class="notificacion-fecha">{{ formatearFecha(notificacion.fechaCreacion) }}</span>
          </div>
        </div>
        <div class="notificacion-actions">
          <button 
            v-if="!notificacion.leida" 
            @click="marcarComoLeida(notificacion.id)"
            class="btn-leida"
          >
            ✓
          </button>
          <button 
            @click="eliminarNotificacion(notificacion.id)"
            class="btn-eliminar"
          >
            ✕
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, computed } from 'vue'
import { useNotificacionStore } from '../stores/notificacionStore'

const notificacionStore = useNotificacionStore()

const notificaciones = computed(() => notificacionStore.notificaciones)
const noLeidasCount = computed(() => notificacionStore.noLeidasCount)
const loading = computed(() => notificacionStore.loading)

onMounted(() => {
  notificacionStore.cargarNotificaciones()
})

function getIconoTipo(tipo) {
  const iconos = {
    'SolicitudAfiliacionNueva': '📩',
    'SolicitudAfiliacionAprobada': '✅',
    'SolicitudAfiliacionRechazada': '❌',
    'ReservaNueva': '📅',
    'ReservaConfirmada': '✓',
    'ReservaCancelada': '✕',
    'SuscripcionPorVencer': '⏰',
    'SuscripcionVencida': '⚠️'
  }
  return iconos[tipo] || '🔔'
}

function formatearFecha(fecha) {
  return new Date(fecha).toLocaleDateString('es-ES', {
    day: 'numeric',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

async function marcarComoLeida(id) {
  await notificacionStore.marcarComoLeida(id)
}

async function marcarTodasLeidas() {
  await notificacionStore.marcarTodasComoLeidas()
}

async function eliminarNotificacion(id) {
  if (confirm('¿Estás seguro de eliminar esta notificación?')) {
    await notificacionStore.eliminarNotificacion(id)
  }
}
</script>

<style scoped>
.notificaciones-container {
  padding: 2rem;
  max-width: 800px;
  margin: 0 auto;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.header h1 {
  margin: 0;
  color: #1a1a2e;
}

.btn-marcar-todas {
  background-color: #e94560;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 0.375rem;
  cursor: pointer;
  font-size: 0.875rem;
  transition: background-color 0.2s;
}

.btn-marcar-todas:hover:not(:disabled) {
  background-color: #d63850;
}

.btn-marcar-todas:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.loading, .empty-state {
  text-align: center;
  padding: 3rem;
  color: #6b7280;
}

.empty-icon {
  font-size: 3rem;
  display: block;
  margin-bottom: 1rem;
}

.notificaciones-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.notificacion-item {
  background: white;
  border-radius: 0.5rem;
  padding: 1rem;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-left: 4px solid #e94560;
}

.notificacion-item.leida {
  opacity: 0.7;
  border-left-color: #9ca3af;
}

.notificacion-content {
  display: flex;
  gap: 1rem;
  flex: 1;
}

.notificacion-icon {
  font-size: 1.5rem;
}

.notificacion-info h3 {
  margin: 0 0 0.25rem 0;
  font-size: 1rem;
  color: #1a1a2e;
}

.notificacion-info p {
  margin: 0 0 0.5rem 0;
  color: #6b7280;
  font-size: 0.875rem;
}

.notificacion-fecha {
  font-size: 0.75rem;
  color: #9ca3af;
}

.notificacion-actions {
  display: flex;
  gap: 0.5rem;
}

.btn-leida, .btn-eliminar {
  background: none;
  border: none;
  cursor: pointer;
  padding: 0.25rem 0.5rem;
  border-radius: 0.25rem;
  font-size: 1rem;
  transition: background-color 0.2s;
}

.btn-leida {
  color: #10b981;
}

.btn-leida:hover {
  background-color: #d1fae5;
}

.btn-eliminar {
  color: #ef4444;
}

.btn-eliminar:hover {
  background-color: #fee2e2;
}
</style>
