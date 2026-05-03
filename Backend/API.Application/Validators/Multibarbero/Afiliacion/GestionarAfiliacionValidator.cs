using API.Application.Dtos.Multibarbero.Afiliaciones;
using FluentValidation;

namespace API.Application.Validators.Multibarbero.Afiliacion
{
    /// <summary>
    /// Validador para la gestión de afiliaciones (activar/desactivar/cambiar principal)
    /// </summary>
    public class GestionarAfiliacionValidator : AbstractValidator<GestionarAfiliacionInputDto>
    {
        public GestionarAfiliacionValidator()
        {
            // Validar AfiliacionId
            RuleFor(x => x.AfiliacionId)
                .NotEmpty().WithMessage("El ID de la afiliación es obligatorio.");

            // Validar MotivoFin cuando se desactiva
            RuleFor(x => x.MotivoFin)
                .NotEmpty().WithMessage("Debe proporcionar un motivo para finalizar la afiliación.")
                .MaximumLength(500).WithMessage("El motivo no puede exceder los 500 caracteres.")
                .When(x => !x.Activo && string.IsNullOrEmpty(x.MotivoFin));

            // Validar que no se pueda marcar como principal si está inactiva
            RuleFor(x => x.EsPrincipal)
                .Must((modelo, esPrincipal) =>
                {
                    // Si intenta marcar como principal pero está inactiva, es inválido
                    if (esPrincipal.HasValue && esPrincipal.Value && !modelo.Activo)
                        return false;
                    return true;
                }).WithMessage("No se puede marcar una afiliación inactiva como principal.");
        }
    }
}
