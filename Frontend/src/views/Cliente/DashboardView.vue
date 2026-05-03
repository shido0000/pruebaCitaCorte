<template>
  <div class="view-container">
    <h1 class="text-2xl font-bold text-gray-800 mb-6">Dashboard Cliente</h1>
    
    <!-- Resumen de próxima cita -->
    <div v-if="proximaCita" class="bg-gradient-to-r from-blue-500 to-blue-600 rounded-lg shadow-lg p-6 text-white mb-6">
      <h2 class="text-xl font-semibold mb-4">Tu próxima cita</h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div>
          <p class="text-blue-100 text-sm">Fecha y hora</p>
          <p class="font-bold text-lg">{{ formatearFecha(proximaCita.fechaHoraInicio) }}</p>
        </div>
        <div>
          <p class="text-blue-100 text-sm">Profesional</p>
          <p class="font-bold">{{ proximaCita.proveedorNombre }}</p>
        </div>
        <div>
          <p class="text-blue-100 text-sm">Servicio</p>
          <p class="font-bold">{{ proximaCita.servicioNombre }}</p>
        </div>
      </div>
      <div class="mt-4 flex gap-2">
        <button @click="verDetalleReserva(proximaCita.id)" class="bg-white text-blue-600 px-4 py-2 rounded-lg hover:bg-blue-50 transition-colors">
          Ver detalles
        </button>
        <button @click="cancelarReserva(proximaCita.id)" class="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 transition-colors">
          Cancelar
        </button>
      </div>
    </div>

    <!-- Accesos rápidos -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
      <router-link to="/cliente/reservas" class="bg-white p-6 rounded-lg shadow hover:shadow-md transition-shadow">
        <div class="flex items-center gap-3">
          <div class="bg-blue-100 p-3 rounded-full">
            <svg class="w-6 h-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"></path>
            </svg>
          </div>
          <div>
            <p class="text-gray-500 text-sm">Mis Reservas</p>
            <p class="text-2xl font-bold text-gray-800">{{ reservasPendientes }}</p>
          </div>
        </div>
      </router-link>

      <router-link to="/cliente/historial" class="bg-white p-6 rounded-lg shadow hover:shadow-md transition-shadow">
        <div class="flex items-center gap-3">
          <div class="bg-green-100 p-3 rounded-full">
            <svg class="w-6 h-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
            </svg>
          </div>
          <div>
            <p class="text-gray-500 text-sm">Historial</p>
            <p class="text-2xl font-bold text-gray-800">{{ totalHistorial }}</p>
          </div>
        </div>
      </router-link>

      <router-link to="/cliente/explorar" class="bg-white p-6 rounded-lg shadow hover:shadow-md transition-shadow">
        <div class="flex items-center gap-3">
          <div class="bg-purple-100 p-3 rounded-full">
            <svg class="w-6 h-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
            </svg>
          </div>
          <div>
            <p class="text-gray-500 text-sm">Explorar</p>
            <p class="text-sm font-semibold text-gray-800">Buscar profesionales</p>
          </div>
        </div>
      </router-link>

      <router-link to="/cliente/perfil" class="bg-white p-6 rounded-lg shadow hover:shadow-md transition-shadow">
        <div class="flex items-center gap-3">
          <div class="bg-orange-100 p-3 rounded-full">
            <svg class="w-6 h-6 text-orange-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path>
            </svg>
          </div>
          <div>
            <p class="text-gray-500 text-sm">Mi Perfil</p>
            <p class="text-sm font-semibold text-gray-800">Editar datos</p>
          </div>
        </div>
      </router-link>
    </div>

    <!-- Promociones destacadas -->
    <div class="bg-white rounded-lg shadow p-6 mb-6">
      <h2 class="text-xl font-semibold text-gray-800 mb-4">Promociones Destacadas</h2>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div v-for="promo in promociones" :key="promo.id" class="border border-gray-200 rounded-lg p-4 hover:border-blue-300 transition-colors cursor-pointer">
          <div class="flex justify-between items-start">
            <div>
              <h3 class="font-semibold text-gray-800">{{ promo.titulo }}</h3>
              <p class="text-gray-600 text-sm mt-1">{{ promo.descripcion }}</p>
              <p class="text-blue-600 font-bold mt-2">{{ promo.descuento }}% OFF</p>
            </div>
            <span class="bg-red-100 text-red-600 text-xs px-2 py-1 rounded">Limitado</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Profesionales favoritos -->
    <div class="bg-white rounded-lg shadow p-6">
      <h2 class="text-xl font-semibold text-gray-800 mb-4">Tus Profesionales Favoritos</h2>
      <div v-if="favoritos.length > 0" class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div v-for="fav in favoritos" :key="fav.id" class="flex items-center gap-3 border border-gray-200 rounded-lg p-3 hover:shadow-md transition-shadow">
          <img :src="fav.fotoUrl || 'https://via.placeholder.com/50'" :alt="fav.nombre" class="w-12 h-12 rounded-full object-cover">
          <div class="flex-1">
            <p class="font-semibold text-gray-800">{{ fav.nombre }}</p>
            <p class="text-gray-500 text-sm">{{ fav.especialidad }}</p>
          </div>
          <router-link :to="`/cliente/reservas?barbero=${fav.id}`" class="text-blue-600 hover:text-blue-800">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6"></path>
            </svg>
          </router-link>
        </div>
      </div>
      <div v-else class="text-gray-500 text-center py-4">
        <p>Aún no tienes profesionales favoritos</p>
        <router-link to="/cliente/explorar" class="text-blue-600 hover:underline mt-2 inline-block">Explorar ahora</router-link>
      </div>
    </div>

    <!-- Estado de carga -->
    <div v-if="cargando" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white p-6 rounded-lg">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
        <p class="mt-4 text-gray-600">Cargando...</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import dashboardClienteService from '@/services/dashboardClienteService';
import reservaService from '@/services/reservaService';

const router = useRouter();
const authStore = useAuthStore();

const cargando = ref(false);
const proximaCita = ref(null);
const reservasPendientes = ref(0);
const totalHistorial = ref(0);
const favoritos = ref([]);
const promociones = ref([
  { id: 1, titulo: '20% OFF en Corte + Barba', descripcion: 'Válido todos los martes', descuento: 20 },
  { id: 2, titulo: 'Pack Premium con Descuento', descripcion: 'Corte, barba y tratamiento facial', descuento: 25 }
]);

const formatearFecha = (fecha) => {
  if (!fecha) return '';
  const date = new Date(fecha);
  return date.toLocaleDateString('es-ES', { 
    weekday: 'long', 
    year: 'numeric', 
    month: 'long', 
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
};

const cargarDatosDashboard = async () => {
  cargando.value = true;
  try {
    const usuarioId = authStore.usuario?.id;
    if (!usuarioId) return;

    // Cargar próxima cita
    const reservas = await reservaService.obtenerPorCliente(usuarioId);
    const pendientes = reservas.filter(r => r.estado === 'Pendiente' || r.estado === 'Confirmada');
    reservasPendientes.value = pendientes.length;
    
    if (pendientes.length > 0) {
      proximaCita.value = pendientes.sort((a, b) => 
        new Date(a.fechaHoraInicio) - new Date(b.fechaHoraInicio)
      )[0];
    }

    // Cargar historial
    const completadas = reservas.filter(r => r.estado === 'Completada');
    totalHistorial.value = completadas.length;

    // Cargar favoritos (simulado por ahora)
    favoritos.value = [];
  } catch (error) {
    console.error('Error cargando dashboard:', error);
  } finally {
    cargando.value = false;
  }
};

const verDetalleReserva = (reservaId) => {
  router.push(`/cliente/reservas?id=${reservaId}`);
};

const cancelarReserva = async (reservaId) => {
  if (!confirm('¿Estás seguro de que deseas cancelar esta reserva?')) return;
  
  try {
    await reservaService.cancelar(reservaId);
    alert('Reserva cancelada exitosamente');
    cargarDatosDashboard();
  } catch (error) {
    alert('Error al cancelar la reserva: ' + error.message);
  }
};

onMounted(() => {
  cargarDatosDashboard();
});
</script>

<style scoped>
.view-container {
  padding: 2rem;
  max-width: 1280px;
  margin: 0 auto;
}
</style>
