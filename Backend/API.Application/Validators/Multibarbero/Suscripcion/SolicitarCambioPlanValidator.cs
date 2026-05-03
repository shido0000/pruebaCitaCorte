using API.Application.Dtos.Multibarbero.Suscripciones;
using FluentValidation;

namespace API.Application.Validators.Multibarbero.Suscripcion
{
    /// <summary>
    /// Validador para solicitar cambio de plan de suscripción
    /// </summary>
    public class SolicitarCambioPlanValidator : AbstractValidator<SolicitarCambioPlanInputDto>
    {
        public SolicitarCambioPlanValidator()
        {
            // Validar PerfilId
            RuleFor(x => x.PerfilId)
                .NotEmpty().WithMessage("El ID del perfil es obligatorio.");

            // Validar TipoPerfil
            RuleFor(x => x.TipoPerfil)
                .NotEmpty().WithMessage("El tipo de perfil es obligatorio.")
                .Must(tipo => tipo == "Barbero" || tipo == "Barberia")
                .WithMessage("El tipo de perfil debe ser 'Barbero' o 'Barberia'.");

            // Validar NuevoPlanId
            RuleFor(x => x.NuevoPlanId)
                .NotEmpty().WithMessage("El ID del nuevo plan es obligatorio.");

            // Validar MotivoSolicitud (opcional pero con longitud máxima)
            RuleFor(x => x.MotivoSolicitud)
                .MaximumLength(500).WithMessage("El motivo de la solicitud no puede exceder los 500 caracteres.");
        }
    }
}
