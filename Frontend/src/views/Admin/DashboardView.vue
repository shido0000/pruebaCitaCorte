<template>
  <div class="dashboard-container">
    <h1>Dashboard Admin</h1>
    <p class="welcome-text">Bienvenido al panel de administración</p>
    
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon">👥</div>
        <div class="stat-info">
          <h3>Barberos</h3>
          <p class="stat-number">0</p>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon">🏪</div>
        <div class="stat-info">
          <h3>Barberías</h3>
          <p class="stat-number">0</p>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon">💼</div>
        <div class="stat-info">
          <h3>Comerciales</h3>
          <p class="stat-number">0</p>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon">📋</div>
        <div class="stat-info">
          <h3>Planes Activos</h3>
          <p class="stat-number">0</p>
        </div>
      </div>
    </div>

    <div class="actions-grid">
      <router-link to="/admin/planes" class="action-card">
        <span class="action-icon">📊</span>
        <h3>Gestionar Planes</h3>
        <p>Crea y administra los planes de suscripción</p>
      </router-link>
      
      <router-link to="/admin/barberos" class="action-card">
        <span class="action-icon">✂️</span>
        <h3>Barberos</h3>
        <p>Administra los barberos registrados</p>
      </router-link>
      
      <router-link to="/admin/barberias" class="action-card">
        <span class="action-icon">🏪</span>
        <h3>Barberías</h3>
        <p>Administra las barberías registradas</p>
      </router-link>
      
      <router-link to="/admin/comerciales" class="action-card">
        <span class="action-icon">💼</span>
        <h3>Comerciales</h3>
        <p>Gestiona el equipo comercial</p>
      </router-link>
      
      <router-link to="/admin/solicitudes" class="action-card">
        <span class="action-icon">📝</span>
        <h3>Solicitudes</h3>
        <p>Revisa solicitudes de afiliación y cambios</p>
      </router-link>
    </div>
  </div>
</template>

<script setup>
// Lógica del dashboard de Admin
</script>

<style scoped>
.dashboard-container {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.welcome-text {
  color: #6b7280;
  margin-bottom: 2rem;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: white;
  border-radius: 0.75rem;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.stat-icon {
  font-size: 2.5rem;
}

.stat-info h3 {
  margin: 0;
  font-size: 0.875rem;
  color: #6b7280;
}

.stat-number {
  margin: 0.25rem 0 0 0;
  font-size: 1.75rem;
  font-weight: bold;
  color: #1a1a2e;
}

.actions-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
}

.action-card {
  background: white;
  border-radius: 0.75rem;
  padding: 1.5rem;
  text-decoration: none;
  color: inherit;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s, box-shadow 0.2s;
}

.action-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.action-icon {
  font-size: 2rem;
  display: block;
  margin-bottom: 0.75rem;
}

.action-card h3 {
  margin: 0 0 0.5rem 0;
  color: #1a1a2e;
}

.action-card p {
  margin: 0;
  color: #6b7280;
  font-size: 0.875rem;
}
</style>
