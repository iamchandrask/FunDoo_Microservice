using Fundoo.NotesService.Domain.Entities;

namespace Fundoo.NotesService.Application.Contracts;

public interface INoteRepository
{
    Task AddAsync(Note note, CancellationToken cancellationToken = default);
    Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Note>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Note note, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}