using API.Application.Dtos.Multibarbero.Afiliacion;
using FluentValidation;

namespace API.Application.Validadotors.Multibarbero
{
    public class CrearSolicitudAfiliacionDtoValidator : AbstractValidator<CrearSolicitudAfiliacionInputDto>
    {
        public CrearSolicitudAfiliacionDtoValidator()
        {
            RuleFor(x => x.BarberoId)
                .NotEmpty().WithMessage("El ID del barbero es obligatorio.");

            RuleFor(x => x.BarberiaId)
                .NotEmpty().WithMessage("El ID de la barbería es obligatorio.");

            RuleFor(x => x.Mensaje)
                .MaximumLength(500).WithMessage("El mensaje no puede exceder los 500 caracteres.");
        }
    }
}
