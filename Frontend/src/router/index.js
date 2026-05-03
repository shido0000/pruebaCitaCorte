import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/authStore'

const routes = [
  // Rutas públicas
  {
    path: '/login',
    name: 'Login',
    component: () => import('../views/Auth/LoginView.vue'),
    meta: { requiresGuest: true }
  },
  {
    path: '/registro',
    name: 'Registro',
    component: () => import('../views/Auth/RegistroView.vue'),
    meta: { requiresGuest: true }
  },
  {
    path: '/',
    name: 'Home',
    component: () => import('../views/HomeView.vue'),
    meta: { requiresAuth: false }
  },
  
  // Rutas Admin
  {
    path: '/admin',
    name: 'AdminDashboard',
    component: () => import('../views/Admin/DashboardView.vue'),
    meta: { requiresAuth: true, role: 'Admin' }
  },
  {
    path: '/admin/planes',
    name: 'AdminPlanes',
    component: () => import('../views/Admin/PlanesView.vue'),
    meta: { requiresAuth: true, role: 'Admin' }
  },
  {
    path: '/admin/barberos',
    name: 'AdminBarberos',
    component: () => import('../views/Admin/BarberosView.vue'),
    meta: { requiresAuth: true, role: 'Admin' }
  },
  {
    path: '/admin/barberias',
    name: 'AdminBarberias',
    component: () => import('../views/Admin/BarberiasView.vue'),
    meta: { requiresAuth: true, role: 'Admin' }
  },
  {
    path: '/admin/comerciales',
    name: 'AdminComerciales',
    component: () => import('../views/Admin/ComercialesView.vue'),
    meta: { requiresAuth: true, role: 'Admin' }
  },
  {
    path: '/admin/solicitudes',
    name: 'AdminSolicitudes',
    component: () => import('../views/Admin/SolicitudesView.vue'),
    meta: { requiresAuth: true, role: 'Admin' }
  },
  
  // Rutas Barbero
  {
    path: '/barbero',
    name: 'BarberoDashboard',
    component: () => import('../views/Barbero/DashboardView.vue'),
    meta: { requiresAuth: true, role: 'Barbero' }
  },
  {
    path: '/barbero/perfil',
    name: 'BarberoPerfil',
    component: () => import('../views/Barbero/PerfilView.vue'),
    meta: { requiresAuth: true, role: 'Barbero' }
  },
  {
    path: '/barbero/servicios',
    name: 'BarberoServicios',
    component: () => import('../views/Barbero/ServiciosView.vue'),
    meta: { requiresAuth: true, role: 'Barbero' }
  },
  {
    path: '/barbero/reservas',
    name: 'BarberoReservas',
    component: () => import('../views/Barbero/ReservasView.vue'),
    meta: { requiresAuth: true, role: 'Barbero' }
  },
  {
    path: '/barbero/estadisticas',
    name: 'BarberoEstadisticas',
    component: () => import('../views/Barbero/EstadisticasView.vue'),
    meta: { requiresAuth: true, role: 'Barbero' }
  },
  {
    path: '/barbero/afiliaciones',
    name: 'BarberoAfiliaciones',
    component: () => import('../views/Barbero/AfiliacionesView.vue'),
    meta: { requiresAuth: true, role: 'Barbero' }
  },
  
  // Rutas Barbería
  {
    path: '/barberia',
    name: 'BarberiaDashboard',
    component: () => import('../views/Barberia/DashboardView.vue'),
    meta: { requiresAuth: true, role: 'Barberia' }
  },
  {
    path: '/barberia/perfil',
    name: 'BarberiaPerfil',
    component: () => import('../views/Barberia/PerfilView.vue'),
    meta: { requiresAuth: true, role: 'Barberia' }
  },
  {
    path: '/barberia/servicios',
    name: 'BarberiaServicios',
    component: () => import('../views/Barberia/ServiciosView.vue'),
    meta: { requiresAuth: true, role: 'Barberia' }
  },
  {
    path: '/barberia/reservas',
    name: 'BarberiaReservas',
    component: () => import('../views/Barberia/ReservasView.vue'),
    meta: { requiresAuth: true, role: 'Barberia' }
  },
  {
    path: '/barberia/barberos',
    name: 'BarberiaBarberos',
    component: () => import('../views/Barberia/BarberosView.vue'),
    meta: { requiresAuth: true, role: 'Barberia' }
  },
  {
    path: '/barberia/solicitudes',
    name: 'BarberiaSolicitudes',
    component: () => import('../views/Barberia/SolicitudesView.vue'),
    meta: { requiresAuth: true, role: 'Barberia' }
  },
  {
    path: '/barberia/estadisticas',
    name: 'BarberiaEstadisticas',
    component: () => import('../views/Barberia/EstadisticasView.vue'),
    meta: { requiresAuth: true, role: 'Barberia' }
  },
  
  // Rutas Comercial
  {
    path: '/comercial',
    name: 'ComercialDashboard',
    component: () => import('../views/Comercial/DashboardView.vue'),
    meta: { requiresAuth: true, role: 'Comercial' }
  },
  {
    path: '/comercial/barberos',
    name: 'ComercialBarberos',
    component: () => import('../views/Comercial/BarberosView.vue'),
    meta: { requiresAuth: true, role: 'Comercial' }
  },
  {
    path: '/comercial/barberias',
    name: 'ComercialBarberias',
    component: () => import('../views/Comercial/BarberiasView.vue'),
    meta: { requiresAuth: true, role: 'Comercial' }
  },
  
  // Rutas Cliente
  {
    path: '/cliente',
    name: 'ClienteDashboard',
    component: () => import('../views/Cliente/DashboardView.vue'),
    meta: { requiresAuth: true, role: 'Cliente' }
  },
  {
    path: '/cliente/perfil',
    name: 'ClientePerfil',
    component: () => import('../views/Cliente/PerfilView.vue'),
    meta: { requiresAuth: true, role: 'Cliente' }
  },
  {
    path: '/cliente/reservas',
    name: 'ClienteReservas',
    component: () => import('../views/Cliente/ReservasView.vue'),
    meta: { requiresAuth: true, role: 'Cliente' }
  },
  {
    path: '/cliente/barberos',
    name: 'ClienteBarberos',
    component: () => import('../views/Cliente/BarberosView.vue'),
    meta: { requiresAuth: true, role: 'Cliente' }
  },
  {
    path: '/cliente/barberias',
    name: 'ClienteBarberias',
    component: () => import('../views/Cliente/BarberiasView.vue'),
    meta: { requiresAuth: true, role: 'Cliente' }
  },
  {
    path: '/cliente/historial',
    name: 'ClienteHistorial',
    component: () => import('../views/Cliente/HistorialView.vue'),
    meta: { requiresAuth: true, role: 'Cliente' }
  },
  
  // Ruta de notificaciones (común para todos los roles)
  {
    path: '/notificaciones',
    name: 'Notificaciones',
    component: () => import('../views/NotificacionesView.vue'),
    meta: { requiresAuth: true }
  },
  
  // Ruta 404
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: () => import('../views/NotFoundView.vue')
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Guard de navegación
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  const isAuthenticated = authStore.isAuthenticated
  const userRole = authStore.userRole
  
  // Rutas que requieren autenticación
  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/login')
    return
  }
  
  // Rutas que solo son para usuarios no autenticados
  if (to.meta.requiresGuest && isAuthenticated) {
    next('/')
    return
  }
  
  // Verificar rol requerido
  if (to.meta.role && to.meta.role !== userRole) {
    // Redirigir al dashboard correspondiente al rol del usuario
    switch (userRole) {
      case 'Admin':
        next('/admin')
        break
      case 'Barbero':
        next('/barbero')
        break
      case 'Barberia':
        next('/barberia')
        break
      case 'Comercial':
        next('/comercial')
        break
      case 'Cliente':
        next('/cliente')
        break
      default:
        next('/')
    }
    return
  }
  
  next()
})

export default router
