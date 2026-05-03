using API.Data.Entidades;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Nomencladores;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;

namespace API.Data.IUnitOfWorks.Interfaces
{
    public interface IUnitOfWork<TEntity> : IDisposable where TEntity : EntidadBase
    {
        #region Seguridad

        IPermisoRepository Permisos { get; }
        IRolPermisoRepository RolesPermisos { get; }
        IRolRepository Roles { get; }
        IUsuarioRepository Usuarios { get; }
        ITrazaRepository Trazas { get; }

        #endregion

        #region Nomencladores

        ICategoriaRepository Categorias { get; }
        IOrigenRepository Origenes { get; }
        IGrupoRepository Grupos { get; }
        IFamiliaRepository Familias { get; }

        #endregion

        #region Multibarbero

        IPlanSuscripcionRepository PlanesSuscripcion { get; }
        IPerfilBarberoRepository PerfilesBarbero { get; }
        IReservaRepository Reservas { get; }
        ISolicitudAfiliacionRepository SolicitudesAfiliacion { get; }
        INotificacionRepository Notificaciones { get; }

        #endregion

        IBaseRepository<TEntity> BasicRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
