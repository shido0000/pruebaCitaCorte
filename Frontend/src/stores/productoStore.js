import { defineStore } from 'pinia';
import productoService from '@/services/productoService';

export const useProductoStore = defineStore('producto', {
  state: () => ({
    productos: [],
    productoActual: null,
    categorias: [],
    cargando: false,
    error: null
  }),

  getters: {
    productosPorCategoria: (state) => (categoriaId) => {
      return state.productos.filter(prod => prod.categoriaId === categoriaId);
    },
    productoPorId: (state) => (id) => {
      return state.productos.find(prod => prod.id === id);
    }
  },

  actions: {
    async cargarProductos(filtros = {}) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await productoService.obtenerProductos(filtros);
        this.productos = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async cargarProducto(id) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await productoService.obtenerProducto(id);
        this.productoActual = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async cargarCategorias() {
      this.cargando = true;
      this.error = null;
      try {
        const response = await productoService.obtenerCategorias();
        this.categorias = response.data;
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async crearProducto(productoData) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await productoService.crearProducto(productoData);
        this.productos.push(response.data);
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async actualizarProducto(id, productoData) {
      this.cargando = true;
      this.error = null;
      try {
        const response = await productoService.actualizarProducto(id, productoData);
        const index = this.productos.findIndex(p => p.id === id);
        if (index !== -1) {
          this.productos[index] = response.data;
        }
        if (this.productoActual?.id === id) {
          this.productoActual = response.data;
        }
        return response.data;
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    async eliminarProducto(id) {
      this.cargando = true;
      this.error = null;
      try {
        await productoService.eliminarProducto(id);
        this.productos = this.productos.filter(p => p.id !== id);
        if (this.productoActual?.id === id) {
          this.productoActual = null;
        }
      } catch (error) {
        this.error = error.message;
        throw error;
      } finally {
        this.cargando = false;
      }
    },

    limpiarProductoActual() {
      this.productoActual = null;
      this.error = null;
    }
  }
});
