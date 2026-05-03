using API.Data.Entidades.Multibarbero;
using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.DbContexts
{
    public interface IApiDbContext
    {
        #region Entities Seguridad

        DbSet<Usuario> Usuarios { get; set; }
        DbSet<Traza> Trazas { get; set; }

        #endregion

        #region Entities Multibarbero

        DbSet<PlanSuscripcion> PlanesSuscripcion { get; set; }
        DbSet<CaracteristicaPlan> CaracteristicasPlan { get; set; }
        DbSet<LimitesPlan> LimitesPlan { get; set; }
        DbSet<PerfilBarbero> PerfilesBarbero { get; set; }
        DbSet<PerfilBarberia> PerfilesBarberia { get; set; }
        DbSet<Servicio> Servicios { get; set; }
        DbSet<CategoriaServicio> CategoriasServicio { get; set; }
        DbSet<PerfilCliente> PerfilesCliente { get; set; }
        DbSet<Producto> Productos { get; set; }
        DbSet<CategoriaProducto> CategoriasProducto { get; set; }
        DbSet<Reserva> Reservas { get; set; }
        DbSet<SolicitudAfiliacion> SolicitudesAfiliacion { get; set; }
        DbSet<AfiliacionBarbero> AfiliacionesBarbero { get; set; }
        DbSet<Notificacion> Notificaciones { get; set; }
        DbSet<EstadisticaBarbero> EstadisticasBarbero { get; set; }
        DbSet<EstadisticaBarberia> EstadisticasBarberia { get; set; }
        DbSet<EstadisticaProducto> EstadisticasProducto { get; set; }

        #endregion
    }
}
