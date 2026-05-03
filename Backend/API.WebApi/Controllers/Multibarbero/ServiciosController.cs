using API.Application.Contracts.Multibarbero;
using API.Application.Dtos.Multibarbero;
using API.Domain.Entities.Multibarbero;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.WebApi.Controllers.Multibarbero
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServiciosController : ControllerBase
    {
        private readonly IServicioService _servicioService;

        public ServiciosController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        /// <summary>
        /// Obtiene todos los servicios accesibles para el usuario actual
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ServicioDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerTodos()
        {
            var servicios = await _servicioService.ObtenerTodosAsync();
            var dtos = servicios.Select(s => new ServicioDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Descripcion = s.Descripcion,
                Precio = s.Precio,
                DuracionMinutos = s.DuracionMinutos,
                IdCategoriaServicio = s.IdCategoriaServicio,
                IdBarberia = s.IdBarberia,
                Estado = s.Estado,
                FechaCreacion = s.FechaCreacion,
                FechaModificacion = s.FechaModificacion
            });
            return Ok(dtos);
        }

        /// <summary>
        /// Obtiene un servicio por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServicioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var servicio = await _servicioService.ObtenerPorIdAsync(id);
            if (servicio == null)
                return NotFound();

            var dto = new ServicioDto
            {
                Id = servicio.Id,
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                Precio = servicio.Precio,
                DuracionMinutos = servicio.DuracionMinutos,
                IdCategoriaServicio = servicio.IdCategoriaServicio,
                IdBarberia = servicio.IdBarberia,
                Estado = servicio.Estado,
                FechaCreacion = servicio.FechaCreacion,
                FechaModificacion = servicio.FechaModificacion
            };
            return Ok(dto);
        }

        /// <summary>
        /// Obtiene los servicios de una barbería específica
        /// </summary>
        [HttpGet("barberia/{idBarberia}")]
        [ProducesResponseType(typeof(IEnumerable<ServicioDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerPorBarberia(Guid idBarberia)
        {
            var servicios = await _servicioService.ObtenerPorBarberiaAsync(idBarberia);
            var dtos = servicios.Select(s => new ServicioDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Descripcion = s.Descripcion,
                Precio = s.Precio,
                DuracionMinutos = s.DuracionMinutos,
                IdCategoriaServicio = s.IdCategoriaServicio,
                IdBarberia = s.IdBarberia,
                Estado = s.Estado,
                FechaCreacion = s.FechaCreacion,
                FechaModificacion = s.FechaModificacion
            });
            return Ok(dtos);
        }

        /// <summary>
        /// Crea un nuevo servicio
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ServicioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Crear([FromBody] CrearServicioInputDto inputDto)
        {
            var entidad = new Servicio
            {
                Nombre = inputDto.Nombre,
                Descripcion = inputDto.Descripcion,
                Precio = inputDto.Precio,
                DuracionMinutos = inputDto.DuracionMinutos,
                IdCategoriaServicio = inputDto.IdCategoriaServicio,
                IdBarberia = inputDto.IdBarberia
            };

            var servicioCreado = await _servicioService.CrearAsync(entidad);

            var dto = new ServicioDto
            {
                Id = servicioCreado.Id,
                Nombre = servicioCreado.Nombre,
                Descripcion = servicioCreado.Descripcion,
                Precio = servicioCreado.Precio,
                DuracionMinutos = servicioCreado.DuracionMinutos,
                IdCategoriaServicio = servicioCreado.IdCategoriaServicio,
                IdBarberia = servicioCreado.IdBarberia,
                Estado = servicioCreado.Estado,
                FechaCreacion = servicioCreado.FechaCreacion
            };

            return CreatedAtAction(nameof(ObtenerPorId), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Actualiza un servicio existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ServicioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarServicioInputDto inputDto)
        {
            if (id != inputDto.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo");

            var servicioActualizado = await _servicioService.ActualizarAsync(new Servicio
            {
                Id = inputDto.Id,
                Nombre = inputDto.Nombre,
                Descripcion = inputDto.Descripcion,
                Precio = inputDto.Precio,
                DuracionMinutos = inputDto.DuracionMinutos,
                IdCategoriaServicio = inputDto.IdCategoriaServicio,
                IdBarberia = inputDto.IdBarberia,
                Estado = inputDto.Estado ?? true
            });

            var dto = new ServicioDto
            {
                Id = servicioActualizado.Id,
                Nombre = servicioActualizado.Nombre,
                Descripcion = servicioActualizado.Descripcion,
                Precio = servicioActualizado.Precio,
                DuracionMinutos = servicioActualizado.DuracionMinutos,
                IdCategoriaServicio = servicioActualizado.IdCategoriaServicio,
                IdBarberia = servicioActualizado.IdBarberia,
                Estado = servicioActualizado.Estado,
                FechaCreacion = servicioActualizado.FechaCreacion,
                FechaModificacion = servicioActualizado.FechaModificacion
            };

            return Ok(dto);
        }

        /// <summary>
        /// Elimina un servicio por su ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            await _servicioService.EliminarAsync(id);
            return NoContent();
        }
    }
}
