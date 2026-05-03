using API.Application.Dtos.Multibarbero.Producto;
using FluentValidation;

namespace API.Application.Validators.Multibarbero.Producto
{
    /// <summary>
    /// Validador para la creación de productos
    /// </summary>
    public class CrearProductoValidator : AbstractValidator<CrearProductoDto>
    {
        public CrearProductoValidator()
        {
            // Validar Nombre
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            // Validar Descripción (opcional)
            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");

            // Validar Precio
            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.")
                .LessThanOrEqualTo(999999.99m).WithMessage("El precio no puede exceder 999,999.99");

            // Validar Stock
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

            // Validar ImagenUrl (opcional)
            RuleFor(x => x.ImagenUrl)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.ImagenUrl))
                .WithMessage("La URL de la imagen debe ser válida.");

            // Validar CategoriaProductoId (opcional)
            RuleFor(x => x.CategoriaProductoId)
                .NotEmpty().When(x => true) // Hacerlo obligatorio si se requiere categoría
                .WithMessage("La categoría del producto es obligatoria.");
        }

        private bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
