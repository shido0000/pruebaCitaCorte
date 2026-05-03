<template>
  <div class="afiliaciones-container">
    <div class="page-header">
      <h1><i class="fas fa-store-alt"></i> Barberías Afiliadas</h1>
      <button class="btn btn-primary" @click="mostrarModalSolicitud">
        <i class="fas fa-plus"></i> Solicitar Afiliación
      </button>
    </div>

    <!-- Estadísticas rápidas -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon icon-primary">
          <i class="fas fa-check-circle"></i>
        </div>
        <div class="stat-info">
          <h3>{{ afiliacionesAceptadas }}</h3>
          <p>Aceptadas</p>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon icon-warning">
          <i class="fas fa-clock"></i>
        </div>
        <div class="stat-info">
          <h3>{{ afiliacionesPendientes }}</h3>
          <p>Pendientes</p>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon icon-danger">
          <i class="fas fa-times-circle"></i>
        </div>
        <div class="stat-info">
          <h3>{{ afiliacionesRechazadas }}</h3>
          <p>Rechazadas</p>
        </div>
      </div>
    </div>

    <!-- Lista de barberías -->
    <div class="table-card">
      <div v-if="loading" class="loading-state">
        <i class="fas fa-spinner fa-spin"></i>
        <p>Cargando barberías...</p>
      </div>

      <div v-else-if="barberias.length === 0" class="empty-state">
        <i class="fas fa-store-slash"></i>
        <p>No hay barberías registradas.</p>
      </div>

      <div v-else class="barberias-grid">
        <div 
          v-for="barberia in barberias" 
          :key="barberia.id" 
          class="barberia-card"
          :class="{ 'destacada': barberia.esDestacada }"
        >
          <div class="barberia-header">
            <div class="barberia-logo">
              <i v-if="!barberia.logo" class="fas fa-cut"></i>
              <img v-else :src="barberia.logo" :alt="barberia.nombre" />
            </div>
            <div class="barberia-info">
              <h3>{{ barberia.nombre }}</h3>
              <p class="ubicacion"><i class="fas fa-map-marker-alt"></i> {{ barberia.direccion }}</p>
            </div>
            <span 
              v-if="barberia.afiliacion"
              :class="['badge', getEstadoClass(barberia.afiliacion.estado)]"
            >
              {{ barberia.afiliacion.estado }}
            </span>
          </div>

          <div class="barberia-body">
            <div class="info-row">
              <span class="label"><i class="fas fa-phone"></i> Teléfono:</span>
              <span class="value">{{ barberia.telefono || 'N/A' }}</span>
            </div>
            <div class="info-row">
              <span class="label"><i class="fas fa-envelope"></i> Email:</span>
              <span class="value">{{ barberia.email || 'N/A' }}</span>
            </div>
            <div class="info-row">
              <span class="label"><i class="fas fa-clock"></i> Horario:</span>
              <span class="value">{{ barberia.horarioAtencion || 'No especificado' }}</span>
            </div>
            <div v-if="barberia.descripcion" class="descripcion">
              {{ barberia.descripcion }}
            </div>
          </div>

          <div class="barberia-footer">
            <button class="btn btn-outline" @click="verDetalles(barberia)">
              <i class="fas fa-eye"></i> Ver Detalles
            </button>
            <button 
              v-if="!barberia.afiliacion"
              class="btn btn-primary" 
              @click="solicitarAfiliacion(barberia)"
            >
              <i class="fas fa-handshake"></i> Solicitar Afiliación
            </button>
            <button 
              v-else-if="barberia.afiliacion.estado === 'Pendiente'"
              class="btn btn-secondary" 
              disabled
            >
              <i class="fas fa-clock"></i> Pendiente
            </button>
            <button 
              v-else-if="barberia.afiliacion.estado === 'Aceptada'"
              class="btn btn-success" 
              @click="gestionarAfiliacion(barberia)"
            >
              <i class="fas fa-cog"></i> Gestionar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal Solicitud de Afiliación -->
    <div v-if="showModalSolicitud" class="modal-overlay" @click.self="cerrarModalSolicitud">
      <div class="modal-content">
        <div class="modal-header">
          <h3><i class="fas fa-handshake"></i> Solicitar Afiliación</h3>
          <button class="close-btn" @click="cerrarModalSolicitud">&times;</button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="enviarSolicitud">
            <div class="form-group">
              <label for="mensaje">Mensaje al propietario (opcional)</label>
              <textarea 
                id="mensaje" 
                v-model="solicitud.mensaje" 
                rows="4"
                placeholder="Explique por qué desea afiliarse a esta barbería..."
              ></textarea>
            </div>
          </form>
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary" @click="cerrarModalSolicitud">Cancelar</button>
          <button class="btn btn-primary" @click="enviarSolicitud" :disabled="enviando">
            <i v-if="enviando" class="fas fa-spinner fa-spin"></i>
            <i v-else class="fas fa-paper-plane"></i> Enviar Solicitud
          </button>
        </div>
      </div>
    </div>

    <!-- Modal Detalles -->
    <div v-if="showModalDetalles" class="modal-overlay" @click.self="cerrarModalDetalles">
      <div class="modal-content modal-large">
        <div class="modal-header">
          <h3>{{ barberiaSeleccionada?.nombre }}</h3>
          <button class="close-btn" @click="cerrarModalDetalles">&times;</button>
        </div>
        <div class="modal-body">
          <div v-if="barberiaSeleccionada" class="detalle-barberia">
            <div class="info-section">
              <h4><i class="fas fa-info-circle"></i> Información General</h4>
              <div class="info-grid">
                <div class="info-item">
                  <strong>Nombre:</strong> {{ barberiaSeleccionada.nombre }}
                </div>
                <div class="info-item">
                  <strong>Dirección:</strong> {{ barberiaSeleccionada.direccion }}
                </div>
                <div class="info-item">
                  <strong>Teléfono:</strong> {{ barberiaSeleccionada.telefono || 'N/A' }}
                </div>
                <div class="info-item">
                  <strong>Email:</strong> {{ barberiaSeleccionada.email || 'N/A' }}
                </div>
                <div class="info-item">
                  <strong>Horario:</strong> {{ barberiaSeleccionada.horarioAtencion || 'No especificado' }}
                </div>
              </div>
            </div>

            <div v-if="barberiaSeleccionada.descripcion" class="info-section">
              <h4><i class="fas fa-align-left"></i> Descripción</h4>
              <p>{{ barberiaSeleccionada.descripcion }}</p>
            </div>

            <div v-if="barberiaSeleccionada.servicios && barberiaSeleccionada.servicios.length > 0" class="info-section">
              <h4><i class="fas fa-cut"></i> Servicios Ofrecidos</h4>
              <ul class="servicios-list">
                <li v-for="servicio in barberiaSeleccionada.servicios" :key="servicio.id">
                  <span class="servicio-nombre">{{ servicio.nombre }}</span>
                  <span class="servicio-precio">${{ servicio.precio?.toFixed(2) }}</span>
                </li>
              </ul>
            </div>

            <div v-if="barberiaSeleccionada.afiliacion" class="info-section">
              <h4><i class="fas fa-link"></i> Estado de Afiliación</h4>
              <div class="afiliacion-info">
                <p><strong>Estado:</strong> 
                  <span :class="['badge', getEstadoClass(barberiaSeleccionada.afiliacion.estado)]">
                    {{ barberiaSeleccionada.afiliacion.estado }}
                  </span>
                </p>
                <p v-if="barberiaSeleccionada.afiliacion.fechaSolicitud">
                  <strong>Fecha Solicitud:</strong> {{ formatDate(barberiaSeleccionada.afiliacion.fechaSolicitud) }}
                </p>
                <p v-if="barberiaSeleccionada.afiliacion.mensajeRespuesta">
                  <strong>Respuesta:</strong> {{ barberiaSeleccionada.afiliacion.mensajeRespuesta }}
                </p>
              </div>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary" @click="cerrarModalDetalles">Cerrar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useAuthStore } from '@/stores/authStore';
// import barberiaService from '@/services/barberiaService'; // Importar cuando esté disponible

const authStore = useAuthStore();

// Estado
const loading = ref(false);
const enviando = ref(false);
const barberias = ref([]);
const showModalSolicitud = ref(false);
const showModalDetalles = ref(false);
const barberiaSeleccionada = ref(null);
const barberiaParaSolicitar = ref(null);

// Formulario de solicitud
const solicitud = ref({
  mensaje: ''
});

// Estadísticas computadas
const afiliacionesAceptadas = computed(() => {
  return barberias.value.filter(b => b.afiliacion?.estado === 'Aceptada').length;
});

const afiliacionesPendientes = computed(() => {
  return barberias.value.filter(b => b.afiliacion?.estado === 'Pendiente').length;
});

const afiliacionesRechazadas = computed(() => {
  return barberias.value.filter(b => b.afiliacion?.estado === 'Rechazada').length;
});

// Métodos
const cargarBarberias = async () => {
  loading.value = true;
  try {
    // Simulación de datos - Reemplazar con llamada real al servicio
    // const response = await barberiaService.obtenerTodas();
    // barberias.value = response.data || response;
    
    // Datos mock para desarrollo
    barberias.value = [
      {
        id: 1,
        nombre: 'Barbería El Clásico',
        direccion: 'Av. Principal 123, Centro',
        telefono: '+53 5555-1234',
        email: 'contacto@elclasico.com',
        horarioAtencion: 'Lun-Sab 9:00 AM - 7:00 PM',
        descripcion: 'Barbería tradicional con más de 20 años de experiencia.',
        esDestacada: true,
        afiliacion: { estado: 'Aceptada', fechaSolicitud: '2025-01-15' }
      },
      {
        id: 2,
        nombre: 'Style Cut',
        direccion: 'Calle 5ta #456, Vedado',
        telefono: '+53 5555-5678',
        email: 'info@stylecut.com',
        horarioAtencion: 'Mar-Dom 10:00 AM - 8:00 PM',
        descripcion: 'Estilo moderno y técnicas contemporáneas.',
        esDestacada: false,
        afiliacion: { estado: 'Pendiente', fechaSolicitud: '2025-02-01' }
      },
      {
        id: 3,
        nombre: 'The Gentleman',
        direccion: 'Calle 10 #789, Miramar',
        telefono: '+53 5555-9012',
        email: null,
        horarioAtencion: null,
        descripcion: null,
        esDestacada: false,
        afiliacion: null
      }
    ];
  } catch (error) {
    console.error('Error al cargar barberías:', error);
    alert('Error al cargar las barberías. Intente nuevamente.');
  } finally {
    loading.value = false;
  }
};

const mostrarModalSolicitud = () => {
  alert('Para solicitar afiliación, primero debe buscar una barbería específica y hacer clic en "Solicitar Afiliación".');
};

const solicitarAfiliacion = (barberia) => {
  barberiaParaSolicitar.value = barberia;
  solicitud.value.mensaje = '';
  showModalSolicitud.value = true;
};

const enviarSolicitud = async () => {
  if (!barberiaParaSolicitar.value) return;

  enviando.value = true;
  try {
    // Simulación de envío - Reemplazar con llamada real al servicio
    // await barberiaService.solicitarAfiliacion(barberiaParaSolicitar.value.id, {
    //   barberoId: authStore.user.id,
    //   mensaje: solicitud.value.mensaje
    // });
    
    await new Promise(resolve => setTimeout(resolve, 1000)); // Simular delay
    
    // Actualizar estado localmente
    const index = barberias.value.findIndex(b => b.id === barberiaParaSolicitar.value.id);
    if (index !== -1) {
      barberias.value[index].afiliacion = {
        estado: 'Pendiente',
        fechaSolicitud: new Date().toISOString().split('T')[0],
        mensaje: solicitud.value.mensaje
      };
    }
    
    alert('Solicitud de afiliación enviada correctamente. Espere la respuesta del propietario.');
    cerrarModalSolicitud();
    cargarBarberias();
  } catch (error) {
    console.error('Error al enviar solicitud:', error);
    alert('Error al enviar la solicitud. Intente nuevamente.');
  } finally {
    enviando.value = false;
  }
};

const cerrarModalSolicitud = () => {
  showModalSolicitud.value = false;
  barberiaParaSolicitar.value = null;
  solicitud.value.mensaje = '';
};

const verDetalles = (barberia) => {
  barberiaSeleccionada.value = barberia;
  showModalDetalles.value = true;
};

const cerrarModalDetalles = () => {
  showModalDetalles.value = false;
  barberiaSeleccionada.value = null;
};

const gestionarAfiliacion = (barberia) => {
  alert(`Gestionando afiliación con ${barberia.nombre}. Funcionalidad en desarrollo.`);
};

const getEstadoClass = (estado) => {
  switch (estado) {
    case 'Aceptada': return 'badge-success';
    case 'Pendiente': return 'badge-warning';
    case 'Rechazada': return 'badge-danger';
    default: return 'badge-secondary';
  }
};

const formatDate = (dateString) => {
  if (!dateString) return '';
  const options = { year: 'numeric', month: 'long', day: 'numeric' };
  return new Date(dateString).toLocaleDateString('es-ES', options);
};

onMounted(() => {
  cargarBarberias();
});
</script>

<style scoped>
.afiliaciones-container {
  padding: 2rem;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

/* Stats Grid */
.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  display: flex;
  align-items: center;
  gap: 1rem;
}

.stat-icon {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
}

.icon-primary { background: #e3f2fd; color: var(--primary-color); }
.icon-warning { background: #fff3e0; color: var(--warning-color); }
.icon-danger { background: #ffebee; color: var(--danger-color); }

.stat-info h3 {
  margin: 0;
  font-size: 1.8rem;
  color: #333;
}

.stat-info p {
  margin: 0;
  color: #666;
  font-size: 0.9rem;
}

/* Barberías Grid */
.barberias-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 1.5rem;
}

.barberia-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
}

.barberia-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
}

.barberia-card.destacada {
  border: 2px solid var(--primary-color);
}

.barberia-header {
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  border-bottom: 1px solid #eee;
}

.barberia-logo {
  width: 60px;
  height: 60px;
  border-radius: 8px;
  background: #f8f9fa;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: var(--primary-color);
}

.barberia-logo img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 8px;
}

.barberia-info {
  flex: 1;
}

.barberia-info h3 {
  margin: 0 0 0.25rem 0;
  font-size: 1.2rem;
  color: #333;
}

.ubicacion {
  margin: 0;
  font-size: 0.85rem;
  color: #666;
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

.barberia-body {
  padding: 1.5rem;
}

.info-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.75rem;
  font-size: 0.9rem;
}

.info-row .label {
  color: #666;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.descripcion {
  margin-top: 1rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 4px;
  font-size: 0.9rem;
  color: #555;
}

.barberia-footer {
  padding: 1.5rem;
  border-top: 1px solid #eee;
  display: flex;
  gap: 0.5rem;
  justify-content: space-between;
}

/* Loading & Empty States */
.loading-state, .empty-state {
  padding: 3rem;
  text-align: center;
  color: #666;
}

.loading-state i {
  font-size: 2rem;
  color: var(--primary-color);
  margin-bottom: 1rem;
}

.empty-state i {
  font-size: 3rem;
  color: #ccc;
  margin-bottom: 1rem;
  display: block;
}

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  border-radius: 8px;
  width: 90%;
  max-width: 500px;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}

.modal-large {
  max-width: 700px;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid #eee;
}

.modal-header h3 {
  margin: 0;
  color: #333;
}

.close-btn {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: #999;
}

.modal-body {
  padding: 1.5rem;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #555;
}

.form-group textarea {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  resize: vertical;
  font-family: inherit;
}

.modal-footer {
  padding: 1.5rem;
  border-top: 1px solid #eee;
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
}

/* Detalle Barberia */
.detalle-barberia {
  max-width: 100%;
}

.info-section {
  margin-bottom: 2rem;
}

.info-section h4 {
  margin: 0 0 1rem 0;
  color: var(--primary-color);
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.info-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
}

.info-item {
  padding: 0.75rem;
  background: #f8f9fa;
  border-radius: 4px;
}

.servicios-list {
  list-style: none;
  padding: 0;
}

.servicios-list li {
  display: flex;
  justify-content: space-between;
  padding: 0.75rem;
  border-bottom: 1px solid #eee;
}

.servicios-list li:last-child {
  border-bottom: none;
}

.servicio-nombre {
  font-weight: 500;
}

.servicio-precio {
  color: var(--success-color);
  font-weight: 600;
}

.afiliacion-info {
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 4px;
}

.afiliacion-info p {
  margin: 0.5rem 0;
}

/* Badges */
.badge {
  padding: 0.25rem 0.6rem;
  border-radius: 12px;
  font-size: 0.85rem;
  font-weight: 500;
}
.badge-success { background: #d4edda; color: #155724; }
.badge-warning { background: #fff3cd; color: #856404; }
.badge-danger { background: #f8d7da; color: #721c24; }
.badge-secondary { background: #e2e3e5; color: #383d41; }

/* Buttons */
.btn {
  padding: 0.6rem 1.2rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 500;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  transition: opacity 0.2s;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary { background: var(--primary-color); color: white; }
.btn-secondary { background: #6c757d; color: white; }
.btn-success { background: var(--success-color); color: white; }
.btn-outline { background: transparent; border: 1px solid var(--primary-color); color: var(--primary-color); }

@media (max-width: 768px) {
  .stats-grid {
    grid-template-columns: 1fr;
  }
  
  .barberias-grid {
    grid-template-columns: 1fr;
  }
  
  .page-header {
    flex-direction: column;
    gap: 1rem;
    align-items: stretch;
  }
}
</style>
