using FluentValidation;
using Fundoo.NotesService.Application.DTOs;

namespace Fundoo.NotesService.Application.Validators;

public class CreateNoteRequestValidator : AbstractValidator<CreateNoteRequest>
{
    public CreateNoteRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000);

        RuleFor(x => x.Color)
            .NotEmpty()
            .Matches("^#([A-Fa-f0-9]{6})$")
            .WithMessage("Color must be a valid hex code like #FFFFFF.");
    }
}