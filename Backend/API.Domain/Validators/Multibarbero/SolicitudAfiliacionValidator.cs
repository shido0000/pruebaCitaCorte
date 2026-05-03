using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Multibarbero
{
    public class SolicitudAfiliacionValidator : AbstractValidator<SolicitudAfiliacion>
    {
        private readonly IUnitOfWork<SolicitudAfiliacion> _repositorios;

        public SolicitudAfiliacionValidator(IUnitOfWork<SolicitudAfiliacion> repositorios)
        {
            _repositorios = repositorios;

            RuleFor(s => s.BarberoId)
                .NotEmpty().WithMessage("El barbero es obligatorio.");

            RuleFor(s => s.BarberiaId)
                .NotEmpty().WithMessage("La barbería es obligatoria.");

            RuleFor(s => s.Estado)
                .IsInEnum().WithMessage("El estado de la solicitud no es válido.");

            RuleFor(s => s.FechaSolicitud)
                .NotEmpty().WithMessage("La fecha de solicitud es obligatoria.");

            RuleFor(s => s.MotivoRechazo)
                .MaximumLength(500).WithMessage("El motivo de rechazo debe tener máximo {MaxLength} caracteres.")
                .When(s => !string.IsNullOrEmpty(s.MotivoRechazo));

            RuleFor(s => s.Notas)
                .MaximumLength(1000).WithMessage("Las notas deben tener máximo {MaxLength} caracteres.")
                .When(s => !string.IsNullOrEmpty(s.Notas));
        }
    }
}
