import api from './api';

const BASE_URL = '/api/multibarbero/productos';

export default {
  async obtenerProductos(filtros = {}) {
    const params = {
      pagina: filtros.pagina || 1,
      tamañoPagina: filtros.tamañoPagina || 10,
      ...filtros
    };
    delete params.pagina;
    delete params.tamañoPagina;
    return await api.get(BASE_URL, { params });
  },

  async obtenerProducto(id) {
    return await api.get(`${BASE_URL}/${id}`);
  },

  async crearProducto(productoData) {
    return await api.post(BASE_URL, productoData);
  },

  async actualizarProducto(id, productoData) {
    return await api.put(`${BASE_URL}/${id}`, productoData);
  },

  async eliminarProducto(id) {
    return await api.delete(`${BASE_URL}/${id}`);
  },

  async obtenerCategorias() {
    return await api.get('/api/multibarbero/categorias-producto');
  },

  async crearCategoria(categoriaData) {
    return await api.post('/api/multibarbero/categorias-producto', categoriaData);
  },

  async actualizarCategoria(id, categoriaData) {
    return await api.put(`/api/multibarbero/categorias-producto/${id}`, categoriaData);
  },

  async eliminarCategoria(id) {
    return await api.delete(`/api/multibarbero/categorias-producto/${id}`);
  }
};
