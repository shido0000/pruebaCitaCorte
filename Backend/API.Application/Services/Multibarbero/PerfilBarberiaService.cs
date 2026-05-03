using API.Application.Contracts.Multibarbero;
using API.Application.Dtos.Multibarbero;
using API.Domain.Entities.Multibarbero;
using API.Domain.Interfaces.Repositories.Multibarbero;
using Microsoft.Extensions.Logging;

namespace API.Application.Services.Multibarbero;

public class PerfilBarberiaService : ServiceBase, IPerfilBarberiaService
{
    private readonly IPerfilBarberiaRepository _perfilBarberiaRepository;
    private readonly ILogger<PerfilBarberiaService> _logger;

    public PerfilBarberiaService(
        IPerfilBarberiaRepository perfilBarberiaRepository,
        ILogger<PerfilBarberiaService> logger)
    {
        _perfilBarberiaRepository = perfilBarberiaRepository;
        _logger = logger;
    }

    public async Task<PerfilBarberiaDto?> ObtenerPorIdAsync(Guid id)
    {
        try
        {
            var entidad = await _perfilBarberiaRepository.ObtenerPorIdAsync(id);
            if (entidad == null) return null;

            return new PerfilBarberiaDto
            {
                Id = entidad.Id,
                BarberiaId = entidad.BarberiaId,
                Descripcion = entidad.Descripcion,
                AnoExperiencia = entidad.AnoExperiencia,
                Especialidades = entidad.Especialidades,
                CertificadoCalidad = entidad.CertificadoCalidad,
                FechaCreacion = entidad.FechaCreacion,
                FechaActualizacion = entidad.FechaActualizacion
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener perfil de barbería por ID: {Id}", id);
            throw;
        }
    }

    public async Task<PerfilBarberiaDto?> ObtenerPorBarberiaAsync(Guid barberiaId)
    {
        try
        {
            var entidad = await _perfilBarberiaRepository.ObtenerPorBarberiaAsync(barberiaId);
            if (entidad == null) return null;

            return new PerfilBarberiaDto
            {
                Id = entidad.Id,
                BarberiaId = entidad.BarberiaId,
                Descripcion = entidad.Descripcion,
                AnoExperiencia = entidad.AnoExperiencia,
                Especialidades = entidad.Especialidades,
                CertificadoCalidad = entidad.CertificadoCalidad,
                FechaCreacion = entidad.FechaCreacion,
                FechaActualizacion = entidad.FechaActualizacion
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener perfil de barbería por BarberiaID: {BarberiaId}", barberiaId);
            throw;
        }
    }

    public async Task<CrearPerfilBarberiaOutputDto> CrearAsync(CrearPerfilBarberiaInputDto input)
    {
        try
        {
            var entidad = new PerfilBarberia
            {
                BarberiaId = input.BarberiaId,
                Descripcion = input.Descripcion,
                AnoExperiencia = input.AnoExperiencia,
                Especialidades = input.Especialidades,
                CertificadoCalidad = input.CertificadoCalidad,
                FechaCreacion = DateTime.UtcNow
            };

            var entidadCreada = await _perfilBarberiaRepository.CrearAsync(entidad);
            
            return new CrearPerfilBarberiaOutputDto
            {
                Id = entidadCreada.Id,
                Mensaje = "Perfil de barbería creado exitosamente"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear perfil de barbería");
            throw;
        }
    }

    public async Task<bool> ActualizarAsync(ActualizarPerfilBarberiaInputDto input)
    {
        try
        {
            var entidad = await _perfilBarberiaRepository.ObtenerPorIdAsync(input.Id);
            if (entidad == null) return false;

            entidad.Descripcion = input.Descripcion ?? entidad.Descripcion;
            entidad.AnoExperiencia = input.AnoExperiencia ?? entidad.AnoExperiencia;
            entidad.Especialidades = input.Especialidades ?? entidad.Especialidades;
            entidad.CertificadoCalidad = input.CertificadoCalidad ?? entidad.CertificadoCalidad;
            entidad.FechaActualizacion = DateTime.UtcNow;

            await _perfilBarberiaRepository.ActualizarAsync(entidad);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar perfil de barbería: {Id}", input.Id);
            throw;
        }
    }

    public async Task<bool> EliminarAsync(Guid id)
    {
        try
        {
            var entidad = await _perfilBarberiaRepository.ObtenerPorIdAsync(id);
            if (entidad == null) return false;

            await _perfilBarberiaRepository.EliminarAsync(entidad);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar perfil de barbería: {Id}", id);
            throw;
        }
    }
}
