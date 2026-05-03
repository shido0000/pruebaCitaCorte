using API.Application.Dtos.Multibarbero.CategoriaProducto;
using FluentValidation;

namespace API.Application.Validators.Multibarbero.CategoriaProducto;

public class CrearCategoriaProductoValidator : AbstractValidator<CrearCategoriaProductoDto>
{
    public CrearCategoriaProductoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).When(x => x.Descripcion != null)
            .WithMessage("La descripción no puede exceder los 500 caracteres");
    }
}
