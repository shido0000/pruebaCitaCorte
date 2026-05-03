using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using API.Domain.Interfaces.Seguridad;

namespace API.Application.Services.Multibarbero
{
    public class ServicioService : ServiceBase<Servicio>, IServicioService
    {
        private readonly IServicioRepository _servicioRepository;
        private readonly IPerfilBarberiaRepository _perfilBarberiaRepository;

        public ServicioService(
            IServicioRepository servicioRepository,
            IPerfilBarberiaRepository perfilBarberiaRepository,
            IUsuarioService usuarioService)
            : base(servicioRepository, usuarioService)
        {
            _servicioRepository = servicioRepository;
            _perfilBarberiaRepository = perfilBarberiaRepository;
        }

        public override async Task<IEnumerable<Servicio>> ObtenerTodosAsync()
        {
            var usuarioActual = await ObtenerUsuarioActualAsync();
            if (usuarioActual.EsRol("Comercial"))
            {
                return await _servicioRepository.ObtenerTodosAsync();
            }

            if (usuarioActual.EsRol("Barbero"))
            {
                var perfilBarberia = await _perfilBarberiaRepository.ObtenerPorBarberoAsync(usuarioActual.Id);
                if (perfilBarberia == null || !perfilBarberia.Any())
                    return Enumerable.Empty<Servicio>();

                var barberiasIds = perfilBarberia.Select(pb => pb.IdBarberia).ToList();
                return await _servicioRepository.ObtenerPorBarberiasAsync(barberiasIds);
            }

            return Enumerable.Empty<Servicio>();
        }

        public async Task<IEnumerable<Servicio>> ObtenerPorBarberiaAsync(Guid idBarberia)
        {
            var usuarioActual = await ObtenerUsuarioActualAsync();

            if (!usuarioActual.EsRol("Comercial"))
            {
                var perfilBarberia = await _perfilBarberiaRepository.ObtenerPorBarberoYBarberiaAsync(usuarioActual.Id, idBarberia);
                if (perfilBarberia == null)
                    throw new UnauthorizedAccessException("No tiene acceso a esta barbería");
            }

            return await _servicioRepository.ObtenerPorBarberiaAsync(idBarberia);
        }

        public async Task<IEnumerable<Servicio>> ObtenerPorCategoriaAsync(Guid idCategoria)
        {
            var usuarioActual = await ObtenerUsuarioActualAsync();

            if (usuarioActual.EsRol("Comercial"))
            {
                return await _servicioRepository.ObtenerPorCategoriaAsync(idCategoria);
            }

            if (usuarioActual.EsRol("Barbero"))
            {
                var perfilBarberia = await _perfilBarberiaRepository.ObtenerPorBarberoAsync(usuarioActual.Id);
                if (perfilBarberia == null || !perfilBarberia.Any())
                    return Enumerable.Empty<Servicio>();

                var barberiasIds = perfilBarberia.Select(pb => pb.IdBarberia).ToList();
                return await _servicioRepository.ObtenerPorCategoriaYBarberiasAsync(idCategoria, barberiasIds);
            }

            return Enumerable.Empty<Servicio>();
        }

        public async Task<bool> ExisteNombreEnBarberiaAsync(string nombre, Guid idBarberia, Guid? idExcluir = null)
        {
            return await _servicioRepository.ExisteNombreEnBarberiaAsync(nombre, idBarberia, idExcluir);
        }

        public override async Task<Servicio> CrearAsync(Servicio entidad)
        {
            var usuarioActual = await ObtenerUsuarioActualAsync();

            if (!usuarioActual.EsRol("Barbero") && !usuarioActual.EsRol("Comercial"))
                throw new UnauthorizedAccessException("No tiene permisos para crear servicios");

            if (await ExisteNombreEnBarberiaAsync(entidad.Nombre, entidad.IdBarberia))
                throw new Exception($"Ya existe un servicio con el nombre '{entidad.Nombre}' en esta barbería");

            entidad.Estado = true;
            return await base.CrearAsync(entidad);
        }

        public override async Task<Servicio> ActualizarAsync(Servicio entidad)
        {
            var usuarioActual = await ObtenerUsuarioActualAsync();

            if (!usuarioActual.EsRol("Barbero") && !usuarioActual.EsRol("Comercial"))
                throw new UnauthorizedAccessException("No tiene permisos para actualizar servicios");

            var servicioExistente = await _servicioRepository.ObtenerPorIdAsync(entidad.Id);
            if (servicioExistente == null)
                throw new Exception("Servicio no encontrado");

            if (servicioExistente.IdBarberia != entidad.IdBarberia && !usuarioActual.EsRol("Comercial"))
                throw new UnauthorizedAccessException("No puede mover servicios entre barberías");

            if (await ExisteNombreEnBarberiaAsync(entidad.Nombre, entidad.IdBarberia, entidad.Id))
                throw new Exception($"Ya existe un servicio con el nombre '{entidad.Nombre}' en esta barbería");

            return await base.ActualizarAsync(entidad);
        }

        public override async Task EliminarAsync(Guid id)
        {
            var usuarioActual = await ObtenerUsuarioActualAsync();

            if (!usuarioActual.EsRol("Barbero") && !usuarioActual.EsRol("Comercial"))
                throw new UnauthorizedAccessException("No tiene permisos para eliminar servicios");

            var servicio = await _servicioRepository.ObtenerPorIdAsync(id);
            if (servicio == null)
                throw new Exception("Servicio no encontrado");

            if (!usuarioActual.EsRol("Comercial"))
            {
                var perfilBarberia = await _perfilBarberiaRepository.ObtenerPorBarberoYBarberiaAsync(usuarioActual.Id, servicio.IdBarberia);
                if (perfilBarberia == null)
                    throw new UnauthorizedAccessException("No tiene acceso a esta barbería");
            }

            await base.EliminarAsync(id);
        }
    }
}
