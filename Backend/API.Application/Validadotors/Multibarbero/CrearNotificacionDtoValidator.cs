using API.Application.Dtos.Multibarbero.Notificacion;
using FluentValidation;

namespace API.Application.Validadotors.Multibarbero
{
    public class CrearNotificacionDtoValidator : AbstractValidator<CrearNotificacionInputDto>
    {
        public CrearNotificacionDtoValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("El título de la notificación es obligatorio.")
                .MaximumLength(100).WithMessage("El título no puede exceder los 100 caracteres.");

            RuleFor(x => x.Mensaje)
                .NotEmpty().WithMessage("El mensaje de la notificación es obligatorio.")
                .MaximumLength(500).WithMessage("El mensaje no puede exceder los 500 caracteres.");

            RuleFor(x => x.TipoNotificacion)
                .IsInEnum().WithMessage("El tipo de notificación no es válido.");

            RuleFor(x => x.DestinatarioId)
                .NotEmpty().WithMessage("El ID del destinatario es obligatorio.");
        }
    }
}
