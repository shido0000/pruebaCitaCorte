using API.Application.Dtos.Multibarbero.CategoriaServicio;
using FluentValidation;

namespace API.Application.Validators.Multibarbero.CategoriaServicio;

public class ActualizarCategoriaServicioValidator : AbstractValidator<ActualizarCategoriaServicioDto>
{
    public ActualizarCategoriaServicioValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID debe ser mayor a cero");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).When(x => x.Descripcion != null)
            .WithMessage("La descripción no puede exceder los 500 caracteres");
    }
}
