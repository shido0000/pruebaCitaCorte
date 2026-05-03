import api from './api';

const BASE_URL = '/api/multibarbero/suscripciones';

export default {
  async obtenerSuscripcion(perfilId, tipoPerfil) {
    return await api.get(`${BASE_URL}/perfil`, {
      params: { perfilId, tipoPerfil }
    });
  },

  async obtenerHistorial(perfilId, tipoPerfil) {
    return await api.get(`${BASE_URL}/historial`, {
      params: { perfilId, tipoPerfil }
    });
  },

  async obtenerSolicitudesCambio(perfilId, tipoPerfil) {
    return await api.get(`${BASE_URL}/solicitudes-cambio`, {
      params: { perfilId, tipoPerfil }
    });
  },

  async solicitarCambio(suscripcionId, planId, motivo) {
    return await api.post(`${BASE_URL}/${suscripcionId}/solicitar-cambio`, {
      planId,
      motivo
    });
  },

  async renovar(suscripcionId) {
    return await api.post(`${BASE_URL}/${suscripcionId}/renovar`);
  },

  async cancelar(suscripcionId, motivo) {
    return await api.post(`${BASE_URL}/${suscripcionId}/cancelar`, { motivo });
  },

  async responderSolicitud(solicitudId, aprobada, observaciones = '') {
    return await api.post(`/api/multibarbero/solicitudes-cambio/${solicitudId}/responder`, {
      aprobada,
      observaciones
    });
  },

  async obtenerSolicitudesPendientes() {
    return await api.get('/api/multibarbero/solicitudes-cambio/pendientes');
  },

  async aprobarSolicitud(solicitudId, observaciones = '') {
    return await this.responderSolicitud(solicitudId, true, observaciones);
  },

  async rechazarSolicitud(solicitudId, observaciones = '') {
    return await this.responderSolicitud(solicitudId, false, observaciones);
  }
};
