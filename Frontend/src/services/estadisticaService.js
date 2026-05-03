import api from './api';

export default {
  async obtenerDashboard(tipoPerfil, perfilId, periodo = 'mes') {
    return await api.get('/api/multibarbero/estadisticas/dashboard', {
      params: { tipoPerfil, perfilId, periodo }
    });
  },

  async obtenerEstadisticasReservas(filtros) {
    return await api.get('/api/multibarbero/estadisticas/reservas', {
      params: filtros
    });
  },

  async obtenerEstadisticasIngresos(filtros) {
    return await api.get('/api/multibarbero/estadisticas/ingresos', {
      params: filtros
    });
  },

  async obtenerEstadisticasClientes(filtros) {
    return await api.get('/api/multibarbero/estadisticas/clientes', {
      params: filtros
    });
  },

  async obtenerEstadisticasServicios(filtros) {
    return await api.get('/api/multibarbero/estadisticas/servicios', {
      params: filtros
    });
  },

  async obtenerEstadisticasProductos(filtros) {
    return await api.get('/api/multibarbero/estadisticas/productos', {
      params: filtros
    });
  },

  async exportarReporte(tipo, formato = 'pdf', filtros = {}) {
    return await api.get(`/api/multibarbero/estadisticas/exportar/${tipo}`, {
      params: { formato, ...filtros },
      responseType: 'blob'
    });
  },

  async obtenerMetricasTiempoReal(tipoPerfil, perfilId) {
    return await api.get('/api/multibarbero/estadisticas/tiempo-real', {
      params: { tipoPerfil, perfilId }
    });
  }
};
