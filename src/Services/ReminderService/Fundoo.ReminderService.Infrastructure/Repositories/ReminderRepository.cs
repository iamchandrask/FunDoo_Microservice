using Fundoo.ReminderService.Application.Contracts;
using Fundoo.ReminderService.Domain.Entities;
using Fundoo.ReminderService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.ReminderService.Infrastructure.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly ReminderDbContext _dbContext;

    public ReminderRepository(ReminderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Reminder reminder, CancellationToken cancellationToken = default)
    {
        await _dbContext.Reminders.AddAsync(reminder, cancellationToken);
    }

    public async Task<Reminder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reminders.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task UpdateAsync(Reminder reminder, CancellationToken cancellationToken = default)
    {
        _dbContext.Reminders.Update(reminder);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}