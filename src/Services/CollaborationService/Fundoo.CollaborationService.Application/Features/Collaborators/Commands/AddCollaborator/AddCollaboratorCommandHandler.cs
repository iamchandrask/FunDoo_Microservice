using Fundoo.CollaborationService.Application.Contracts;
using Fundoo.CollaborationService.Domain.Entities;
using MediatR;

namespace Fundoo.CollaborationService.Application.Features.Collaborators.Commands.AddCollaborator;

public class AddCollaboratorCommandHandler : IRequestHandler<AddCollaboratorCommand, Guid>
{
    private readonly ICollaboratorRepository _repository;

    public AddCollaboratorCommandHandler(ICollaboratorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddCollaboratorCommand command, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByNoteAndEmailAsync(
            command.Request.NoteId,
            command.Request.CollaboratorEmail,
            cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException("Collaborator already exists.");

        var collaborator = new Collaborator
        {
            NoteId = command.Request.NoteId,
            OwnerUserId = command.OwnerUserId,
            CollaboratorEmail = command.Request.CollaboratorEmail
        };

        await _repository.AddAsync(collaborator, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return collaborator.Id;
    }
}