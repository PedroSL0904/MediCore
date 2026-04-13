using FluentValidation;
using MediCore.Api.DTOs;

namespace MediCore.Api.Validations
{
    // Heredamos de AbstractValidator y le decimos qué DTO vamos a vigilar
    public class PacienteRegistroValidator : AbstractValidator<PacienteRegistroDto>
    {
        public PacienteRegistroValidator()
        {
            // Reglas para el Nombre
            RuleFor(x => x.NombreCompleto)
                .NotEmpty().WithMessage("El nombre del paciente es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(150).WithMessage("El nombre es demasiado largo.");

            // Reglas para el Teléfono (Solo si decidieron enviarlo)
            RuleFor(x => x.Telefono)
                .Matches(@"^\d{10}$").WithMessage("El teléfono debe tener exactamente 10 dígitos numéricos.")
                .When(x => !string.IsNullOrEmpty(x.Telefono));
        }
    }
}