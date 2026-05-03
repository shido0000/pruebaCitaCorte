using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace API.Application.Services
{
    public abstract class ServiceBase
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected ServiceBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<Usuario> ObtenerUsuarioActualAsync()
        {
            var usuarioNombre = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            
            if (string.IsNullOrEmpty(usuarioNombre))
                throw new CustomException 
                { 
                    Status = StatusCodes.Status401Unauthorized, 
                    Message = "Usuario no autenticado" 
                };

            // Nota: Esto requiere acceso al repositorio de usuarios
            // Se debe inyectar en las clases hijas o pasar como parámetro
            throw new NotImplementedException("Debe implementar la obtención del usuario actual");
        }
    }
}
