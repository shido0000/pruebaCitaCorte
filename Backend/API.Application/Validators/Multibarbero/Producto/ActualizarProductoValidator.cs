using API.Application.Dtos.Multibarbero.Producto;
using FluentValidation;

namespace API.Application.Validators.Multibarbero.Producto
{
    /// <summary>
    /// Validador para la actualización de productos
    /// </summary>
    public class ActualizarProductoValidator : AbstractValidator<ActualizarProductoDto>
    {
        public ActualizarProductoValidator()
        {
            // Validar Id
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID del producto es obligatorio.");

            // Validar Nombre (opcional en actualización)
            RuleFor(x => x.Nombre)
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Nombre));

            // Validar Descripción (opcional)
            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");

            // Validar Precio (opcional en actualización)
            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.")
                .LessThanOrEqualTo(999999.99m).WithMessage("El precio no puede exceder 999,999.99")
                .When(x => x.Precio > 0);

            // Validar Stock (opcional en actualización)
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

            // Validar ImagenUrl (opcional)
            RuleFor(x => x.ImagenUrl)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.ImagenUrl))
                .WithMessage("La URL de la imagen debe ser válida.");

            // Validar CategoriaProductoId (opcional)
            RuleFor(x => x.CategoriaProductoId)
                .NotEmpty().When(x => x.CategoriaProductoId.HasValue)
                .WithMessage("La categoría del producto es obligatoria si se proporciona.");
        }

        private bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
