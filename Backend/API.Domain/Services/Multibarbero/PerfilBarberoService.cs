using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Multibarbero
{
    public class PerfilBarberoService : BasicService<PerfilBarbero, PerfilBarberoValidator>, IPerfilBarberoService
    {
        public PerfilBarberoService(IUnitOfWork<PerfilBarbero> repositorios, IHttpContextAccessor httpContext)
            : base(repositorios, httpContext)
        {
        }

        public async Task<IEnumerable<PerfilBarbero>> ObtenerBarberosPorBarberia(Guid barberiaId)
        {
            return await _repositorios.BasicRepository.GetQuery()
                .Where(b => b.PerfilBarberiaId == barberiaId)
                .Include(b => b.Usuario)
                .ToListAsync();
        }

        public async Task<PerfilBarbero?> ObtenerBarberoPorUsuarioId(Guid usuarioId)
        {
            return await _repositorios.BasicRepository.FirstAsync(b => b.UsuarioId == usuarioId);
        }

        public async Task<bool> EsBarberoPrincipal(Guid barberoId)
        {
            var barbero = await _repositorios.BasicRepository.FirstAsync(b => b.Id == barberoId);
            return barbero != null && barbero.PerfilBarberiaId.HasValue;
        }
    }
}
