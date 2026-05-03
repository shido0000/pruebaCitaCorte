<template>
  <div class="home-container">
    <div class="hero-section">
      <h1>Bienvenido a MultiBarbero</h1>
      <p>La plataforma integral para la gestión de barberías y barberos</p>
      <div class="hero-actions" v-if="!isAuthenticated">
        <router-link to="/registro" class="btn-primary">Comenzar Ahora</router-link>
        <router-link to="/login" class="btn-secondary">Iniciar Sesión</router-link>
      </div>
    </div>

    <div class="features-section">
      <h2>Características Principales</h2>
      <div class="features-grid">
        <div class="feature-card">
          <span class="feature-icon">✂️</span>
          <h3>Para Barberos</h3>
          <p>Gestiona tu perfil, servicios, reservas y estadísticas</p>
        </div>
        <div class="feature-card">
          <span class="feature-icon">🏪</span>
          <h3>Para Barberías</h3>
          <p>Administra tu negocio, barberos afiliados y más</p>
        </div>
        <div class="feature-card">
          <span class="feature-icon">👤</span>
          <h3>Para Clientes</h3>
          <p>Reserva citas con tus barberos favoritos fácilmente</p>
        </div>
        <div class="feature-card">
          <span class="feature-icon">💼</span>
          <h3>Para Comerciales</h3>
          <p>Gestiona clientes y promociona el servicio</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useAuthStore } from '../stores/authStore'

const authStore = useAuthStore()
const isAuthenticated = computed(() => authStore.isAuthenticated)
</script>

<style scoped>
.home-container {
  min-height: 100vh;
}

.hero-section {
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
  color: white;
  padding: 4rem 2rem;
  text-align: center;
}

.hero-section h1 {
  font-size: 2.5rem;
  margin: 0 0 1rem 0;
}

.hero-section p {
  font-size: 1.25rem;
  opacity: 0.9;
  margin: 0 0 2rem 0;
}

.hero-actions {
  display: flex;
  gap: 1rem;
  justify-content: center;
}

.btn-primary {
  background-color: #e94560;
  color: white;
  padding: 0.875rem 2rem;
  border-radius: 0.5rem;
  text-decoration: none;
  font-weight: 600;
  transition: background-color 0.2s;
}

.btn-primary:hover {
  background-color: #d63850;
}

.btn-secondary {
  background-color: transparent;
  color: white;
  padding: 0.875rem 2rem;
  border-radius: 0.5rem;
  text-decoration: none;
  font-weight: 600;
  border: 2px solid white;
  transition: background-color 0.2s;
}

.btn-secondary:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

.features-section {
  padding: 4rem 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.features-section h2 {
  text-align: center;
  color: #1a1a2e;
  margin: 0 0 2rem 0;
  font-size: 2rem;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
}

.feature-card {
  background: white;
  padding: 2rem;
  border-radius: 0.75rem;
  text-align: center;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.feature-icon {
  font-size: 3rem;
  display: block;
  margin-bottom: 1rem;
}

.feature-card h3 {
  color: #1a1a2e;
  margin: 0 0 0.75rem 0;
}

.feature-card p {
  color: #6b7280;
  margin: 0;
}
</style>
