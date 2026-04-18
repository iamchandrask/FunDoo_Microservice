using Fundoo.ReminderService.Application.DTOs;
using MediatR;

namespace Fundoo.ReminderService.Application.Features.Reminders.Commands.AddReminder;

public record AddReminderCommand(AddReminderRequest Request) : IRequest<Guid>;