using API.Application.Dtos.Multibarbero;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Application.Contracts.Multibarbero;

public interface IPerfilBarberiaService
{
    Task<PerfilBarberiaDto?> ObtenerPorIdAsync(Guid id);
    Task<PerfilBarberiaDto?> ObtenerPorBarberiaAsync(Guid barberiaId);
    Task<CrearPerfilBarberiaOutputDto> CrearAsync(CrearPerfilBarberiaInputDto input);
    Task<bool> ActualizarAsync(ActualizarPerfilBarberiaInputDto input);
    Task<bool> EliminarAsync(Guid id);
}
