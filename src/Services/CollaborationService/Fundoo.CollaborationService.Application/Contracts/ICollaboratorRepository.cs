using Fundoo.CollaborationService.Domain.Entities;

namespace Fundoo.CollaborationService.Application.Contracts;

public interface ICollaboratorRepository
{
    Task AddAsync(Collaborator collaborator, CancellationToken cancellationToken = default);
    Task<Collaborator?> GetByNoteAndEmailAsync(Guid noteId, string email, CancellationToken cancellationToken = default);
    Task RemoveAsync(Collaborator collaborator, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}