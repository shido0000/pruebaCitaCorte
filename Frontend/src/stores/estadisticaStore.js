import { defineStore } from 'pinia';
import estadisticaService from '@/services/estadisticaService';

export const useEstadisticaStore = defineStore('estadistica', {
  state: () => ({
    dashboard: null,
    estadisticasReservas: [],
    estadisticasIngresos: [],
    estadisticasClientes: [],
    filtrosActuales: {},
    cargando: false,
    error: null
  }),

  getters: {
    totalReservas: (state) => state.dashboard?.totalReservas || 0,
    totalIngresos: (state) => state.dashboard?.totalIngresos || 0,
    totalClientes: (state) => state.dashboard?.totalClientes || 0,
    tasaOcupacion: (state) => state.dashboard?.tasaOcupacion || 0,
    reservasPendientes: (state) => state.dashboard?.reservasPendientes || 0,
    reservasConfirmadas: (state) => state.dashboard?.reservasConfirmadas || 0
  },

  actions: {
    async cargarDashboard(tipoPerfil, perfilId, periodo = 'mes') {
      this.cargando = true;
      this.error = null;
      try {
        const response = await estadisticaService.obtenerDashboard(tipoPerfil, perfilId, periodo);
        this.dashboard = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async cargarEstadisticasReservas(filtros) {
      this.cargando = true;
      this.error = null;
      this.filtrosActuales = filtros;
      try {
        const response = await estadisticaService.obtenerEstadisticasReservas(filtros);
        this.estadisticasReservas = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async cargarEstadisticasIngresos(filtros) {
      this.cargando = true;
      this.error = null;
      this.filtrosActuales = filtros;
      try {
        const response = await estadisticaService.obtenerEstadisticasIngresos(filtros);
        this.estadisticasIngresos = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async cargarEstadisticasClientes(filtros) {
      this.cargando = true;
      this.error = null;
      this.filtrosActuales = filtros;
      try {
        const response = await estadisticaService.obtenerEstadisticasClientes(filtros);
        this.estadisticasClientes = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async exportarReporte(tipo, formato = 'pdf', filtros = {}) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await estadisticaService.exportarReporte(tipo, formato, filtros);
        return response;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    limpiarEstadisticas() {
      this.dashboard = null;
      this.estadisticasReservas = [];
      this.estadisticasIngresos = [];
      this.estadisticasClientes = [];
      this.filtrosActuales = {};
      this.error = null;
    }
  }
});
