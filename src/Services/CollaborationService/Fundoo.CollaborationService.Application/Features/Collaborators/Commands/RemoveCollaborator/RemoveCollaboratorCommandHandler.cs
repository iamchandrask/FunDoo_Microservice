using Fundoo.CollaborationService.Application.Contracts;
using MediatR;

namespace Fundoo.CollaborationService.Application.Features.Collaborators.Commands.RemoveCollaborator;

public class RemoveCollaboratorCommandHandler : IRequestHandler<RemoveCollaboratorCommand, bool>
{
    private readonly ICollaboratorRepository _repository;

    public RemoveCollaboratorCommandHandler(ICollaboratorRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(RemoveCollaboratorCommand command, CancellationToken cancellationToken)
    {
        var collaborator = await _repository.GetByNoteAndEmailAsync(
            command.Request.NoteId,
            command.Request.CollaboratorEmail,
            cancellationToken);

        if (collaborator is null)
            throw new KeyNotFoundException("Collaborator not found.");

        await _repository.RemoveAsync(collaborator, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}