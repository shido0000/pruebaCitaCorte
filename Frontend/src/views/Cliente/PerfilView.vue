<template>
  <div class="perfil-view">
    <h1><i class="fas fa-user-circle"></i> Mi Perfil</h1>
    <div class="perfil-container">
      <div class="avatar-section">
        <div class="avatar">{{ usuario.nombre.charAt(0) }}</div>
        <button class="btn-upload">Cambiar foto</button>
      </div>
      <form class="perfil-form" @submit.prevent="guardar">
        <div class="form-group">
          <label>Nombre Completo</label>
          <input v-model="usuario.nombre" type="text" />
        </div>
        <div class="form-group">
          <label>Email</label>
          <input v-model="usuario.email" type="email" />
        </div>
        <div class="form-group">
          <label>Teléfono</label>
          <input v-model="usuario.telefono" type="tel" />
        </div>
        <div class="form-actions">
          <button type="submit" class="btn-primary">Guardar Cambios</button>
        </div>
      </form>
    </div>
  </div>
</template>
<script setup>
import { ref } from 'vue';
const usuario = ref({ nombre: "Juan Pérez", email: "juan@email.com", telefono: "+34 600 000 000" });
const guardar = () => alert('Perfil actualizado');
</script>
<style scoped>
.perfil-view { padding: 2rem; max-width: 600px; margin: 0 auto; }
.perfil-container { background: white; padding: 2rem; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.avatar-section { text-align: center; margin-bottom: 2rem; }
.avatar { width: 100px; height: 100px; background: #3b82f6; color: white; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-size: 2.5rem; margin: 0 auto 1rem; }
.btn-upload { background: #f3f4f6; border: none; padding: 0.5rem 1rem; border-radius: 8px; cursor: pointer; }
.form-group { margin-bottom: 1.5rem; }
.form-group label { display: block; margin-bottom: 0.5rem; font-weight: 600; }
.form-group input { width: 100%; padding: 0.75rem; border: 2px solid #e0e0e0; border-radius: 8px; font-size: 1rem; }
.form-actions { display: flex; justify-content: flex-end; }
.btn-primary { background: #3b82f6; color: white; border: none; padding: 0.75rem 2rem; border-radius: 8px; cursor: pointer; font-weight: 600; }
</style>
