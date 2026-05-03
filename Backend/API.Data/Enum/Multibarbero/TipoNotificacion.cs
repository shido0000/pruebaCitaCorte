namespace API.Data.Enum.Multibarbero
{
    /// <summary>
    /// Tipo de notificación del sistema
    /// </summary>
    public enum TipoNotificacion
    {
        SolicitudAfiliacionNueva = 1,
        SolicitudAfiliacionAprobada = 2,
        SolicitudAfiliacionRechazada = 3,
        CambioSuscripcionSolicitada = 4,
        CambioSuscripcionAprobada = 5,
        CambioSuscripcionRechazada = 6,
        ReservaNueva = 7,
        ReservaConfirmada = 8,
        ReservaCancelada = 9,
        ReservaRechazada = 10,
        SuscripcionPorVencer = 11,
        SuscripcionVencida = 12,
        NuevoRegistroBarberia = 13
    }
}
