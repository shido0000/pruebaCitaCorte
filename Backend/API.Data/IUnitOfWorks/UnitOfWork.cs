using API.Data.DbContexts;
using API.Data.Entidades;
using API.Data.IUnitOfWorks.Interfaces;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Nomencladores;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;
using API.Data.IUnitOfWorks.Repositorios;
using API.Data.IUnitOfWorks.Repositorios.Multibarbero;
using API.Data.IUnitOfWorks.Repositorios.Nomencladores;
using API.Data.IUnitOfWorks.Repositorios.Seguridad;

namespace API.Data.IUnitOfWorks
{
    public class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : EntidadBase
    {
        private readonly ApiDbContext _context;

        #region Seguridad

        public IPermisoRepository Permisos { get; }
        public IRolPermisoRepository RolesPermisos { get; }
        public IRolRepository Roles { get; }
        public IUsuarioRepository Usuarios { get; }
        public ITrazaRepository Trazas { get; }

        #endregion

        #region Nomencladores

        public ICategoriaRepository Categorias { get; }
        public IOrigenRepository Origenes { get; }
        public IGrupoRepository Grupos { get; }
        public IFamiliaRepository Familias { get; }

        #endregion

        #region Multibarbero

        public IPlanSuscripcionRepository PlanesSuscripcion { get; }
        public IPerfilBarberoRepository PerfilesBarbero { get; }
        public IReservaRepository Reservas { get; }
        public ISolicitudAfiliacionRepository SolicitudesAfiliacion { get; }
        public INotificacionRepository Notificaciones { get; }

        #endregion

        public IBaseRepository<TEntity> BasicRepository { get; }

        public UnitOfWork(ApiDbContext context)
        {
            _context = context;

            // Seguridad
            Permisos = new PermisoRepository(context);
            RolesPermisos = new RolPermisoRepository(context);
            Roles = new RolRepository(context);
            Usuarios = new UsuarioRepository(context);
            Trazas = new TrazaRepository(context);

            // Nomencladores
            Categorias = new CategoriaRepository(context);
            Origenes = new OrigenRepository(context);
            Grupos = new GrupoRepository(context);
            Familias = new FamiliaRepository(context);

            // Multibarbero
            PlanesSuscripcion = new PlanSuscripcionRepository(context);
            PerfilesBarbero = new PerfilBarberoRepository(context);
            Reservas = new ReservaRepository(context);
            SolicitudesAfiliacion = new SolicitudAfiliacionRepository(context);
            Notificaciones = new NotificacionRepository(context);

            BasicRepository = new BaseRepository<TEntity>(context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
           => await _context.SaveChangesAsync(cancellationToken);

        public void Dispose() => _context.Dispose();


    }
}
