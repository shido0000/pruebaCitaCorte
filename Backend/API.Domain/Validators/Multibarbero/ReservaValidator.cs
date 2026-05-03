using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Multibarbero
{
    public class ReservaValidator : AbstractValidator<Reserva>
    {
        private readonly IUnitOfWork<Reserva> _repositorios;
        private readonly IReservaValidacionService _validacionService;

        public ReservaValidator(
            IUnitOfWork<Reserva> repositorios,
            IReservaValidacionService validacionService)
        {
            _repositorios = repositorios;
            _validacionService = validacionService;

            RuleFor(r => r.ClienteId)
                .NotEmpty().WithMessage("El cliente es obligatorio.");

            RuleFor(r => r.ProveedorId)
                .NotEmpty().WithMessage("El proveedor es obligatorio.");

            RuleFor(r => r.ServicioId)
                .NotEmpty().WithMessage("El servicio es obligatorio.");

            RuleFor(r => r.FechaHoraInicio)
                .NotEmpty().WithMessage("La fecha de inicio es obligatoria.")
                .GreaterThan(DateTime.Now.AddHours(-1)).WithMessage("La fecha de inicio no puede ser en el pasado.");

            RuleFor(r => r.FechaHoraFin)
                .NotEmpty().WithMessage("La fecha de fin es obligatoria.")
                .GreaterThan(r => r.FechaHoraInicio).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");

            RuleFor(r => r.PrecioTotal)
                .GreaterThanOrEqualTo(0).WithMessage("El precio total no puede ser negativo.");

            RuleFor(r => r.TipoProveedor)
                .IsInEnum().WithMessage("El tipo de proveedor no es válido.");

            // Validación personalizada de solapamiento
            RuleFor(r => r)
                .MustAsync(ValidarSolapamientoAsync).WithMessage("Ya existe una reserva en ese horario para el mismo proveedor.");
        }

        private async Task<bool> ValidarSolapamientoAsync(Reserva reserva, CancellationToken cancellationToken)
        {
            // Solo validar solapamiento al crear nuevas reservas (cuando Id es Guid.Empty o default)
            if (reserva.Id == Guid.Empty)
            {
                return !await _validacionService.ExisteSolapamientoAsync(
                    reserva.ProveedorId,
                    reserva.FechaHoraInicio,
                    reserva.FechaHoraFin);
            }

            // Para actualizaciones, excluir la propia reserva de la validación
            return !await _validacionService.ExisteSolapamientoAsync(
                reserva.ProveedorId,
                reserva.FechaHoraInicio,
                reserva.FechaHoraFin,
                reserva.Id);
        }
    }
}
