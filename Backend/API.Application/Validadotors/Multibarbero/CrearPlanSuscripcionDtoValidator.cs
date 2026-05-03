using API.Application.Dtos.Multibarbero.PlanSuscripcion;
using FluentValidation;

namespace API.Application.Validadotors.Multibarbero
{
    public class CrearPlanSuscripcionDtoValidator : AbstractValidator<CrearPlanSuscripcionInputDto>
    {
        public CrearPlanSuscripcionDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del plan es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción del plan es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");

            RuleFor(x => x.PrecioMensual)
                .GreaterThanOrEqualTo(0).WithMessage("El precio mensual debe ser mayor o igual a 0.");

            RuleFor(x => x.PrecioAnual)
                .GreaterThanOrEqualTo(0).WithMessage("El precio anual debe ser mayor o igual a 0.");

            RuleFor(x => x.DuracionDias)
                .GreaterThan(0).WithMessage("La duración en días debe ser mayor a 0.");

            RuleFor(x => x.TipoPlan)
                .IsInEnum().WithMessage("El tipo de plan no es válido.");
        }
    }
}
