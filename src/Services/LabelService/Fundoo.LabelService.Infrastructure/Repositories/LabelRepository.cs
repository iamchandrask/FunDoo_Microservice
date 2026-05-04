using Fundoo.LabelService.Application.Contracts;
using Fundoo.LabelService.Domain.Entities;
using Fundoo.LabelService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.LabelService.Infrastructure.Repositories;

public class LabelRepository : ILabelRepository
{
    private readonly LabelDbContext _dbContext;

    public LabelRepository(LabelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddLabelAsync(Label label, CancellationToken cancellationToken = default)
    {
        await _dbContext.Labels.AddAsync(label, cancellationToken);
    }

    public async Task<Label?> GetLabelByIdAsync(Guid labelId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Labels.FirstOrDefaultAsync(x => x.Id == labelId, cancellationToken);
    }

    public async Task AddNoteLabelAsync(NoteLabel noteLabel, CancellationToken cancellationToken = default)
    {
        await _dbContext.NotesLabels.AddAsync(noteLabel, cancellationToken);
    }

    public async Task<NoteLabel?> GetNoteLabelAsync(Guid noteId, Guid labelId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.NotesLabels
            .FirstOrDefaultAsync(x => x.NoteId == noteId && x.LabelId == labelId, cancellationToken);
    }

    public Task RemoveNoteLabelAsync(NoteLabel noteLabel, CancellationToken cancellationToken = default)
    {
        _dbContext.NotesLabels.Remove(noteLabel);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}