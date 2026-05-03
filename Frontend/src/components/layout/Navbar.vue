<template>
  <div class="navbar">
    <div class="navbar-brand">
      <router-link to="/" class="brand-link">
        <span class="brand-icon">✂️</span>
        <span class="brand-text">MultiBarbero</span>
      </router-link>
    </div>
    
    <div class="navbar-menu" v-if="isAuthenticated">
      <div class="navbar-items">
        <router-link to="/notificaciones" class="nav-item notification-link">
          <span class="icon">🔔</span>
          <span v-if="noLeidasCount > 0" class="notification-badge">{{ noLeidasCount }}</span>
        </router-link>
        
        <div class="user-menu">
          <button @click="toggleUserMenu" class="user-button">
            <span class="user-avatar">{{ userInitial }}</span>
            <span class="user-name">{{ userName }}</span>
            <span class="icon chevron">▼</span>
          </button>
          
          <div v-if="showUserMenu" class="user-dropdown">
            <router-link :to="dashboardRoute" class="dropdown-item">
              <span class="icon">📊</span> Dashboard
            </router-link>
            <router-link :to="perfilRoute" class="dropdown-item" v-if="perfilRoute">
              <span class="icon">👤</span> Mi Perfil
            </router-link>
            <div class="dropdown-divider"></div>
            <button @click="handleLogout" class="dropdown-item logout">
              <span class="icon">🚪</span> Cerrar Sesión
            </button>
          </div>
        </div>
      </div>
    </div>
    
    <div class="navbar-menu" v-else>
      <div class="navbar-items">
        <router-link to="/login" class="nav-item btn-login">Iniciar Sesión</router-link>
        <router-link to="/registro" class="nav-item btn-registro">Registrarse</router-link>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useAuthStore } from '../../stores/authStore'
import { useNotificacionStore } from '../../stores/notificacionStore'

const authStore = useAuthStore()
const notificacionStore = useNotificacionStore()

const showUserMenu = ref(false)

const isAuthenticated = computed(() => authStore.isAuthenticated)
const userName = computed(() => authStore.userName)
const userRole = computed(() => authStore.userRole)
const noLeidasCount = computed(() => notificacionStore.noLeidasCount)

const userInitial = computed(() => {
  return userName.value ? userName.value.charAt(0).toUpperCase() : 'U'
})

const dashboardRoute = computed(() => {
  const routes = {
    'Admin': '/admin',
    'Barbero': '/barbero',
    'Barberia': '/barberia',
    'Comercial': '/comercial',
    'Cliente': '/cliente'
  }
  return routes[userRole.value] || '/'
})

const perfilRoute = computed(() => {
  const routes = {
    'Barbero': '/barbero/perfil',
    'Barberia': '/barberia/perfil',
    'Cliente': '/cliente/perfil'
  }
  return routes[userRole.value] || null
})

function toggleUserMenu() {
  showUserMenu.value = !showUserMenu.value
}

function closeUserMenu() {
  showUserMenu.value = false
}

async function handleLogout() {
  await authStore.logout()
  window.location.href = '/login'
}

// Cerrar menú al hacer clic fuera
function handleClickOutside(event) {
  if (!event.target.closest('.user-menu')) {
    closeUserMenu()
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
  if (authStore.isAuthenticated) {
    notificacionStore.cargarNotificaciones()
  }
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>

<style scoped>
.navbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 1.5rem;
  background-color: #1a1a2e;
  color: white;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
  position: sticky;
  top: 0;
  z-index: 1000;
}

.navbar-brand {
  display: flex;
  align-items: center;
}

.brand-link {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  text-decoration: none;
  color: white;
  font-weight: bold;
  font-size: 1.25rem;
}

.brand-icon {
  font-size: 1.5rem;
}

.navbar-menu {
  display: flex;
  align-items: center;
}

.navbar-items {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.nav-item {
  padding: 0.5rem 1rem;
  border-radius: 0.375rem;
  text-decoration: none;
  color: white;
  transition: background-color 0.2s;
}

.nav-item:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

.btn-login {
  border: 1px solid rgba(255, 255, 255, 0.3);
}

.btn-registro {
  background-color: #e94560;
  border: 1px solid #e94560;
}

.btn-registro:hover {
  background-color: #d63850;
}

.notification-link {
  position: relative;
  font-size: 1.25rem;
}

.notification-badge {
  position: absolute;
  top: -0.25rem;
  right: -0.5rem;
  background-color: #e94560;
  color: white;
  font-size: 0.75rem;
  font-weight: bold;
  padding: 0.125rem 0.375rem;
  border-radius: 9999px;
  min-width: 1.25rem;
  text-align: center;
}

.user-menu {
  position: relative;
}

.user-button {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  background: none;
  border: none;
  color: white;
  cursor: pointer;
  padding: 0.5rem;
  border-radius: 0.375rem;
  transition: background-color 0.2s;
}

.user-button:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

.user-avatar {
  width: 2rem;
  height: 2rem;
  border-radius: 50%;
  background-color: #e94560;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
}

.user-name {
  font-weight: 500;
}

.chevron {
  font-size: 0.75rem;
}

.user-dropdown {
  position: absolute;
  top: 100%;
  right: 0;
  margin-top: 0.5rem;
  background-color: white;
  border-radius: 0.5rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  min-width: 12rem;
  overflow: hidden;
}

.dropdown-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1rem;
  color: #1a1a2e;
  text-decoration: none;
  transition: background-color 0.2s;
  width: 100%;
  text-align: left;
  background: none;
  border: none;
  cursor: pointer;
  font-size: 0.875rem;
}

.dropdown-item:hover {
  background-color: #f3f4f6;
}

.dropdown-item.logout {
  color: #e94560;
}

.dropdown-divider {
  height: 1px;
  background-color: #e5e7eb;
  margin: 0.25rem 0;
}

.icon {
  font-size: 1rem;
}
</style>
