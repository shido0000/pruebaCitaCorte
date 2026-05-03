namespace API.Data.Enum.Multibarbero
{
    /// <summary>
    /// Estados de una reserva
    /// </summary>
    public enum EstadoReserva
    {
        Pendiente = 1,
        Confirmada = 2,
        EnProceso = 3,
        Completada = 4,
        CanceladaCliente = 5,
        CanceladaProveedor = 6,
        Rechazada = 7
    }
}
