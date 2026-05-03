using API.Application.Dtos.Multibarbero;
using FluentValidation;

namespace API.Application.Validators.Multibarbero
{
    public class ActualizarServicioInputDtoValidator : AbstractValidator<ActualizarServicioInputDto>
    {
        public ActualizarServicioInputDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID es obligatorio");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres");

            RuleFor(x => x.Precio)
                .GreaterThanOrEqualTo(0).WithMessage("El precio debe ser mayor o igual a 0");

            RuleFor(x => x.DuracionMinutos)
                .InclusiveBetween(1, 480).WithMessage("La duración debe estar entre 1 y 480 minutos");

            RuleFor(x => x.IdCategoriaServicio)
                .NotEmpty().WithMessage("La categoría es obligatoria");

            RuleFor(x => x.IdBarberia)
                .NotEmpty().WithMessage("La barbería es obligatoria");
        }
    }
}
