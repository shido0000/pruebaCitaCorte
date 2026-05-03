using API.Application.Dtos.Multibarbero.PerfilBarbero;
using FluentValidation;

namespace API.Application.Validadotors.Multibarbero
{
    public class CrearPerfilBarberoDtoValidator : AbstractValidator<CrearPerfilBarberoInputDto>
    {
        public CrearPerfilBarberoDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del barbero es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.Apellidos)
                .NotEmpty().WithMessage("Los apellidos del barbero son obligatorios.")
                .MaximumLength(100).WithMessage("Los apellidos no pueden exceder los 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El formato del email no es válido.")
                .MaximumLength(255).WithMessage("El email no puede exceder los 255 caracteres.");

            RuleFor(x => x.Telefono)
                .MaximumLength(20).WithMessage("El teléfono no puede exceder los 20 caracteres.");

            RuleFor(x => x.ExperienciaAnios)
                .GreaterThanOrEqualTo(0).WithMessage("Los años de experiencia deben ser mayor o igual a 0.");

            RuleFor(x => x.Especialidades)
                .MaximumLength(500).WithMessage("Las especialidades no pueden exceder los 500 caracteres.");
        }
    }
}
