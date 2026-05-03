<template>
  <div class="not-found-container">
    <div class="not-found-content">
      <h1 class="error-code">404</h1>
      <h2>Página no encontrada</h2>
      <p>Lo sentimos, la página que buscas no existe o ha sido movida.</p>
      <router-link to="/" class="btn-home">Volver al Inicio</router-link>
    </div>
  </div>
</template>

<style scoped>
.not-found-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
}

.not-found-content {
  text-align: center;
  color: white;
}

.error-code {
  font-size: 8rem;
  margin: 0;
  color: #e94560;
  line-height: 1;
}

.not-found-content h2 {
  font-size: 2rem;
  margin: 1rem 0;
}

.not-found-content p {
  color: rgba(255, 255, 255, 0.8);
  margin-bottom: 2rem;
}

.btn-home {
  display: inline-block;
  background-color: #e94560;
  color: white;
  padding: 0.875rem 2rem;
  border-radius: 0.5rem;
  text-decoration: none;
  font-weight: 600;
  transition: background-color 0.2s;
}

.btn-home:hover {
  background-color: #d63850;
}
</style>
