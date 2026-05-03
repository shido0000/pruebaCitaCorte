using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Multibarbero
{
    public class PlanSuscripcionValidator : AbstractValidator<PlanSuscripcion>
    {
        private readonly IUnitOfWork<PlanSuscripcion> _repositorios;

        public PlanSuscripcionValidator(IUnitOfWork<PlanSuscripcion> repositorios)
        {
            _repositorios = repositorios;

            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre debe tener máximo {MaxLength} caracteres.");

            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción debe tener máximo {MaxLength} caracteres.");

            RuleFor(p => p.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            RuleFor(p => p.DuracionDias)
                .GreaterThan(0).WithMessage("La duración debe ser mayor a 0 días.");

            RuleFor(p => p.Tipo)
                .IsInEnum().WithMessage("El tipo de plan no es válido.");
        }
    }
}
