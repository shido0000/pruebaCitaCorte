<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useAuthStore } from './stores/authStore'

const authStore = useAuthStore()

onMounted(() => {
  authStore.cargarUsuario()
})
</script>

<style scoped>
#app {
  min-height: 100vh;
}
</style>
