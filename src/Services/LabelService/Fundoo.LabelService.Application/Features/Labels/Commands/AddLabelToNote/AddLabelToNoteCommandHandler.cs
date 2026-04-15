using Fundoo.LabelService.Application.Contracts;
using Fundoo.LabelService.Domain.Entities;
using MediatR;

namespace Fundoo.LabelService.Application.Features.Labels.Commands.AddLabelToNote;

public class AddLabelToNoteCommandHandler : IRequestHandler<AddLabelToNoteCommand, bool>
{
    private readonly ILabelRepository _repository;

    public AddLabelToNoteCommandHandler(ILabelRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(AddLabelToNoteCommand command, CancellationToken cancellationToken)
    {
        var noteLabel = new NoteLabel
        {
            NoteId = command.Request.NoteId,
            LabelId = command.Request.LabelId
        };

        await _repository.AddNoteLabelAsync(noteLabel, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}