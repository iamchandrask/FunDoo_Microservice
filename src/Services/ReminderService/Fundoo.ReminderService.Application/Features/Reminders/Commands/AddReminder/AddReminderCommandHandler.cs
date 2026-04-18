using Fundoo.ReminderService.Application.Contracts;
using Fundoo.ReminderService.Domain.Entities;
using MediatR;

namespace Fundoo.ReminderService.Application.Features.Reminders.Commands.AddReminder;

public class AddReminderCommandHandler : IRequestHandler<AddReminderCommand, Guid>
{
    private readonly IReminderRepository _repository;

    public AddReminderCommandHandler(IReminderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddReminderCommand command, CancellationToken cancellationToken)
    {
        var reminder = new Reminder
        {
            NoteId = command.Request.NoteId,
            ReminderTime = command.Request.ReminderTime,
            Message = command.Request.Message
        };

        await _repository.AddAsync(reminder, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return reminder.Id;
    }
}