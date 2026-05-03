using FluentValidation;
using API.Application.Dtos.Multibarbero.PerfilBarberia;

namespace API.Application.Validators.Multibarbero.PerfilBarberia
{
    /// <summary>
    /// Validador para la actualización de perfiles de barbería
    /// </summary>
    public class ActualizarPerfilBarberiaValidator : AbstractValidator<ActualizarPerfilBarberiaDto>
    {
        public ActualizarPerfilBarberiaValidator()
        {
            // Validar Id
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID del perfil de barbería es obligatorio.");

            // Validar NombreComercial (opcional en actualización)
            RuleFor(x => x.NombreComercial)
                .MaximumLength(100).WithMessage("El nombre comercial no puede exceder los 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.NombreComercial));

            // Validar Dirección (opcional en actualización)
            RuleFor(x => x.Direccion)
                .MaximumLength(200).WithMessage("La dirección no puede exceder los 200 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Direccion));

            // Validar Teléfono (opcional en actualización)
            RuleFor(x => x.Telefono)
                .Matches(@"^\+?[\d\s\-\(\)]+$").WithMessage("El teléfono debe tener un formato válido.")
                .MinimumLength(7).WithMessage("El teléfono debe tener al menos 7 dígitos.")
                .MaximumLength(20).WithMessage("El teléfono no puede exceder los 20 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Telefono));

            // Validar Logo (opcional)
            RuleFor(x => x.Logo)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.Logo))
                .WithMessage("El logo debe ser una URL válida.");

            // Validar FotoPortada (opcional)
            RuleFor(x => x.FotoPortada)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.FotoPortada))
                .WithMessage("La foto de portada debe ser una URL válida.");

            // Validar Descripción (opcional)
            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");

            // Validar Coordenadas (opcionales)
            RuleFor(x => x.Latitud)
                .InclusiveBetween(-90, 90).When(x => x.Latitud.HasValue)
                .WithMessage("La latitud debe estar entre -90 y 90 grados.");

            RuleFor(x => x.Longitud)
                .InclusiveBetween(-180, 180).When(x => x.Longitud.HasValue)
                .WithMessage("La longitud debe estar entre -180 y 180 grados.");

            // Validar Horarios (opcionales)
            RuleFor(x => x.HorarioCierre)
                .Must((modelo, cierre) => 
                {
                    if (!modelo.HorarioApertura.HasValue && !cierre.HasValue)
                        return true;
                    if (modelo.HorarioApertura.HasValue && !cierre.HasValue)
                        return false;
                    if (!modelo.HorarioApertura.HasValue && cierre.HasValue)
                        return false;
                    return modelo.HorarioApertura.Value < cierre.Value;
                }).WithMessage("El horario de cierre debe ser posterior al horario de apertura.")
                .When(x => x.HorarioApertura.HasValue || x.HorarioCierre.HasValue);

            // Validar Capacidad Máxima (opcional)
            RuleFor(x => x.CapacidadMaxima)
                .GreaterThan(0).When(x => x.CapacidadMaxima.HasValue)
                .WithMessage("La capacidad máxima debe ser mayor a 0.");

            // Validar MaxBarberosPermitidos (opcional)
            RuleFor(x => x.MaxBarberosPermitidos)
                .GreaterThan(0).When(x => x.MaxBarberosPermitidos.HasValue)
                .WithMessage("El máximo de barberos permitidos debe ser mayor a 0.")
                .LessThanOrEqualTo(50).When(x => x.MaxBarberosPermitidos.HasValue)
                .WithMessage("El máximo de barberos permitidos no puede exceder 50.");
        }

        private bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
