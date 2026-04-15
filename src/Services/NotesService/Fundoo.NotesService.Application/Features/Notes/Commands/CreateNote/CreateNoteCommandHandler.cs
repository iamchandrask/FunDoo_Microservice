using Fundoo.NotesService.Application.Contracts;
using Fundoo.NotesService.Domain.Entities;
using MediatR;

namespace Fundoo.NotesService.Application.Features.Notes.Commands.CreateNote;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Guid>
{
    private readonly INoteRepository _repository;
    private readonly INotesCacheService _cacheService;

    public CreateNoteCommandHandler(INoteRepository repository, INotesCacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<Guid> Handle(CreateNoteCommand command, CancellationToken cancellationToken)
    {
        var note = new Note
        {
            UserId = command.UserId,
            Title = command.Request.Title,
            Description = command.Request.Description,
            Color = command.Request.Color
        };

        await _repository.AddAsync(note, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync(command.UserId);

        return note.Id;
    }
}