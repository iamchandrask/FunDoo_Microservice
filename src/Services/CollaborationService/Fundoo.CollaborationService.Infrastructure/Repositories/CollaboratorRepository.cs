using Fundoo.CollaborationService.Application.Contracts;
using Fundoo.CollaborationService.Domain.Entities;
using Fundoo.CollaborationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.CollaborationService.Infrastructure.Repositories;

public class CollaboratorRepository : ICollaboratorRepository
{
    private readonly CollaborationDbContext _dbContext;

    public CollaboratorRepository(CollaborationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Collaborator collaborator, CancellationToken cancellationToken = default)
    {
        await _dbContext.Collaborators.AddAsync(collaborator, cancellationToken);
    }

    public async Task<Collaborator?> GetByNoteAndEmailAsync(Guid noteId, string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Collaborators
            .FirstOrDefaultAsync(x => x.NoteId == noteId && x.CollaboratorEmail == email, cancellationToken);
    }

    public Task RemoveAsync(Collaborator collaborator, CancellationToken cancellationToken = default)
    {
        _dbContext.Collaborators.Remove(collaborator);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}