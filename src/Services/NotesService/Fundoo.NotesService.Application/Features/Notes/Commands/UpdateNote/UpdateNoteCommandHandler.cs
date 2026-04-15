using Fundoo.NotesService.Application.Contracts;
using MediatR;

namespace Fundoo.NotesService.Application.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, bool>
{
    private readonly INoteRepository _repository;
    private readonly INotesCacheService _cacheService;

    public UpdateNoteCommandHandler(INoteRepository repository, INotesCacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<bool> Handle(UpdateNoteCommand command, CancellationToken cancellationToken)
    {
        var note = await _repository.GetByIdAsync(command.NoteId, cancellationToken);

        if (note is null || note.UserId != command.UserId)
            throw new KeyNotFoundException("Note not found.");

        note.Title = command.Request.Title;
        note.Description = command.Request.Description;
        note.Color = command.Request.Color;
        note.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(note, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync(command.UserId);

        return true;
    }
}