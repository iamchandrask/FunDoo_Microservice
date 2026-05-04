using Fundoo.ReminderService.Application.Contracts;
using MediatR;

namespace Fundoo.ReminderService.Application.Features.Reminders.Commands.UpdateReminder;

public class UpdateReminderCommandHandler : IRequestHandler<UpdateReminderCommand, bool>
{
    private readonly IReminderRepository _repository;

    public UpdateReminderCommandHandler(IReminderRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateReminderCommand command, CancellationToken cancellationToken)
    {
        var reminder = await _repository.GetByIdAsync(command.ReminderId, cancellationToken);

        if (reminder is null)
            throw new KeyNotFoundException("Reminder not found.");

        reminder.ReminderTime = command.Request.ReminderTime;
        reminder.Message = command.Request.Message;
        reminder.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(reminder, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}