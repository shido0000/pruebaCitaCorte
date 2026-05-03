using API.Application.Dtos.Multibarbero.Reserva;
using FluentValidation;

namespace API.Application.Validadotors.Multibarbero
{
    public class CrearReservaDtoValidator : AbstractValidator<CrearReservaInputDto>
    {
        public CrearReservaDtoValidator()
        {
            RuleFor(x => x.FechaHoraInicio)
                .NotEmpty().WithMessage("La fecha y hora de inicio es obligatoria.")
                .GreaterThan(DateTime.Now).WithMessage("La fecha y hora de inicio debe ser futura.");

            RuleFor(x => x.FechaHoraFin)
                .NotEmpty().WithMessage("La fecha y hora de fin es obligatoria.")
                .GreaterThan(x => x.FechaHoraInicio).WithMessage("La fecha y hora de fin debe ser posterior a la de inicio.");

            RuleFor(x => x.BarberoId)
                .NotEmpty().WithMessage("El ID del barbero es obligatorio.");

            RuleFor(x => x.ClienteId)
                .NotEmpty().WithMessage("El ID del cliente es obligatorio.");

            RuleFor(x => x.ServicioId)
                .NotEmpty().WithMessage("El ID del servicio es obligatorio.");

            RuleFor(x => x.Nota)
                .MaximumLength(500).WithMessage("La nota no puede exceder los 500 caracteres.");
        }
    }
}
