using Fundoo.ReminderService.Domain.Entities;

namespace Fundoo.ReminderService.Application.Contracts;

public interface IReminderRepository
{
    Task AddAsync(Reminder reminder, CancellationToken cancellationToken = default);
    Task<Reminder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Reminder reminder, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}