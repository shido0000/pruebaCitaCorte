using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Multibarbero
{
    public class ReservaValidator : AbstractValidator<Reserva>
    {
        private readonly IUnitOfWork<Reserva> _repositorios;

        public ReservaValidator(IUnitOfWork<Reserva> repositorios)
        {
            _repositorios = repositorios;

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
        }
    }
}
