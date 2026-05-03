using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Multibarbero
{
    public class NotificacionValidator : AbstractValidator<Notificacion>
    {
        private readonly IUnitOfWork<Notificacion> _repositorios;

        public NotificacionValidator(IUnitOfWork<Notificacion> repositorios)
        {
            _repositorios = repositorios;

            RuleFor(n => n.UsuarioDestinoId)
                .NotEmpty().WithMessage("El usuario destino es obligatorio.");

            RuleFor(n => n.Tipo)
                .IsInEnum().WithMessage("El tipo de notificación no es válido.");

            RuleFor(n => n.Titulo)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MaximumLength(200).WithMessage("El título debe tener máximo {MaxLength} caracteres.");

            RuleFor(n => n.Mensaje)
                .NotEmpty().WithMessage("El mensaje es obligatorio.")
                .MaximumLength(1000).WithMessage("El mensaje debe tener máximo {MaxLength} caracteres.");

            RuleFor(n => n.UrlAccion)
                .MaximumLength(500).WithMessage("La URL de acción debe tener máximo {MaxLength} caracteres.")
                .When(n => !string.IsNullOrEmpty(n.UrlAccion));
        }
    }
}
