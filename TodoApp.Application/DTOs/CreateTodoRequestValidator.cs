using FluentValidation;

namespace TodoApp.Application.DTOs;

public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Le titre est obligatoire.")
            .MaximumLength(100).WithMessage("Le titre ne doit pas dépasser 100 caractères.");

        RuleFor(x => x.DueDate)
            .Must(date => !date.HasValue || date.Value > DateTime.Now)
            .WithMessage("La date d'échéance ne peut pas être dans le passé.");
    }
}