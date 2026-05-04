using Fundoo.ReminderService.Application.DTOs;
using MediatR;

namespace Fundoo.ReminderService.Application.Features.Reminders.Commands.UpdateReminder;

public record UpdateReminderCommand(Guid ReminderId, UpdateReminderRequest Request) : IRequest<bool>;