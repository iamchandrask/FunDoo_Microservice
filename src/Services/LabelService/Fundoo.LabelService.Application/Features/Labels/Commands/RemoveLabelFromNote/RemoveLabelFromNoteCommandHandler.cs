using Fundoo.LabelService.Application.Contracts;
using MediatR;

namespace Fundoo.LabelService.Application.Features.Labels.Commands.RemoveLabelFromNote;

public class RemoveLabelFromNoteCommandHandler : IRequestHandler<RemoveLabelFromNoteCommand, bool>
{
    private readonly ILabelRepository _repository;

    public RemoveLabelFromNoteCommandHandler(ILabelRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(RemoveLabelFromNoteCommand command, CancellationToken cancellationToken)
    {
        var noteLabel = await _repository.GetNoteLabelAsync(
            command.Request.NoteId,
            command.Request.LabelId,
            cancellationToken);

        if (noteLabel is null)
            throw new KeyNotFoundException("Label mapping not found.");

        await _repository.RemoveNoteLabelAsync(noteLabel, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}