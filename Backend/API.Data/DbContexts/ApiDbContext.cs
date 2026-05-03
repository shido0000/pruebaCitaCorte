using API.Data.ConfiguracionEntidades.Nomencladores;
using API.Data.ConfiguracionEntidades.Seguridad;
using API.Data.Entidades.Nomencladores;
using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.DbContexts
{
    public class ApiDbContext : DbContext, IApiDbContext
    {
        #region DbSet Seguridad

        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermiso { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Traza> Trazas { get; set; }

        #endregion

        #region DbSet Nomencladores

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Origen> Origenes { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Familia> Familias { get; set; }

        #endregion

        #region DbSet Multibarbero

        public DbSet<PlanSuscripcion> PlanesSuscripcion { get; set; }
        public DbSet<CaracteristicaPlan> CaracteristicasPlan { get; set; }
        public DbSet<LimitesPlan> LimitesPlan { get; set; }
        public DbSet<PerfilBarbero> PerfilesBarbero { get; set; }
        public DbSet<PerfilBarberia> PerfilesBarberia { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<CategoriaServicio> CategoriasServicio { get; set; }
        public DbSet<PerfilCliente> PerfilesCliente { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<CategoriaProducto> CategoriasProducto { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<SolicitudAfiliacion> SolicitudesAfiliacion { get; set; }
        public DbSet<AfiliacionBarbero> AfiliacionesBarbero { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<EstadisticaBarbero> EstadisticasBarbero { get; set; }
        public DbSet<EstadisticaBarberia> EstadisticasBarberia { get; set; }
        public DbSet<EstadisticaProducto> EstadisticasProducto { get; set; }

        #endregion

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seguridad
            RolPermisoConfiguracionBD.SetEntityBuilder(modelBuilder);
            RolConfiguracionBD.SetEntityBuilder(modelBuilder);
            PermisoConfiguracionBD.SetEntityBuilder(modelBuilder);
            UsuarioConfiguracionBD.SetEntityBuilder(modelBuilder);
            TrazaConfiguracionBD.SetEntityBuilder(modelBuilder);

            // Nomencladores
            CategoriaConfiguracionBD.SetEntityBuilder(modelBuilder);
            OrigenConfiguracionBD.SetEntityBuilder(modelBuilder);
            GrupoConfiguracionBD.SetEntityBuilder(modelBuilder);
            FamiliaConfiguracionBD.SetEntityBuilder(modelBuilder);

            // Multibarbero
            PlanSuscripcionConfiguracionBD.SetEntityBuilder(modelBuilder);
            PerfilBarberoConfiguracionBD.SetEntityBuilder(modelBuilder);
            PerfilBarberiaConfiguracionBD.SetEntityBuilder(modelBuilder);
            ReservaConfiguracionBD.SetEntityBuilder(modelBuilder);
            SolicitudAfiliacionConfiguracionBD.SetEntityBuilder(modelBuilder);
            AfiliacionBarberoConfiguracionBD.SetEntityBuilder(modelBuilder);
            NotificacionConfiguracionBD.SetEntityBuilder(modelBuilder);
            ServicioConfiguracionBD.SetEntityBuilder(modelBuilder);
            ProductoConfiguracionBD.SetEntityBuilder(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
