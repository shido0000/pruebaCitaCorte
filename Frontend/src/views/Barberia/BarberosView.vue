<template>
  <div class="view-container">
    <div class="page-header">
      <h1>Barberos Afiliados</h1>
      <button class="btn-primary" @click="mostrarModal = true">
        <i class="fas fa-plus"></i> Invitar Barbero
      </button>
    </div>

    <!-- Estadísticas -->
    <div class="stats-row">
      <div class="stat-box">
        <h3>{{ barberosActivos }}</h3>
        <p>Barberos Activos</p>
      </div>
      <div class="stat-box">
        <h3>{{ solicitudesPendientes }}</h3>
        <p>Solicitudes Pendientes</p>
      </div>
      <div class="stat-box">
        <h3>{{ cupoDisponible }}</h3>
        <p>Cupos Disponibles</p>
      </div>
    </div>

    <!-- Filtros y Búsqueda -->
    <div class="filters-bar">
      <div class="search-box">
        <i class="fas fa-search"></i>
        <input v-model="filtroNombre" type="text" placeholder="Buscar por nombre...">
      </div>
      <select v-model="filtroEstado">
        <option value="">Todos los estados</option>
        <option value="activo">Activos</option>
        <option value="inactivo">Inactivos</option>
        <option value="pendiente">Pendientes</option>
      </select>
    </div>

    <!-- Lista de Barberos -->
    <div class="barberos-grid">
      <div v-for="barbero in barberosFiltrados" :key="barbero.id" class="barbero-card">
        <div class="card-header">
          <img :src="barbero.fotoPerfil || '/default-avatar.png'" alt="Foto" class="avatar">
          <div class="info">
            <h3>{{ barbero.nombre }}</h3>
            <p class="especialidad">{{ barbero.especialidad }}</p>
          </div>
          <span :class="['badge', getBadgeClass(barbero.estado)]">
            {{ barbero.estado }}
          </span>
        </div>

        <div class="card-body">
          <div class="info-row">
            <i class="fas fa-calendar"></i>
            <span>Afiliado desde: {{ formatearFecha(barbero.fechaAfiliacion) }}</span>
          </div>
          <div class="info-row">
            <i class="fas fa-star"></i>
            <span>Calificación: {{ barbero.calificacion }}/5</span>
          </div>
          <div class="info-row">
            <i class="fas fa-cut"></i>
            <span>Servicios: {{ barbero.totalServicios }}</span>
          </div>
          <div class="info-row">
            <i class="fas fa-users"></i>
            <span>Clientes: {{ barbero.totalClientes }}</span>
          </div>
        </div>

        <div class="card-actions">
          <button class="btn-outline" @click="verPerfil(barbero.id)">
            <i class="fas fa-eye"></i> Ver Perfil
          </button>
          <button v-if="barbero.estado === 'activo'" class="btn-danger" @click="eliminarAfiliacion(barbero.id)">
            <i class="fas fa-trash"></i> Eliminar
          </button>
          <button v-if="barbero.estado === 'pendiente'" class="btn-success" @click="aprobarSolicitud(barbero.id)">
            <i class="fas fa-check"></i> Aprobar
          </button>
        </div>
      </div>
    </div>

    <div v-if="barberosFiltrados.length === 0" class="empty-state">
      <i class="fas fa-user-slash"></i>
      <h3>No hay barberos afiliados</h3>
      <p>Invita barberos a tu barbería para comenzar</p>
    </div>

    <!-- Modal Invitar -->
    <div v-if="mostrarModal" class="modal-overlay" @click="mostrarModal = false">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h2>Invitar Barbero</h2>
          <button class="btn-close" @click="mostrarModal = false">&times;</button>
        </div>
        <div class="modal-body">
          <div class="form-group">
            <label>Buscar Barbero por Email</label>
            <input v-model="emailInvitar" type="email" placeholder="ejemplo@correo.com">
          </div>
          <p class="help-text">El barbero recibirá una invitación para unirse a tu barbería</p>
        </div>
        <div class="modal-footer">
          <button class="btn-secondary" @click="mostrarModal = false">Cancelar</button>
          <button class="btn-primary" @click="enviarInvitacion">Enviar Invitación</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed } from 'vue'
import axios from 'axios'

export default {
  name: 'BarberiaBarberos',
  setup() {
    const mostrarModal = ref(false)
    const filtroNombre = ref('')
    const filtroEstado = ref('')
    const emailInvitar = ref('')

    const barberos = ref([
      {
        id: 1,
        nombre: 'Juan Pérez',
        especialidad: 'Corte Clásico, Barba',
        fotoPerfil: '',
        estado: 'activo',
        fechaAfiliacion: '2024-01-15',
        calificacion: 4.8,
        totalServicios: 156,
        totalClientes: 89
      },
      {
        id: 2,
        nombre: 'Carlos Rodríguez',
        especialidad: 'Fade, Color',
        fotoPerfil: '',
        estado: 'activo',
        fechaAfiliacion: '2024-02-20',
        calificacion: 4.9,
        totalServicios: 203,
        totalClientes: 112
      },
      {
        id: 3,
        nombre: 'Miguel Ángel',
        especialidad: 'Barba, Afeitado',
        fotoPerfil: '',
        estado: 'pendiente',
        fechaAfiliacion: null,
        calificacion: 0,
        totalServicios: 0,
        totalClientes: 0
      }
    ])

    const barberosActivos = computed(() => barberos.value.filter(b => b.estado === 'activo').length)
    const solicitudesPendientes = computed(() => barberos.value.filter(b => b.estado === 'pendiente').length)
    const cupoDisponible = ref(7) // Ejemplo: plan permite 10, hay 3

    const barberosFiltrados = computed(() => {
      return barberos.value.filter(barbero => {
        const coincideNombre = !filtroNombre.value || 
          barbero.nombre.toLowerCase().includes(filtroNombre.value.toLowerCase())
        const coincideEstado = !filtroEstado.value || barbero.estado === filtroEstado.value
        return coincideNombre && coincideEstado
      })
    })

    const getBadgeClass = (estado) => {
      const classes = {
        'activo': 'badge-success',
        'inactivo': 'badge-secondary',
        'pendiente': 'badge-warning'
      }
      return classes[estado] || ''
    }

    const formatearFecha = (fecha) => {
      if (!fecha) return '-'
      return new Date(fecha).toLocaleDateString('es-ES', { 
        day: 'numeric', month: 'long', year: 'numeric' 
      })
    }

    const verPerfil = (id) => {
      window.location.href = `/barberia/barberos/${id}`
    }

    const eliminarAfiliacion = async (id) => {
      if (!confirm('¿Estás seguro de eliminar esta afiliación?')) return
      try {
        await axios.delete(`/api/afiliacion/${id}`)
        alert('Afiliación eliminada correctamente')
        barberos.value = barberos.value.filter(b => b.id !== id)
      } catch (error) {
        alert('Error al eliminar la afiliación')
      }
    }

    const aprobarSolicitud = async (id) => {
      try {
        await axios.post(`/api/solicitudafiliacion/${id}/aprobar`)
        alert('Solicitud aprobada')
        const barbero = barberos.value.find(b => b.id === id)
        if (barbero) barbero.estado = 'activo'
      } catch (error) {
        alert('Error al aprobar la solicitud')
      }
    }

    const enviarInvitacion = async () => {
      if (!emailInvitar.value) {
        alert('Por favor ingresa un email')
        return
      }
      try {
        await axios.post('/api/afiliacion/invitar', { email: emailInvitar.value })
        alert('Invitación enviada correctamente')
        mostrarModal.value = false
        emailInvitar.value = ''
      } catch (error) {
        alert('Error al enviar la invitación')
      }
    }

    return {
      mostrarModal,
      filtroNombre,
      filtroEstado,
      emailInvitar,
      barberos,
      barberosActivos,
      solicitudesPendientes,
      cupoDisponible,
      barberosFiltrados,
      getBadgeClass,
      formatearFecha,
      verPerfil,
      eliminarAfiliacion,
      aprobarSolicitud,
      enviarInvitacion
    }
  }
}
</script>

<style scoped>
.view-container {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.page-header h1 {
  font-size: 2rem;
  color: #1a1a1a;
  margin: 0;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  cursor: pointer;
  font-size: 1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-primary:hover {
  opacity: 0.9;
}

.stats-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  margin-bottom: 2rem;
}

.stat-box {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  text-align: center;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

.stat-box h3 {
  font-size: 2rem;
  color: #4F46E5;
  margin: 0 0 0.5rem 0;
}

.stat-box p {
  color: #666;
  margin: 0;
  font-size: 0.9rem;
}

.filters-bar {
  display: flex;
  gap: 1rem;
  margin-bottom: 2rem;
}

.search-box {
  flex: 1;
  position: relative;
}

.search-box i {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #999;
}

.search-box input {
  width: 100%;
  padding: 0.75rem 1rem 0.75rem 2.5rem;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-size: 1rem;
}

.filters-bar select {
  padding: 0.75rem 1rem;
  border: 1px solid #ddd;
  border-radius: 8px;
  font-size: 1rem;
  background: white;
  cursor: pointer;
}

.barberos-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 1.5rem;
}

.barbero-card {
  background: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  transition: transform 0.2s;
}

.barbero-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.12);
}

.card-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1.5rem;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
}

.avatar {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  object-fit: cover;
}

.info h3 {
  margin: 0;
  color: #1a1a1a;
  font-size: 1.1rem;
}

.especialidad {
  color: #666;
  font-size: 0.9rem;
  margin: 0.25rem 0 0 0;
}

.badge {
  margin-left: auto;
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 500;
}

.badge-success { background: #d1fae5; color: #065f46; }
.badge-warning { background: #fef3c7; color: #92400e; }
.badge-secondary { background: #e5e7eb; color: #374151; }

.card-body {
  padding: 1.5rem;
}

.info-row {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-bottom: 0.75rem;
  color: #666;
  font-size: 0.9rem;
}

.info-row:last-child {
  margin-bottom: 0;
}

.info-row i {
  color: #4F46E5;
  width: 20px;
}

.card-actions {
  display: flex;
  gap: 0.5rem;
  padding: 1.5rem;
  border-top: 1px solid #e5e7eb;
}

.btn-outline {
  flex: 1;
  background: transparent;
  border: 1px solid #4F46E5;
  color: #4F46E5;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.btn-outline:hover {
  background: #4F46E5;
  color: white;
}

.btn-danger {
  flex: 1;
  background: #ef4444;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.btn-danger:hover {
  background: #dc2626;
}

.btn-success {
  flex: 1;
  background: #10b981;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.btn-success:hover {
  background: #059669;
}

.empty-state {
  text-align: center;
  padding: 4rem 2rem;
  color: #666;
}

.empty-state i {
  font-size: 4rem;
  color: #ddd;
  margin-bottom: 1rem;
  display: block;
}

.empty-state h3 {
  margin: 0 0 0.5rem 0;
  color: #1a1a1a;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  border-radius: 12px;
  width: 90%;
  max-width: 500px;
  max-height: 90vh;
  overflow-y: auto;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid #e5e7eb;
}

.modal-header h2 {
  margin: 0;
  font-size: 1.5rem;
  color: #1a1a1a;
}

.btn-close {
  background: none;
  border: none;
  font-size: 2rem;
  cursor: pointer;
  color: #999;
  line-height: 1;
}

.btn-close:hover {
  color: #1a1a1a;
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
  color: #374151;
  font-weight: 500;
}

.form-group input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
}

.help-text {
  color: #666;
  font-size: 0.9rem;
  margin: 0;
}

.modal-footer {
  display: flex;
  gap: 1rem;
  padding: 1.5rem;
  border-top: 1px solid #e5e7eb;
}

.btn-secondary {
  flex: 1;
  background: #f3f4f6;
  color: #374151;
  border: none;
  padding: 0.75rem 1rem;
  border-radius: 6px;
  cursor: pointer;
  font-size: 1rem;
}

.btn-secondary:hover {
  background: #e5e7eb;
}

@media (max-width: 768px) {
  .page-header {
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }

  .filters-bar {
    flex-direction: column;
  }

  .barberos-grid {
    grid-template-columns: 1fr;
  }

  .card-actions {
    flex-direction: column;
  }
}
</style>
