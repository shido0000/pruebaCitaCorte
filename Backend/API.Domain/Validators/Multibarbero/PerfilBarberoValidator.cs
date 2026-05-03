using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Multibarbero
{
    public class PerfilBarberoValidator : AbstractValidator<PerfilBarbero>
    {
        private readonly IUnitOfWork<PerfilBarbero> _repositorios;

        public PerfilBarberoValidator(IUnitOfWork<PerfilBarbero> repositorios)
        {
            _repositorios = repositorios;

            RuleFor(p => p.UsuarioId)
                .NotEmpty().WithMessage("El usuario es obligatorio.");

            RuleFor(p => p.Biografia)
                .MaximumLength(1000).WithMessage("La biografía debe tener máximo {MaxLength} caracteres.")
                .When(p => !string.IsNullOrEmpty(p.Biografia));

            RuleFor(p => p.ExperienciaAnios)
                .GreaterThanOrEqualTo(0).WithMessage("Los años de experiencia no pueden ser negativos.")
                .LessThanOrEqualTo(50).WithMessage("Los años de experiencia no pueden ser mayores a 50.");

            RuleFor(p => p.CalificacionPromedio)
                .GreaterThanOrEqualTo(0).WithMessage("La calificación no puede ser negativa.")
                .LessThanOrEqualTo(5).WithMessage("La calificación máxima es 5.");

            RuleFor(p => p.TotalReseñas)
                .GreaterThanOrEqualTo(0).WithMessage("El total de reseñas no puede ser negativo.");
        }
    }
}
