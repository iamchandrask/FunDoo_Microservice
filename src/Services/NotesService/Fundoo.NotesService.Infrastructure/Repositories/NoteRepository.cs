using Fundoo.NotesService.Application.Contracts;
using Fundoo.NotesService.Domain.Entities;
using Fundoo.NotesService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.NotesService.Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly NotesDbContext _dbContext;

    public NoteRepository(NotesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Note note, CancellationToken cancellationToken = default)
    {
        await _dbContext.Notes.AddAsync(note, cancellationToken);
    }

    public async Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Note>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notes
            .Where(x => x.UserId == userId && !x.IsTrashed)
            .OrderByDescending(x => x.IsPinned)
            .ThenByDescending(x => x.UpdatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        _dbContext.Notes.Update(note);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}