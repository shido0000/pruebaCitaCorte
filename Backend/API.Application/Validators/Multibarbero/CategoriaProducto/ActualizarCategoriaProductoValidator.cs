using API.Application.Dtos.Multibarbero.CategoriaProducto;
using FluentValidation;

namespace API.Application.Validators.Multibarbero.CategoriaProducto;

public class ActualizarCategoriaProductoValidator : AbstractValidator<ActualizarCategoriaProductoDto>
{
    public ActualizarCategoriaProductoValidator()
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
