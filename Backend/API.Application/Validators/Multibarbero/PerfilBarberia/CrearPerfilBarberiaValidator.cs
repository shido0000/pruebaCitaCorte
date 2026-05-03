using FluentValidation;
using API.Application.Dtos.Multibarbero.PerfilBarberia;

namespace API.Application.Validators.Multibarbero.PerfilBarberia
{
    /// <summary>
    /// Validador para la creación de perfiles de barbería
    /// </summary>
    public class CrearPerfilBarberiaValidator : AbstractValidator<CrearPerfilBarberiaDto>
    {
        public CrearPerfilBarberiaValidator()
        {
            // Validar UsuarioId
            RuleFor(x => x.UsuarioId)
                .NotEmpty().WithMessage("El ID del usuario es obligatorio.");

            // Validar NombreComercial
            RuleFor(x => x.NombreComercial)
                .NotEmpty().WithMessage("El nombre comercial es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre comercial no puede exceder los 100 caracteres.");

            // Validar Dirección
            RuleFor(x => x.Direccion)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(200).WithMessage("La dirección no puede exceder los 200 caracteres.");

            // Validar Teléfono
            RuleFor(x => x.Telefono)
                .NotEmpty().WithMessage("El teléfono es obligatorio.")
                .Matches(@"^\+?[\d\s\-\(\)]+$").WithMessage("El teléfono debe tener un formato válido.")
                .MinimumLength(7).WithMessage("El teléfono debe tener al menos 7 dígitos.")
                .MaximumLength(20).WithMessage("El teléfono no puede exceder los 20 caracteres.");

            // Validar Logo (opcional pero con formato correcto si se proporciona)
            RuleFor(x => x.Logo)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.Logo))
                .WithMessage("El logo debe ser una URL válida.");

            // Validar FotoPortada (opcional pero con formato correcto si se proporciona)
            RuleFor(x => x.FotoPortada)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.FotoPortada))
                .WithMessage("La foto de portada debe ser una URL válida.");

            // Validar Descripción (opcional)
            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");

            // Validar Coordenadas (opcionales, pero si una existe, la otra también)
            RuleFor(x => x.Longitud)
                .Must((modelo, longitud) => 
                {
                    if (!modelo.Latitud.HasValue && !longitud.HasValue)
                        return true;
                    return modelo.Latitud.HasValue && longitud.HasValue;
                }).WithMessage("Si proporciona coordenadas, debe incluir tanto latitud como longitud.");

            RuleFor(x => x.Latitud)
                .InclusiveBetween(-90, 90).When(x => x.Latitud.HasValue)
                .WithMessage("La latitud debe estar entre -90 y 90 grados.");

            RuleFor(x => x.Longitud)
                .InclusiveBetween(-180, 180).When(x => x.Longitud.HasValue)
                .WithMessage("La longitud debe estar entre -180 y 180 grados.");

            // Validar Horarios
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
                }).WithMessage("El horario de cierre debe ser posterior al horario de apertura.");

            // Validar Capacidad Máxima
            RuleFor(x => x.CapacidadMaxima)
                .GreaterThan(0).When(x => x.CapacidadMaxima.HasValue)
                .WithMessage("La capacidad máxima debe ser mayor a 0.");

            // Validar MaxBarberosPermitidos
            RuleFor(x => x.MaxBarberosPermitidos)
                .GreaterThan(0).WithMessage("El máximo de barberos permitidos debe ser mayor a 0.")
                .LessThanOrEqualTo(50).WithMessage("El máximo de barberos permitidos no puede exceder 50.");

            // Validar Días Laborables (JSON válido)
            RuleFor(x => x.DiasLaborablesJson)
                .Must(BeValidJsonArray).When(x => !string.IsNullOrEmpty(x.DiasLaborablesJson))
                .WithMessage("Los días laborables deben ser un array JSON válido.");
        }

        private bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private bool BeValidJsonArray(string json)
        {
            try
            {
                // Intento básico de validación de JSON array
                return json.Trim().StartsWith("[") && json.Trim().EndsWith("]");
            }
            catch
            {
                return false;
            }
        }
    }
}
