import { defineStore } from 'pinia';
import planService from '@/services/planService';

export const usePlanStore = defineStore('plan', {
  state: () => ({
    planes: [],
    planActual: null,
    cargando: false,
    error: null
  }),

  getters: {
    planesPorTipo: (state) => (tipo) => {
      return state.planes.filter(plan => plan.tipoProveedor === tipo);
    },
    planPorId: (state) => (id) => {
      return state.planes.find(plan => plan.id === id);
    }
  },

  actions: {
    async cargarPlanes(tipoProveedor = null) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await planService.obtenerPlanes(tipoProveedor);
        this.planes = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async cargarPlan(id) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await planService.obtenerPlan(id);
        this.planActual = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async crearPlan(planData) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await planService.crearPlan(planData);
        this.planes.push(response.data);
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async actualizarPlan(id, planData) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await planService.actualizarPlan(id, planData);
        const index = this.planes.findIndex(p => p.id === id);
        if (index !== -1) {
          this.planes[index] = response.data;
        }
        if (this.planActual?.id === id) {
          this.planActual = response.data;
        }
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async eliminarPlan(id) {
      this.cargando = true;
      this.error = null;
      try {
        await planService.eliminarPlan(id);
        this.planes = this.planes.filter(p => p.id !== id);
        if (this.planActual?.id === id) {
          this.planActual = null;
        }
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    limpiarPlanActual() {
      this.planActual = null;
      this.error = null;
    }
  }
});
