import { defineStore } from 'pinia';
import suscripcionService from '@/services/suscripcionService';

export const useSuscripcionStore = defineStore('suscripcion', {
  state: () => ({
    suscripcionActual: null,
    historialSuscripciones: [],
    solicitudesCambio: [],
    cargando: false,
    error: null
  }),

  getters: {
    tieneSuscripcionActiva: (state) => {
      return state.suscripcionActual?.estado === 'Activa';
    },
    diasRestantes: (state) => {
      if (!state.suscripcionActual?.fechaVencimiento) return 0;
      const ahora = new Date();
      const vencimiento = new Date(state.suscripcionActual.fechaVencimiento);
      const diffTime = vencimiento - ahora;
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
      return diffDays > 0 ? diffDays : 0;
    },
    puedeSolicitarCambio: (state) => {
      return state.suscripcionActual?.estado === 'Activa' && 
             !state.solicitudesCambio.some(s => s.estado === 'Pendiente');
    }
  },

  actions: {
    async cargarSuscripcionActual(perfilId, tipoPerfil) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await suscripcionService.obtenerSuscripcion(perfilId, tipoPerfil);
        this.suscripcionActual = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async cargarHistorial(perfilId, tipoPerfil) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await suscripcionService.obtenerHistorial(perfilId, tipoPerfil);
        this.historialSuscripciones = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async cargarSolicitudesCambio(perfilId, tipoPerfil) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await suscripcionService.obtenerSolicitudesCambio(perfilId, tipoPerfil);
        this.solicitudesCambio = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async solicitarCambioPlan(planId, motivo) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await suscripcionService.solicitarCambio(
          this.suscripcionActual.id, 
          planId, 
          motivo
        );
        this.solicitudesCambio.push(response.data);
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async renovarSuscripcion() {
      this.cargando = true;
      this.error = null;
      try {
        const response = await suscripcionService.renovar(this.suscripcionActual.id);
        this.suscripcionActual = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async cancelarSuscripcion(motivo) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await suscripcionService.cancelar(this.suscripcionActual.id, motivo);
        this.suscripcionActual = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async responderSolicitudCambio(solicitudId, aprobada, observaciones) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await suscripcionService.responderSolicitud(
          solicitudId, 
          aprobada, 
          observaciones
        );
        const index = this.solicitudesCambio.findIndex(s => s.id === solicitudId);
        if (index !== -1) {
          this.solicitudesCambio[index] = response.data;
        }
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    limpiarSuscripcionActual() {
      this.suscripcionActual = null;
      this.error = null;
    }
  }
});
